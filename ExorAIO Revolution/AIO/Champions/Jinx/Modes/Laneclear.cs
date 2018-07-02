
using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using AIO.Utilities;

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
        /// <param name="args">The <see cref="PreAttackEventArgs" /> instance containing the event data.</param>
        public void Laneclear(object sender, PreAttackEventArgs args)
        {
            var minionTarget = args.Target as Obj_AI_Minion;
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
                var manaPercent = UtilityClass.Player.ManaPercent();
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