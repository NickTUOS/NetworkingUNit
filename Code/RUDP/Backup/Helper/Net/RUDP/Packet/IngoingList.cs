using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Helper.ThreadingNET35;

namespace Helper.Net.RUDP
{
	internal sealed class IngoingList
	{

		#region Variables

		private object _lock = new object();
		private LinkedList<RUDPIngoingPacket> _list = new LinkedList<RUDPIngoingPacket>();
		private int _currentPacketId = -1;
		private int _biggestPacketId = -1;

		#endregion

		#region AddPacket

		internal void AddPacket(RUDPIngoingPacket packet)
		{
			lock (_lock)
			{
				_biggestPacketId = Math.Max(_biggestPacketId, packet.PacketId);

				if (packet.PacketId < 0)
				{
					_list.AddLast(packet);
					return;
				}

				LinkedListNode<RUDPIngoingPacket> node = _list.Last;
				LinkedListNode<RUDPIngoingPacket> firstNonReliableNode = null;
				while (node != null && (node.Value.PacketId < 0 || packet.PacketId < node.Value.PacketId))
				{
					if (firstNonReliableNode == null && node.Value.PacketId < 0)
						firstNonReliableNode = node;

					node = node.Previous;

					if (node != null && packet.PacketId < node.Value.PacketId)
						firstNonReliableNode = null;
				}

				// Just after a non reliable packet
				if (firstNonReliableNode != null)
				{
					_list.AddAfter(firstNonReliableNode, packet);
					return;
				}

				if (node == null)
				{
					_list.AddFirst(packet);
					return;
				}

				// If duplicated packet -> drop
				if (packet.PacketId == node.Value.PacketId)
					return;

				// Add
				_list.AddAfter(node, packet);
			}
		}

		#endregion

		#region RemoveNextPacket

		/// <summary>
		/// Returns the next packet to process and remove it.
		/// </summary>
		/// <returns></returns>
		internal RUDPIngoingPacket RemoveNextPacket()
		{
			lock (_lock)
			{
				LinkedListNode<RUDPIngoingPacket> node = _list.First;
				if (node == null)
					return null;

				// Next reliable packet
				if (node.Value.PacketId > -1 && node.Value.PacketId == _currentPacketId + 1)
				{
					_currentPacketId++;
					_list.Remove(node);
					return node.Value;
				}

				// Search for the first non-reliable packet
				while (node != null)
					if (node.Value.PacketId < 0)
					{
						_list.Remove(node);
						return node.Value;
					}
					else
						node = node.Next;
			}

			return null;
		}

		#endregion

		#region Properties

		internal int Count
		{
			get
			{
				return _list.Count;
			}
		}

		internal int CurrentPacketId
		{
			get
			{
				return _currentPacketId;
			}
			set
			{
				_currentPacketId = value;
			}
		}

		internal int BiggestPacketId
		{
			get
			{
				return _biggestPacketId;
			}
		}

		#endregion

		#region Clear

		internal void Clear()
		{
			lock (_lock)
			{
				_list.Clear();
				_currentPacketId = -1;
				_biggestPacketId = -1;
			}
		}

		#endregion

		#region ContainsPacket

		internal bool ContainsPacket(int packetId)
		{
			lock (_lock)
			{

				LinkedListNode<RUDPIngoingPacket> node = _list.First;
				while (node != null)
					if (node.Value.PacketId != packetId)
						node = node.Next;
					else
						return true;

			}

			return false;
		}

		#endregion

	}
}
