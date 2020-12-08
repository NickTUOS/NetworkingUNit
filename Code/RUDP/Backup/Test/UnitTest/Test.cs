using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Diagnostics;

namespace Test.UnitTest
{
	abstract public class UnitTest
	{

		#region Variables

		static public IPAddress LocalAddress = Dns.GetHostAddresses(Dns.GetHostName())[0];
		static public int LastAvailablePort = 10000;
		protected string Name = "Test";

		internal bool executeClient = true;
		internal bool executeServer = true;

		#endregion

		#region Helpers

		static public IPEndPoint CreateLocalEndPoint(int port)
		{
			return new IPEndPoint(LocalAddress, port);
		}

		static public int GetAvailablePort()
		{
			return LastAvailablePort++;
		}

		#endregion

		#region ExecuteTest

		public void ExecuteTest()
		{
			try
			{
				Name = this.GetType().Name;
				Console.WriteLine("---------- Start [" + Name + "]");

				Execute(executeClient, executeServer);

				Console.WriteLine("---------- End [" + Name + "]");
			}
			catch (Exception e)
			{
				Debug.Fail(e.Message);
			}
		}

		#endregion

		abstract public void Execute(bool executeClient, bool executeServer);

	}
}
