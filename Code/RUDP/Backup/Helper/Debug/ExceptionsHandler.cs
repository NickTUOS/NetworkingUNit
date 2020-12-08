using System;
using System.Diagnostics;

namespace Helper.Debug
{

	/// <summary>
	/// This class is a central handler for exceptions.
	/// </summary>
	public sealed class ExceptionsHandler
	{

		#region Constructor

		private ExceptionsHandler()
		{
		}

		#endregion

		#region Handle

		[Conditional("DEBUG")]
		public static void Handle(Exception exception, params object[] args)
		{
			string paramsText = "";

			foreach (object val in args)
				paramsText += " - " + val.ToString();

			if (paramsText.Length > 0)
				Console.WriteLine(exception.Message + '(' + paramsText + ")\n" + exception.StackTrace);

			else
				Console.WriteLine(exception.Message + '\n' + exception.StackTrace);
		}

		#endregion

	}
}