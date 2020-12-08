using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

using Helper.Net.RUDP;

namespace Test.UnitTest.MultipleConnection
{

	#region MultipleConnectionTest

	public class MultipleConnectionTest : UnitTest
	{
		public RUDPSocket ServerSocket;

		int serverPort = GetAvailablePort();
		static public int numberOfClients = 100;
		static public int numberOfIterationsPerClients = 100;

		private List<ClientSocket> Clients = new List<ClientSocket>();

		override public void Execute(bool executeClient, bool executeServer)
		{
			ServerSocket = new RUDPSocket();

			//---- Execute server
			if (executeServer)
			{
				ServerSocket.Bind(CreateLocalEndPoint(serverPort));
				ServerSocket.BeginAccept(new AsyncCallback(Accept), ServerSocket);
			}

			//---- Execute client
			if (executeClient)
				ExecuteClient();

			//---- Wait for completion
			while (true)
			{
				bool isComplete = true;
				foreach (ClientSocket cs in Clients)
					if (!cs.IsComplete)
					{
						isComplete = false;
						break;
					}

				Thread.Sleep(100);

				if (isComplete)
				{
					//---- Close the accepting socket
					ServerSocket.Close();
					return;
				}
			}

		}

		public void Accept(IAsyncResult result)
		{
			RUDPSocket acceptedSocket = ServerSocket.EndAccept(result);
			ServerSocket ss = new ServerSocket(acceptedSocket);
			acceptedSocket.BeginReceive(new AsyncCallback(ss.Receive), null);

			ServerSocket.BeginAccept(new AsyncCallback(Accept), ServerSocket);
		}

		public void ExecuteClient()
		{
			for (int index = 0; index < numberOfClients; index++)
			{
				ClientSocket cs = new ClientSocket(serverPort);
				Clients.Add(cs);
				Thread t = new Thread(new ThreadStart(cs.Start));
				t.Start();
				Thread.Sleep(1000);
			}
		}
	}

	#endregion

	#region ServerSocket

	public class ServerSocket
	{
		static public int Number = 0;

		public RUDPSocket Socket;
		public int MyNumber;
		public int Iteration = 0;

		public ServerSocket(RUDPSocket socket)
		{
			Socket = socket;
			MyNumber = ++Number;
		}

		public void Receive(IAsyncResult result)
		{
			byte[] buffer = Socket.EndReceive(result);

			if (buffer == null)
			{
				return;
				// Closed
			}

			int receiveMyNumber = BitConverter.ToInt32(buffer, 0);

			int receiveIteration = BitConverter.ToInt32(buffer, 4);

			if (receiveMyNumber != MyNumber)
				Debug.Fail("Bad addressed server");

			if (receiveIteration != Iteration)
				Debug.Fail("Bad Iteration, packet not right ordered");

			if ((buffer.Length - 4 - 4) != Iteration)
				Debug.Fail("Bad packet size");

			Iteration++;

			Socket.BeginReceive(new AsyncCallback(Receive), null);
		}
	}

	#endregion

	#region ClientSocket

	public class ClientSocket
	{
		static public int Number = 0;

		public RUDPSocket Socket;
		public int MyNumber;
		public int ServerPort;
		public int Iteration = 0;

		public bool IsComplete = false;

		public ClientSocket(int serverPort)
		{
			Socket = new RUDPSocket();
			MyNumber = ++Number;
			ServerPort = serverPort;
		}

		public void Start()
		{
			Socket.Connect(UnitTest.CreateLocalEndPoint(ServerPort));

			byte[] buffer = new byte[4 + 4 + Iteration];
			Array.Copy(BitConverter.GetBytes(MyNumber), buffer, 4);
			Array.Copy(BitConverter.GetBytes(Iteration), 0, buffer, 4, 4);

			RUDPSocketError error = RUDPSocketError.Success;
			do
			{
				error = RUDPSocketError.Success;
				Socket.BeginSend(buffer, 0, buffer.Length, out error, new AsyncCallback(EndSend), null);
				if (error != RUDPSocketError.Success)
					Thread.Sleep(1);
			} while (error != RUDPSocketError.Success);
		}

		public void EndSend(IAsyncResult result)
		{
			Socket.EndSend(result);

			Iteration++;

			if (Iteration > MultipleConnectionTest.numberOfIterationsPerClients)
			{
				Socket.Close();
				IsComplete = true;
				return;
			}

			byte[] buffer = new byte[4 + 4 + Iteration];
			Array.Copy(BitConverter.GetBytes(MyNumber), buffer, 4);
			Array.Copy(BitConverter.GetBytes(Iteration), 0, buffer, 4, 4);

			RUDPSocketError error = RUDPSocketError.Success;
			do
			{
				error = RUDPSocketError.Success;
				Socket.BeginSend(buffer, 0, buffer.Length, out error, new AsyncCallback(EndSend), null);
				if (error != RUDPSocketError.Success)
					Thread.Sleep(1);
			} while (error != RUDPSocketError.Success);
		}
	}

	#endregion

}
