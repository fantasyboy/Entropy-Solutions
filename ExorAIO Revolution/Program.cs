
using Entropy.SDK.Events;
using Entropy.SDK.Util;

#pragma warning disable 1587
namespace AIO
{
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
        ///     Event which triggers on game start.
        /// </summary>
        private static void OnStart()
        {
            DelayAction.Queue(3000, () =>
            {
                General.Menu();
                General.Methods();

                Bootstrap.LoadChampion();
            });
        }

        #endregion
    }
}