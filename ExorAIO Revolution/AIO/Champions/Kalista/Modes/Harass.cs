
using System.Collections.Generic;
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Kalista
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Harass(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Q Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["harass"]) &&
                MenuClass.Spells["q"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (bestTarget != null &&
                    MenuClass.Spells["q"]["whitelist"][bestTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                {
                    var collisions = SpellClass.Q.GetPrediction(bestTarget).CollisionObjects
                        .Where(c => Extensions.GetAllGenericUnitTargets().Contains(c));
                    var objAiBases = collisions as IList<AIBaseClient> ?? collisions.ToList();
                    if (objAiBases.Any())
                    {
                        if (objAiBases.All(c => c.GetRealHealth() <= UtilityClass.Player.GetSpellDamage(c, SpellSlot.Q)))
                        {
                            SpellClass.Q.Cast(SpellClass.Q.GetPrediction(bestTarget).CastPosition);
                        }
                    }
                    else
                    {
                        SpellClass.Q.Cast(SpellClass.Q.GetPrediction(bestTarget).CastPosition);
                    }
                }
            }
        }

        /// <summary>
        ///     Fired as fast as possible.
        /// </summary>
        public void RendHarass(args)
        {
            /// <summary>
            ///     The E Minion Harass Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                Extensions.GetEnemyLaneMinionsTargets().Any(m =>
                    IsPerfectRendTarget(m) &&
                    m.GetRealHealth() <= GetTotalRendDamage(m)) &&
                MenuClass.Spells["e"]["harass"].As<MenuBool>().Enabled)
            {
                if (GameObjects.EnemyHeroes.Where(IsPerfectRendTarget).Any(enemy => !enemy.HasBuffOfType(BuffType.Slow) || !MenuClass.Spells["e"]["dontharassslowed"].As<MenuBool>().Enabled))
                {
                    SpellClass.E.Cast();
                }
            }
        }

        #endregion
    }
}