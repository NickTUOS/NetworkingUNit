using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

using Helper.Net.RUDP;

namespace Test.UnitTest.NonReliable
{
	public class NonReliableTest : UnitTest
	{
		public RUDPSocket ServerSocket;

		int serverPort = GetAvailablePort();

		ClientSocket cs;
		ServerSocket ss;

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
			{
				cs = new ClientSocket(serverPort);
				Thread t = new Thread(new ThreadStart(cs.Start));
				t.Name = "PerformanceTest - Client";
				t.Start();

				while (!cs.IsCompleted)
					Thread.Sleep(1000);
			}

			// Close the accepting socket
			ServerSocket.Close();

			//----
			Console.WriteLine("Client speed: (Kb/s)" + cs.Speed);
			Console.WriteLine("Server speed: (Kb/s)" + ss.Speed);
		}

		public void Accept(IAsyncResult result)
		{
			RUDPSocket acceptedSocket = ServerSocket.EndAccept(result);
			ss = new ServerSocket(acceptedSocket);
			acceptedSocket.BeginReceive(new AsyncCallback(ss.Receive), null);

			ServerSocket.BeginAccept(new AsyncCallback(Accept), ServerSocket);
		}
	}

	#region ServerSocket

	public class ServerSocket
	{
		static public int Number = 0;

		public RUDPSocket Socket;
		public int MyNumber;

		public long TotalBytes = 0;
		public int StartTicks;
		public int EndTicks;

		public ServerSocket(RUDPSocket socket)
		{
			Socket = socket;
			MyNumber = ++Number;
			StartTicks = Environment.TickCount;
		}

		public void Receive(IAsyncResult result)
		{
			byte[] buffer = Socket.EndReceive(result);

			if (buffer == null)
				return;

			EndTicks = Environment.TickCount;
			TotalBytes += buffer.Length;

			Socket.BeginReceive(new AsyncCallback(Receive), null);
		}

		public double Speed
		{
			get
			{
				double nowSeconds = (EndTicks - StartTicks) / 1000;
				double speed = (TotalBytes / 1024) / nowSeconds;

				return speed;
			}
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

		public long TotalBytes = 0;
		public int StartTicks;
		public int EndTicks;

		public int Duration = 30; // 60 seconds

		public bool IsCompleted = false;

		Random rand = new Random();

		public ClientSocket(int serverPort)
		{
			Socket = new RUDPSocket();
			MyNumber = ++Number;
			ServerPort = serverPort;
			StartTicks = Environment.TickCount;
		}

		public void Start()
		{
			Socket.Connect(UnitTest.CreateLocalEndPoint(ServerPort));

			byte[] buffer = new byte[BufferSendSize];

			while (true)
			{
				RUDPSocketError error = RUDPSocketError.Success;
				Socket.BeginSend(buffer, 0, buffer.Length, out error, new AsyncCallback(EndSend), null, false);

				if (error == RUDPSocketError.Success)
					return;

				if (error != RUDPSocketError.NoBufferSpaceAvailable)
				{
					Debug.Fail("RUDPSocketError=" + error);
					return;
				}

				Thread.Sleep(1);
			}
		}

		public void EndSend(IAsyncResult result)
		{
			int size = Socket.EndSend(result);

			EndTicks = Environment.TickCount;
			TotalBytes += size;

			if (((EndTicks - StartTicks) / 1000) > Duration)
			{
				Socket.Close();
				IsCompleted = true;
				return;
			}

			byte[] buffer = new byte[BufferSendSize];

			while (true)
			{
				RUDPSocketError error = RUDPSocketError.Success;
				Socket.BeginSend(buffer, 0, buffer.Length, out error, new AsyncCallback(EndSend), null, false);

				if (error == RUDPSocketError.Success)
					return;

				if (error != RUDPSocketError.NoBufferSpaceAvailable)
				{
					Debug.Fail("RUDPSocketError=" + error);
					return;
				}

				Thread.Sleep(1);
			}
		}

		public int BufferSendSize
		{
			get
			{
				//return rand.Next(32 * 1024);
				return 32 * 1024;
			}
		}

		public double Speed
		{
			get
			{
				double nowSeconds = (EndTicks - StartTicks) / 1000;
				double speed = (TotalBytes / 1024) / nowSeconds;

				return speed;
			}
		}
	}

	#endregion

}
