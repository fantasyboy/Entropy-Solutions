
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
        public void Laneclear(OnPreAttackEventArgs args)
        {
            var minionTarget = args.Target as AIMinionClient;
            if (minionTarget == null)
            {
                if (IsUsingFishBones())
                {
                    SpellClass.Q.Cast();
                }
                return;
            }

            /// <summary>
            ///     The Laneclear Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                var manaPercent = UtilityClass.Player.MPPercent();
                var minionsInRange = Extensions.GetEnemyLaneMinionsTargets().Count(m => m.Distance(minionTarget) < SplashRange);
                var laneClearMinMinions = MenuClass.Spells["q"]["customization"]["laneclear"].Value;
                var laneClearManaManager = MenuClass.Spells["q"]["laneclear"].As<MenuSliderBool>().Value;

                if (!IsUsingFishBones())
                {
                    if (manaPercent >= laneClearManaManager &&
                        minionsInRange >= laneClearMinMinions)
                    {
                        SpellClass.Q.Cast();
                    }
                }
                else
                {
                    if (manaPercent < laneClearManaManager ||
                        minionsInRange < laneClearMinMinions)
                    {
                        SpellClass.Q.Cast();
                    }
                }
            }
        }

        #endregion
    }
}