using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

using Helper.Net.RUDP;

namespace Test.UnitTest.Connect
{
	public class ConnectTest : UnitTest
	{
		public RUDPSocket ServerSocket = new RUDPSocket();
		public RUDPSocket ClientSocket = new RUDPSocket();

		int serverPort = GetAvailablePort();
		int clientPort = GetAvailablePort();

		override public void Execute(bool executeClient, bool executeServer)
		{
			if (executeServer)
				ServerSocket.Bind(CreateLocalEndPoint(serverPort));

			//---- Execute client
			if (executeClient)
			{
				Thread t = new Thread(new ThreadStart(ExecuteClient));
				t.Start();
			}

			//---- Execute server
			if (executeServer)
				ExecuteServer();
		}

		public void ExecuteClient()
		{
			ClientSocket.Bind(CreateLocalEndPoint(clientPort));
			ClientSocket.Connect(CreateLocalEndPoint(serverPort));

			Debug.Assert(ClientSocket.Connected, "Connection failed");

			//---- Send message 1
			byte[] buffer = ASCIIEncoding.UTF8.GetBytes("Hello World");
			ClientSocket.Send(buffer, 0, buffer.Length);

			//---- Receive message 2
			buffer = ClientSocket.Receive();
			System.Console.WriteLine(ASCIIEncoding.UTF8.GetString(buffer));

			//---- Send message 3
			buffer = ASCIIEncoding.UTF8.GetBytes("Are you fine ?");
			ClientSocket.Send(buffer, 0, buffer.Length);

			//---- Receive message 4
			buffer = ClientSocket.Receive();
			System.Console.WriteLine(ASCIIEncoding.UTF8.GetString(buffer));

			//---- Wait for close
			buffer = ClientSocket.Receive();
			System.Console.WriteLine("Connection closed");
		}

		public void ExecuteServer()
		{
			RUDPSocket acceptedSocket = ServerSocket.Accept();

			ServerSocket.Close();

			//---- Receive message 1
			byte[] buffer = acceptedSocket.Receive();
			System.Console.WriteLine(ASCIIEncoding.UTF8.GetString(buffer));

			//---- Send message 2
			buffer = ASCIIEncoding.UTF8.GetBytes("Hello You ;-)");
			acceptedSocket.Send(buffer, 0, buffer.Length);

			//---- Receive message 3
			buffer = acceptedSocket.Receive();
			System.Console.WriteLine(ASCIIEncoding.UTF8.GetString(buffer));

			//---- Send message 4
			buffer = ASCIIEncoding.UTF8.GetBytes("Really fine");
			acceptedSocket.Send(buffer, 0, buffer.Length);

			//----
			acceptedSocket.Shutdown();
			acceptedSocket.Close();
		}
	}
}
