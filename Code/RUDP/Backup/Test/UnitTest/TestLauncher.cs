using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Test.UnitTest
{
	public class TestLauncher
	{
		static List<UnitTest> Tests = new List<UnitTest>();

		static public void Main(string[] args)
		{
			//---- Add the tests
			Tests.Add(new Test.UnitTest.Connect.ConnectTest());
			Tests.Add(new Test.UnitTest.RendezVous.RendezVousTest());
			Tests.Add(new Test.UnitTest.Performance.PerformanceTest());
			Tests.Add(new Test.UnitTest.NonReliable.NonReliableTest());
			Tests.Add(new Test.UnitTest.MultipleConnection.MultipleConnectionTest());

			//---- Start
			ExecuteAllTests();
		}

		static public void ExecuteAllTests()
		{
			foreach (UnitTest test in Tests)
			{
				test.ExecuteTest();
			}
		}
	}
}
