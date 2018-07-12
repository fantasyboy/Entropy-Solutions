
using Entropy;
using AIO.Utilities;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Twitch
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Automatic(EntropyEventArgs args)
        {
        }

        /// <summary>
        ///     Fired as fast as possible.
        /// </summary>
        public void ExpungeAutomatic(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Automatic E Logics.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["ondeath"].As<MenuBool>().Enabled &&
                ImplementationClass.IHealthPrediction.GetPrediction(UtilityClass.Player, 1000 + Game.Ping) <= 0)
            {
                SpellClass.E.Cast();
            }
        }

        #endregion
    }
}