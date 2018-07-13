using System;
using Entropy;
using Entropy.SDK.Events;
using Entropy.ToolKit;
using NabbTracker.Trackers;
using NabbTracker.Utilities;

namespace NabbTracker
{
    /// <summary>
    ///     The application class.
    /// </summary>
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
        ///     Called on present.
        /// </summary>
        private static void OnRender(EntropyEventArgs args)
        {
            SpellTracker.Initialize();
            ExpTracker.Initialize();
            TowerRangeTracker.Initialize();
            AttackRangeTracker.Initialize();
        }

        /// <summary>
        ///     Called upon game start.
        /// </summary>
        private static void Loading_OnLoadingComplete()
        {
            Menus.Initialize();
            Logging.Log("NabbTracker: Revolution - Loaded!");

	        Renderer.OnRender += OnRender;
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