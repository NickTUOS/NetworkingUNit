using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Helper.Net.RUDP
{

	#region Abstract NATHistoryPoint

	/// <summary>
	/// Sometimes we learn things about the NAT we may be behind.
	/// This class represents the data we learn
	/// </summary>
	public abstract class NATHistoryPoint
	{

		#region Variables

		protected DateTime _date;
		protected IPEndPoint _localEndPoint;
		protected IPEndPoint _remoteEndPoint;
		protected IPEndPoint _peerViewOfLocalEndPoint;
		protected IPEndPoint _previousEndPoint;

		// Store the socket handle to avoid garbage collection
		// to clean it
		protected int _socketHandle;

		#endregion

		#region Constructor

		public NATHistoryPoint(DateTime date, RUDPSocket socket)
		{
			_date = date;
			_socketHandle = socket.Handle;
			_localEndPoint = socket.LocalEndPoint;
			_remoteEndPoint = socket.RemoteEndPoint;
		}

		#endregion

		#region Properties

		public DateTime DateTime
		{
			get
			{
				return _date;
			}
		}

		public IPEndPoint LocalEndPoint
		{
			get
			{
				return _localEndPoint;
			}
		}

		public IPEndPoint PeerViewOfLocalEndPoint
		{
			get
			{
				return _peerViewOfLocalEndPoint;
			}
		}

		public IPEndPoint RemoteEndPoint
		{
			get
			{
				return _remoteEndPoint;
			}
		}

		/// <summary>
		/// When the mapping changes, this is the previous IPEndPoint.
		/// </summary>
		public IPEndPoint PreviousEndPoint
		{
			get
			{
				return _previousEndPoint;
			}
		}

		/// <summary>
		/// So we don't keep a reference to the socket, thereby potentially never allowing
		/// garbage collection, each socket is assigned a unique number (handle)
		/// This is a unique mapping for the life of the socket.
		/// </summary>
		public int SocketHandle
		{
			get
			{
				return _socketHandle;
			}
		}

		#endregion

	}

	#endregion

	#region NewSocketPoint

	/// <summary>
	/// When a new socket is created this is the point
	/// </summary>
	sealed public class NewSocketPoint : NATHistoryPoint
	{
		public NewSocketPoint(DateTime date, RUDPSocket socket)
			: base(date, socket)
		{
		}
	}

	#endregion

	#region SocketClosePoint

	/// <summary>
	/// When a socket closes, we note it
	/// </summary>
	sealed public class SocketClosePoint : NATHistoryPoint
	{
		public SocketClosePoint(DateTime date, RUDPSocket socket)
			: base(date, socket)
		{
		}
	}

	#endregion

	#region LocalMappingChangePoint

	/// <summary>
	/// When the local mapping changes, record it here.
	/// </summary>
	sealed public class LocalMappingChangePoint : NATHistoryPoint
	{
		public LocalMappingChangePoint(DateTime date, RUDPSocket socket, IPEndPoint newEndPoint)
			: base(date, socket)
		{
			_peerViewOfLocalEndPoint = newEndPoint;
		}
	}

	#endregion

	#region RemoteMappingChangePoint

	/// <summary>
	/// When the local mapping changes, record it here.
	/// </summary>
	sealed public class RemoteMappingChangePoint : NATHistoryPoint
	{
		public RemoteMappingChangePoint(DateTime date, RUDPSocket socket)
			: base(date, socket)
		{
		}
	}

	#endregion

}
