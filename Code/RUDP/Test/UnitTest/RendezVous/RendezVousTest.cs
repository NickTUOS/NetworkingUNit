using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

using Helper.Net.RUDP;

namespace Test.UnitTest.RendezVous
{
	#region RendezVousTest

	public class RendezVousTest : UnitTest
	{
		public RUDPSocket Socket;

		int clientPort1 = GetAvailablePort();
		int clientPort2 = GetAvailablePort();

		override public void Execute(bool executeClient, bool executeServer)
		{
			Socket = new RUDPSocket();

			//---- Client 1
			ClientSocket cs = new ClientSocket(clientPort1, clientPort2);
			Thread t = new Thread(new ThreadStart(cs.Start));
			t.Start();

			Thread.Sleep(5000);

			//---- Client 2
			cs = new ClientSocket(clientPort2, clientPort1);
			cs.Start();
		}
	}

	#endregion

	#region ClientSocket

	public class ClientSocket
	{
		static public int Number = 0;

		public RUDPSocket Socket;
		public int BindPort;
		public int ConnectPort;

		public int MyNumber;
		public int SendIteration = 0;
		public int ReceiveIteration = 0;

		public ClientSocket(int bindPort, int connectPort)
		{
			Socket = new RUDPSocket();

			BindPort = bindPort;
			ConnectPort = connectPort;

			MyNumber = ++Number;
		}

		public void Start()
		{
			Socket.Bind(UnitTest.CreateLocalEndPoint(BindPort));
			Socket.IsRendezVousMode = true;
			Socket.Connect(UnitTest.CreateLocalEndPoint(ConnectPort));

			if (!Socket.Connected)
				Debug.Fail("Socket not connected");

			byte[] buffer = new byte[4 + 4 + SendIteration];
			Array.Copy(BitConverter.GetBytes(MyNumber), buffer, 4);
			Array.Copy(BitConverter.GetBytes(SendIteration), 0, buffer, 4, 4);

			RUDPSocketError error = RUDPSocketError.Success;
			Socket.BeginSend(buffer, 0, buffer.Length, out error, new AsyncCallback(EndSend), null);
			Socket.BeginReceive(new AsyncCallback(EndReceive), null);

			SendIteration++;
		}

		public void EndSend(IAsyncResult result)
		{
			Socket.EndSend(result);

			if (SendIteration > 100)
			{
				Socket.Close();
				return;
			}

			byte[] buffer = new byte[4 + 4 + SendIteration];
			Array.Copy(BitConverter.GetBytes(MyNumber), buffer, 4);
			Array.Copy(BitConverter.GetBytes(SendIteration), 0, buffer, 4, 4);

			RUDPSocketError error = RUDPSocketError.Success;
			Socket.BeginSend(buffer, 0, buffer.Length, out error, new AsyncCallback(EndSend), null);

			SendIteration++;
		}

		public void EndReceive(IAsyncResult result)
		{
			byte[] buffer = Socket.EndReceive(result);

			if (buffer == null)
			{
				// Closed
				return;
			}
			int receiveMyNumber = BitConverter.ToInt32(buffer, 0);

			int receiveIteration = BitConverter.ToInt32(buffer, 4);

			if (receiveMyNumber == MyNumber)
				Debug.Fail("Bad addressed server");

			if (receiveIteration != ReceiveIteration)
				Debug.Fail("Bad Iteration, packet not right ordered");

			if ((buffer.Length - 4 - 4) != ReceiveIteration)
				Debug.Fail("Bad packet size");

			ReceiveIteration++;

			Socket.BeginReceive(new AsyncCallback(EndReceive), null);
		}
	}

	#endregion
}
