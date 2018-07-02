
using System.Linq;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Xayah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Laneclear()
        {
            /// <summary>
            ///     The Laneclear Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["laneclear"]) &&
                MenuClass.Spells["q"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                /*
                var farmLocation = SpellClass.Q.GetLineFarmLocation(Extensions.GetEnemyLaneMinionsTargets(), SpellClass.Q.Width);
                if (farmLocation.MinionsHit >= MenuClass.Spells["q"]["customization"]["laneclear"].As<MenuSlider>().Value)
                {
                    SpellClass.Q.Cast(farmLocation.Position);
                }
                */
            }

            /// <summary>
            ///     The Laneclear W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["laneclear"]) &&
                MenuClass.Spells["w"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                if (Extensions.GetEnemyLaneMinionsTargetsInRange(UtilityClass.Player.AttackRange).Any())
                {
                    SpellClass.W.Cast();
                }
            }
        }

        /// <summary>
        ///     Fired as fast as possible.
        /// </summary>
        public void BladeCallerLaneclear()
        {
            /// <summary>
            ///     The Laneclear E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["laneclear"]) &&
                MenuClass.Spells["e"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                if (CountFeathersKillableMinions() >= MenuClass.Spells["e"]["customization"]["laneclear"].As<MenuSlider>().Value)
                {
                    SpellClass.E.Cast();
                }
            }
        }

        #endregion
    }
}