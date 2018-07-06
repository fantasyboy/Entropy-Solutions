
using System.Linq;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Olaf
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(args)
        {
            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The Automatic R Cleanse Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["logical"].As<MenuSliderBool>().Enabled)
            {
                if (UtilityClass.Player.GetActiveBuffs().Any(b =>
                        b.IsHardCC() &&
                        b.GetRemainingBuffTime() >= MenuClass.Spells["r"]["logical"].As<MenuSliderBool>().Value/1000f))
                {
                    SpellClass.R.Cast();
                }
            }
        }

        #endregion
    }
}