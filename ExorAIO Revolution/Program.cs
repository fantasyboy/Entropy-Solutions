using Entropy.SDK.Events;
using Entropy.SDK.Utils;
using Entropy.ToolKit;

namespace AIO
{
	using System;

	internal class Program
	{
		#region Methods

		/// <summary>
		///     The entry point of the application.
		/// </summary>
		private static void Main()
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			Loading.OnLoadingComplete += Loading_OnLoadingComplete;
		}

		/// <summary>
		///     Event which triggers on game start.
		/// </summary>
		private static void Loading_OnLoadingComplete()
		{
			DelayAction.Queue(() =>
			{
				General.Menu();
				General.Methods();

				Bootstrap.LoadChampion();
			}, 1750); // Let it load the Champ Stats. (~2sec)
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.ExceptionObject is Exception exception)
			{
				exception.ToolKitLog("Unexpected error occurred in AIO");
			}
		}

		#endregion
	}
}