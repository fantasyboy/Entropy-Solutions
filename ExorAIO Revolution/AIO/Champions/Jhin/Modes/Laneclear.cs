using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Jhin
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void LaneClear(EntropyEventArgs args)
        {


            /// <summary>
            ///     The Laneclear Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["laneclear"]) &&
                MenuClass.Q["laneclear"].As<MenuSliderBool>().Enabled)
            {
                /*
                var farmLocation = SpellClass.Q.GetLineFarmLocation(Extensions.GetEnemyLaneMinionsTargets(), SpellClass.Q.Width);
                if (farmLocation.MinionsHit >= MenuClass.Q["customization"]["laneclear"].As<MenuSlider>().Value)
                {
                    SpellClass.Q.Cast(farmLocation.Position);
                }
                */
            }

            /// <summary>
            ///     The Laneclear W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.W["laneclear"]) &&
                MenuClass.W["laneclear"].As<MenuSliderBool>().Enabled)
            {
                /*
                var farmLocation = SpellClass.W.GetLineFarmLocation(Extensions.GetEnemyLaneMinionsTargets(), SpellClass.W.Width);
                if (farmLocation.MinionsHit >= MenuClass.W["customization"]["laneclear"].As<MenuSlider>().Value)
                {
                    SpellClass.W.Cast(farmLocation.Position);
                }
                */
            }

            /// <summary>
            ///     The Laneclear E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.E["laneclear"]) &&
                MenuClass.E["laneclear"].As<MenuSliderBool>().Enabled)
            {
                //SpellClass.E.CastIfWillHit(MenuClass.E["customization"]["laneclear"].As<MenuSlider>().Value);
            }
        }

        #endregion
    }
}