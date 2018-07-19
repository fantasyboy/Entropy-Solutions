using System;
using System.Reflection;
using AIO.Utilities;
using Entropy.ToolKit;

#pragma warning disable 1587
namespace AIO
{
	/// <summary>
	///     The bootstrap class.
	/// </summary>
	internal static class Bootstrap
	{
		#region Public Methods and Operators

		/// <summary>
		///     Tries to load the champion which is being currently played.
		/// </summary>
		public static void LoadChampion()
		{
			try
			{
				var pluginName = "AIO.Champions." + UtilityClass.Player.CharName;
				var type = Type.GetType(pluginName, true);

				Activator.CreateInstance(type);
				Logging.Log($"ExorAIO: Revolution - {UtilityClass.Player.CharName} Loaded.");
			}
			catch (Exception e)
			{
				switch (e)
				{
					case TargetInvocationException _:
						Logging.Log(
							$"ExorAIO: Revolution - Error occurred while trying to load {UtilityClass.Player.CharName}.");
						e.ToolKitLog();
						break;
					case TypeLoadException _:
						Logging.Log($"ExorAIO: Revolution - {UtilityClass.Player.CharName} is NOT supported yet.");
						break;
				}
			}
		}

		#endregion
	}
}