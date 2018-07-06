
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Jinx
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on orbwalker action.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
        public void Jungleclear(OnPreAttackEventArgs args)
        {
            var jungleTarget = args.Target as AIMinionClient;
            if (!jungleTarget.IsValidTarget() ||
                !Extensions.GetGenericJungleMinionsTargets().Contains(jungleTarget))
            {
                if (IsUsingFishBones())
                {
                    SpellClass.Q.Cast();
                }
                return;
            }

            /// <summary>
            ///     The Jungleclear Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                var manaPercent = UtilityClass.Player.MPPercent();
                var minionsInRange = Extensions.GetGenericJungleMinionsTargets().Count(m => m.Distance(jungleTarget) <= SplashRange);
                var jungleClearMinMinions = MenuClass.Spells["q"]["customization"]["jungleclear"].As<MenuSlider>().Value;
                var jungleClearManaManager = MenuClass.Spells["q"]["jungleclear"].As<MenuSliderBool>().Value;

                if (!IsUsingFishBones())
                {
                    if (manaPercent >= jungleClearManaManager &&
                        minionsInRange >= jungleClearMinMinions)
                    {
                        SpellClass.Q.Cast();
                    }
                }
                else
                {
                    if (manaPercent < jungleClearManaManager ||
                        minionsInRange < jungleClearMinMinions)
                    {
                        SpellClass.Q.Cast();
                    }
                }
            }

            /// <summary>
            ///     The Jungleclear W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                jungleTarget.IsValidTarget(SpellClass.W.Range) &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["jungleclear"]) &&
                MenuClass.Spells["w"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.W.Cast(jungleTarget);
            }
        }

        #endregion
    }
}