using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Helper.Net.RUDP
{
	#region NATEndPointsManager

	/// <summary>
	/// This class create the end points for a given history
	/// </summary>
	sealed public class NATEndPointsManager
	{

		#region Variables

		private readonly List<NATHistoryPoint> _history;
		private volatile List<IPAddressRecord> _remoteIPAddresses;
		private readonly List<IPEndPoint> _localEndPoints;
		private volatile List<IPEndPoint> _generatedEndPoints;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="history">history information learned from talking to peers (may be null)</param>
		/// <param name="localEndPoints">the list of end points to use as last resort</param>
		public NATEndPointsManager(List<IPEndPoint> localEndPoints, List<NATHistoryPoint> history)
		{
			_history = history;
			_localEndPoints = localEndPoints;
		}

		#endregion

		#region GetAllNATHandlers

		/// <summary>
		/// Returns all the NAT handlers
		/// </summary>
		/// <returns></returns>
		static private List<NATHandler> GetAllNATHandlers()
		{
			List<NATHandler> handlers = new List<NATHandler>(5);
			handlers.Add(new PublicNATHandler());
			handlers.Add(new ConeNATHandler());
			handlers.Add(new LinuxNATHandler());
			handlers.Add(new SymmetricNATHandler());
			handlers.Add(new NullNATHandler());

			return handlers;
		}

		#endregion

		#region InitRemoteIPAddresses

		private static bool PredicateFindAllIPAddresses(NATHistoryPoint historyPoint)
		{
			IPEndPoint endPoint = historyPoint.PeerViewOfLocalEndPoint;
			if (endPoint != null)
				return true;

			return false;
		}

		private void InitRemoteIPAddresses()
		{
			List<NATHistoryPoint> allAddresses = _history.FindAll(PredicateFindAllIPAddresses);

			//---- Create a list of records
			Dictionary<IPAddress, IPAddressRecord> records = new Dictionary<IPAddress, IPAddressRecord>();
			foreach (NATHistoryPoint historyPoint in allAddresses)
				if (!records.ContainsKey(historyPoint.PeerViewOfLocalEndPoint.Address))
				{
					IPAddressRecord record = new IPAddressRecord();
					record.IPAddress = historyPoint.PeerViewOfLocalEndPoint.Address;
					record.Count = 1;
					records[historyPoint.PeerViewOfLocalEndPoint.Address] = record;
				}
				else
					records[historyPoint.PeerViewOfLocalEndPoint.Address].Count++;

			//---- Sort the list based on count
			List<IPAddressRecord> addresses = new List<IPAddressRecord>();
			foreach (IPAddressRecord record in records.Values)
				addresses.Add(record);

			// Now we have a list of the most used to least used IPs
			addresses.Sort();

			//---- Set it
			_remoteIPAddresses = addresses;
		}

		#endregion

		#region GenerateEndPoints

		/// Give all the NATHistoryPoint that have a PeerViewOfLocalEndPoint matching the
		/// giving IPAddress
		private List<NATHistoryPoint> GetNATHistoryPointsForIPAddress(List<NATHistoryPoint> history, IPAddress address)
		{
			List<NATHistoryPoint> result = new List<NATHistoryPoint>();
			foreach (NATHistoryPoint historyPoint in history)
				if (historyPoint.PeerViewOfLocalEndPoint != null && address.Equals(historyPoint.PeerViewOfLocalEndPoint.Address))
					result.Add(historyPoint);

			return result;
		}

		private void GenerateEndPoints()
		{
			List<IPEndPoint> endPoints = new List<IPEndPoint>();
			Dictionary<IPEndPoint, bool> ht = new Dictionary<IPEndPoint, bool>();
			if (_history != null)
			{
				//----- We go through the list from most likely to least likely
				if (_remoteIPAddresses == null)
					InitRemoteIPAddresses();

				foreach (IPAddressRecord record in _remoteIPAddresses)
				{
					List<NATHistoryPoint> points = GetNATHistoryPointsForIPAddress(_history, record.IPAddress);
					List<NATHandler> handlers = NATEndPointsManager.GetAllNATHandlers();
					bool yielded = false;
					for (int index = 0; index < handlers.Count && !yielded; index++)
						if (handlers[index].CanHandleNAT(points))
						{
							foreach (IPEndPoint endPoint in handlers[index].TargetEndPoints(points))
								if (!ht.ContainsKey(endPoint))
								{
									ht[endPoint] = true;
									endPoints.Add(endPoint);
								}

							// Break out of the while loop, we found the handler.
							yielded = true;
						}
				}
			}

			//---- Now we should yield the locally configured points:
			foreach (IPEndPoint endPoint in _localEndPoints)
				if (!ht.ContainsKey(endPoint))
					endPoints.Add(endPoint);

			//---- Set it
			_generatedEndPoints = endPoints;
		}

		#endregion

		#region EndPoints

		public List<IPEndPoint> EndPoints
		{
			get
			{
				if (_generatedEndPoints == null)
					GenerateEndPoints();

				return _generatedEndPoints;
			}
		}

		#endregion

	}

	#endregion

	#region IPAddressRecord

	/// <summary>
	/// Record an IPAddress and the count of occurences.
	/// </summary>
	internal sealed class IPAddressRecord : IComparable
	{
		public IPAddress IPAddress;
		public int Count;

		/// Sort them from largest count to least count
		public int CompareTo(object o)
		{
			if (this == o)
				return 0;

			IPAddressRecord other = o as IPAddressRecord;
			if (Count > other.Count)
				return -1;

			if (Count < other.Count)
				return 1;

			return 0;
		}
	}

	#endregion
}
