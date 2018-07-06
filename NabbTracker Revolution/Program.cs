using System;
using Aimtec;
using Aimtec.SDK.Events;

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
            GameEvents.GameStart += OnStart;
        }

        /// <summary>
        ///     Called on present.
        /// </summary>
        private static void OnPresent()
        {
            SpellTracker.Initialize();
            ExpTracker.Initialize();
            TowerRangeTracker.Initialize();
            AttackRangeTracker.Initialize();
        }

        /// <summary>
        ///     Called upon game start.
        /// </summary>
        private static void OnStart()
        {
            Menus.Initialize();
            Console.WriteLine("NabbTracker: Revolution - Loaded!");

            Render.OnPresent += OnPresent;
        }

        #endregion
    }
}