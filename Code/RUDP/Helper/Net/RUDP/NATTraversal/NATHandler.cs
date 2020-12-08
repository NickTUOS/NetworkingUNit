using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Helper.Net.RUDP
{

	#region NATHandler

	/// <summary>
	/// All NatHandlers are subclasses of this class.
	/// </summary>
	public abstract class NATHandler
	{

		#region CanHandleNAT

		/// <summary>
		/// Use the history to check if the current NAT handler can handle this kind of NAT.
		/// Returns 'true' if it is the right NAT handler.
		/// </summary>
		abstract public bool CanHandleNAT(List<NATHistoryPoint> history);

		#endregion

		#region TargetEndPoints

		/// <summary>
		/// Returns a list of IPEndPoint which should correspond to the local NAT.
		/// </summary>
		virtual public List<IPEndPoint> TargetEndPoints(List<NATHistoryPoint> history)
		{
			// Put each end point in once, but the most recently used ones should be first:
			List<IPEndPoint> endPoints = new List<IPEndPoint>();

			foreach (NATHistoryPoint historyPoint in history)
			{
				IPEndPoint endPoint = historyPoint.PeerViewOfLocalEndPoint;
				if (endPoint != null && !endPoints.Contains(endPoint))
					endPoints.Add(endPoint);
			}

			return endPoints;
		}

		#endregion

	}

	#endregion

	#region PublicNATHandler

	/// <summary>
	/// For a public node, there is never any port translation,
	/// so, local ports always match remote ports.
	/// 
	/// We assume here only one port is used.
	/// </summary>
	sealed public class PublicNATHandler : NATHandler
	{

		#region CanHandleNAT

		override public bool CanHandleNAT(List<NATHistoryPoint> history)
		{
			int port = 0;
			bool port_is_set = false;
			bool retv = true;

			foreach (NATHistoryPoint p in history)
			{
				int this_port = p.LocalEndPoint.Port;
				if (!port_is_set)
				{
					port = this_port;
					port_is_set = true;
				}
				else
					retv = retv && (this_port == port);

				IPEndPoint pv = p.PeerViewOfLocalEndPoint;

				// Check that everything is okay:
				if (pv != null)
					retv = retv && (pv.Port == port);

				if (retv == false)
					break;
			}

			return retv;
		}

		#endregion

		#region TargetEndPoints

		/// This is easy, just return the most recent non-null PeerViewTA.
		override public List<IPEndPoint> TargetEndPoints(List<NATHistoryPoint> history)
		{
			List<IPEndPoint> list = new List<IPEndPoint>();
			IPEndPoint local = null;
			foreach (NATHistoryPoint p in history)
			{
				// Get the most recent local end point
				if (local == null)
					local = p.LocalEndPoint;

				IPEndPoint pv = p.PeerViewOfLocalEndPoint;
				if (pv != null)
				{
					list.Add(pv);
					break;
				}
			}

			// We never found one, just use a local one
			if (list.Count == 0 && local != null)
				list.Add(local);

			return list;
		}

		#endregion

	}

	#endregion

	#region NullNATHandler

	/// <summary>
	/// This is some kind of default handler which is a last resort mode
	/// The algorithm here is to just return the full history of reported
	/// EndPoints or localEndPoints in the order of most recently to least recently used
	/// </summary>
	sealed public class NullNATHandler : NATHandler
	{

		/// <summary>
		/// This NATHandler thinks it can handle anything.
		/// </summary>
		override public bool CanHandleNAT(List<NATHistoryPoint> history)
		{
			return true;
		}

	}

	#endregion

	#region ConeNATHandler

	/// <summary>
	/// Handles Cone Nats
	/// </summary>
	sealed public class ConeNATHandler : NATHandler
	{

		#region CanHandleNAT

		/// The cone nat uses exactly one port on each IP address
		override public bool CanHandleNAT(List<NATHistoryPoint> h)
		{
			/*
			 We go through the list.  At each moment we see how many active
			 ports there are.  A cone nat should be using 1, but due to NAT mapping
			 changes, it may take us a little while to notice a change, which may
			 make it appear that we are using 2 ports (assuming we notice much faster
			 than the changes happen).
			 It is also a neccesary condition that we have connections to at least two
			 IP addresses using the same port (part of the definition of a cone nat).
			 */
			bool multiple_remote_ip_same_port = false;
			int max_active_ports = 0;
			Dictionary<int, List<IPEndPoint>> portToRemoteMapping = new Dictionary<int, List<IPEndPoint>>();

			// Reverse the list to go from least recent to most recent:
			List<NATHistoryPoint> allEndPoints = new List<NATHistoryPoint>();
			foreach (NATHistoryPoint dp in h)
				allEndPoints.Add(dp);
			allEndPoints.Reverse();

			Dictionary<int, NATHistoryPoint> edge_no_to_most_recent_point = new Dictionary<int, NATHistoryPoint>();
			bool is_cone = true;
			foreach (NATHistoryPoint historyPoint in allEndPoints)
			{
				IPEndPoint endPoint = historyPoint.PeerViewOfLocalEndPoint;
				if (endPoint == null)
					continue;

				int port = endPoint.Port;
				NATHistoryPoint oldHistoryPoint = edge_no_to_most_recent_point[historyPoint.SocketHandle];

				if (historyPoint is NewSocketPoint)
				{
					// There is a new mapping:
					List<IPEndPoint> l = AddRemoteEndPoint(portToRemoteMapping, port, historyPoint.RemoteEndPoint);
					multiple_remote_ip_same_port |= (l.Count > 1);
					max_active_ports = Math.Max(max_active_ports, portToRemoteMapping.Count);
				}
				else if (historyPoint is SocketClosePoint)
				{
					// Remove a mapping, this obviously can't increase the number of active ports,
					// or change the multiple_remote_ip_same_port variable
					RemoveRemoteEndPoint(portToRemoteMapping, port, historyPoint.RemoteEndPoint);
				}
				else if (historyPoint is LocalMappingChangePoint)
				{
					//Look at the old port, remove from the old port, and put into the new port.
					if (oldHistoryPoint != null)
					{
						int old_port = oldHistoryPoint.PeerViewOfLocalEndPoint.Port;
						RemoveRemoteEndPoint(portToRemoteMapping, old_port, oldHistoryPoint.RemoteEndPoint);
					}
					List<IPEndPoint> l = AddRemoteEndPoint(portToRemoteMapping, port, historyPoint.RemoteEndPoint);
					multiple_remote_ip_same_port |= (l.Count > 1);
					max_active_ports = Math.Max(max_active_ports, portToRemoteMapping.Count);
				}
				else if (historyPoint is RemoteMappingChangePoint)
				{
					//Remove the old RemoteEndPoint, and put in the new one:
					if (oldHistoryPoint != null)
						RemoveRemoteEndPoint(portToRemoteMapping, port, oldHistoryPoint.RemoteEndPoint);

					List<IPEndPoint> l = AddRemoteEndPoint(portToRemoteMapping, port, historyPoint.RemoteEndPoint);
					multiple_remote_ip_same_port |= (l.Count > 1);
					max_active_ports = Math.Max(max_active_ports, portToRemoteMapping.Count);
				}

				edge_no_to_most_recent_point[historyPoint.SocketHandle] = historyPoint;
				//is_cone = (max_active_ports < 3) && ( multiple_remote_ip_same_port);
				is_cone = (max_active_ports < 3);

				// We can stop now, we are clearly not in the cone case.
				if (!is_cone)
					break;
			}

			return is_cone;
		}

		#endregion

		#region AddRemoteEndPoint

		// Returns the set of TAs for the given port
		private List<IPEndPoint> AddRemoteEndPoint(Dictionary<int, List<IPEndPoint>> dictionary, int port, IPEndPoint endPoint)
		{
			List<IPEndPoint> list = dictionary[port];
			if (list == null)
				list = new List<IPEndPoint>();

			if (!list.Contains(endPoint))
				list.Add(endPoint);

			dictionary[port] = list;

			return list;
		}

		#endregion

		#region RemoveRemoteEndPoint

		// Remote the TA from the set
		private void RemoveRemoteEndPoint(Dictionary<int, List<IPEndPoint>> dictionary, int port, IPEndPoint endPoint)
		{
			List<IPEndPoint> list = dictionary[port];

			list.Remove(endPoint);

			// Get this out of here.
			if (list.Count == 0)
				dictionary.Remove(port);
		}

		#endregion

		#region TargetEndPoints

		/// <summary>
		/// Returns the list of TAs that should be tried.
		/// </summary>
		override public List<IPEndPoint> TargetEndPoints(List<NATHistoryPoint> history)
		{
			// The trick here is, for a cone nat, we should only report
			// the most recently used ip/port pair. 
			// For safety, we return the most recent two
			List<IPEndPoint> endPoints = new List<IPEndPoint>();
			foreach (NATHistoryPoint historyPoint in history)
			{
				IPEndPoint last_reported = historyPoint.PeerViewOfLocalEndPoint;
				if (last_reported != null)
				{
					if (endPoints.Count == 0 || !last_reported.Equals(endPoints[0]))
						endPoints.Add(last_reported);

					if (endPoints.Count == 2)
						return endPoints;
				}
			}

			return endPoints;
		}

		#endregion

	}

	#endregion

	#region SymmetricNATHandler

	public class SymmetricNATHandler : NATHandler
	{

		#region Variables

		/// How many std. dev. on each side of the mean to use
		protected static readonly double SAFETY = 2.0;
		protected static readonly double MAX_STD_DEV = 5.0;

		#endregion

		#region CanHandleNAT

		override public bool CanHandleNAT(List<NATHistoryPoint> history)
		{
			return (0 < PredictPorts(history).Count);
		}

		#endregion

		#region TargetEndPoints

		override public List<IPEndPoint> TargetEndPoints(List<NATHistoryPoint> history)
		{
			return PredictPorts(history);
		}

		#endregion

		#region PredictPorts

		/// <summary>
		/// Given an IEnumerable of NatDataPoints, return a list of next port used by the NAT
		/// </summary>
		/// <returns>an empty list if this is not our type</returns>
		protected List<IPEndPoint> PredictPorts(List<NATHistoryPoint> ndps)
		{
			int allDifferencesCount = 0;

			// Get an increasing subset of the ports:
			int prev = Int32.MinValue;
			int most_recent_port = -1;
			uint sum = 0;
			uint sum2 = 0;
			bool got_extra_data = false;
			IPAddress host = null;
			foreach (NATHistoryPoint historyPoint in ndps)
			{
				// Ignore closing events for prediction, they'll screw up the port prediction
				if (!(historyPoint is SocketClosePoint))
				{
					IPEndPoint endPoint = historyPoint.PeerViewOfLocalEndPoint;
					if (endPoint != null)
					{
						int port = endPoint.Port;
						
						if (!got_extra_data)
						{
							host = endPoint.Address;
							most_recent_port = port;
							got_extra_data = true;
						}

						if (prev > port)
						{
							uint diff = (uint)(prev - port); // Clearly diff is always non-neg
							allDifferencesCount++;
							sum += diff;
							sum2 += diff * diff;
						}
						prev = port;
					}
				}
			}

			//---- Now look at the mean and variance of the diffs
			List<IPEndPoint> prediction = new List<IPEndPoint>();
			if (allDifferencesCount > 1)
			{
				double n = (double)allDifferencesCount;
				double sd = (double)sum;
				double mean = sd / n;
				double s2 = ((double)sum2) - sd * sd / n;
				s2 = s2 / (double)(allDifferencesCount - 1);
				double stddev = Math.Sqrt(s2);
				
				if (stddev < MAX_STD_DEV)
				{
					try
					{
						double max_delta = mean + SAFETY * stddev;
						if (max_delta < mean + 0.001)
						{
							// This means the stddev is very small, just go up one above the
							// mean:
							max_delta = mean + 1.001;
						}

						int delta = (int)(mean - SAFETY * stddev);
						while (delta < max_delta)
						{
							if (delta > 0)
							{
								int pred_port = most_recent_port + delta;
								prediction.Add(new IPEndPoint(host, pred_port));
							}
							else
							{
								// Increment the max by one just to keep a constant width:
								max_delta += 1.001; //Giving a little extra to make sure we get 1
							}
							delta++;
						}
					}
					catch
					{
						//Just ignore any bad transport addresses.
					}
				}
				else
				{
					//The standard deviation is too wide to make a meaningful prediction
				}
			}

			return prediction;
		}

		#endregion

	}

	#endregion

	#region LinuxNATHandler

	/// <summary>
	/// The standard IPTables NAT in Linux is similar to a symmetric NAT.
	/// It will try to avoid translating the port number, but if it can't
	/// (due to another node behind the NAT already using that port to contact
	/// the same remote IP/port), then it will assign a new port. 
	///
	/// So, we should try to use the "default" port first, but if that doesn't
	/// work, use port prediction.
	/// </summary>
	sealed public class LinuxNATHandler : SymmetricNATHandler
	{

		#region CanHandleNAT

		/// <summary>
		/// Check to see that at least some of the remote ports match the local port 
		/// </summary>
		override public bool CanHandleNAT(List<NATHistoryPoint> history)
		{
			bool retv = false;
			MakeTargets(history, out retv);
			return retv;
		}

		#endregion

		#region TargetEndPoints

		public override List<IPEndPoint> TargetEndPoints(List<NATHistoryPoint> h)
		{
			bool success = false;
			List<IPEndPoint> result = MakeTargets(h, out success);

			if (success)
				return result;

			return new List<IPEndPoint>();
		}

		#endregion

		#region MakeTargets

		protected List<IPEndPoint> MakeTargets(List<NATHistoryPoint> h, out bool success)
		{
			bool there_is_a_match = false;
			int matched_port = 0;
			IPEndPoint matched_ta = null;

			foreach (NATHistoryPoint p in h)
			{
				IPEndPoint l = p.LocalEndPoint;
				IPEndPoint pv = p.PeerViewOfLocalEndPoint;
				if (l != null && pv != null)
				{
					there_is_a_match = (l.Port == pv.Port);
					if (there_is_a_match)
					{
						//Move on.
						matched_port = l.Port;
						matched_ta = pv;
						break;
					}
				}
			}

			//---- Success
			if (there_is_a_match)
			{
				// This is all the non-matching history points
				List<NATHistoryPoint> non_matched = new List<NATHistoryPoint>();
				foreach (NATHistoryPoint historyPoint in h)
					if (historyPoint.PeerViewOfLocalEndPoint != null && historyPoint.PeerViewOfLocalEndPoint.Port != matched_port)
						non_matched.Add(historyPoint);

				List<IPEndPoint> l = PredictPorts(non_matched);

				// Put in the matched port at the top of the list:
				l.Insert(0, matched_ta);
				success = true;

				return l;
			}
			

			//---- Failed
			success = false;
			return null;
		}

		#endregion

	}

	#endregion

}
