
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;
using SharpDX;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Lucian
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void LaneClear(EntropyEventArgs args)
        {
            /// <summary>
            ///     Extended.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q2"]["laneclear"]) &&
                MenuClass.Spells["q2"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q2.Range).Where(t =>
                    !t.IsValidTarget(SpellClass.Q.Range) &&
                    MenuClass.Spells["q2"]["whitelist"][t.CharName.ToLower()].Enabled))
                {
                    foreach (var minion in Extensions.GetAllGenericUnitTargetsInRange(SpellClass.Q.Range))
                    {
                        if (minion.NetworkID != target.NetworkID &&
                            QRectangle(minion).IsInside((Vector2)target.Position))
                        {
                            SpellClass.Q.CastOnUnit(minion);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
        public void Laneclear(OnPostAttackEventArgs args)
        {
            /// <summary>
            ///     The E Laneclear Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["laneclear"]) &&
                MenuClass.Spells["e"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.E.Cast(UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, UtilityClass.Player.BoundingRadius));
            }

            /// <summary>
            ///     The Laneclear Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["laneclear"]) &&
                MenuClass.Spells["q"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                /*
                var farmLocation = SpellClass.Q2.GetLineFarmLocation(Extensions.GetEnemyLaneMinionsTargets(), SpellClass.Q2.Width);
                if (farmLocation.MinionsHit >= MenuClass.Spells["q"]["customization"]["laneclear"].As<MenuSlider>().Value)
                {
                    SpellClass.Q.CastOnUnit(farmLocation.FirstMinion);
                    return;
                }
                */
            }

            /// <summary>
            ///     The Laneclear W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["laneclear"]) &&
                MenuClass.Spells["w"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                /*
                var farmLocation = SpellClass.W.GetCircularFarmLocation(Extensions.GetEnemyLaneMinionsTargets(), SpellClass.W.Width);
                if (farmLocation.MinionsHit >= MenuClass.Spells["w"]["customization"]["laneclear"].As<MenuSlider>().Value)
                {
                    SpellClass.W.Cast(farmLocation.Position);
                }
                */
            }
        }

        #endregion
    }
}