
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
        public void Combo(EntropyEventArgs args)
        {
            /// <summary>
            ///     Orbwalk on minions.
            /// </summary>
            var minion = ObjectManager.Get<AIMinionClient>()
                .Where(m => m.IsValidSpellTarget(UtilityClass.Player.GetAutoAttackRange(m)))
                .OrderBy(s => s.GetRealBuffCount("kalistaexpungemarker"))
                .MinBy(o => o.HP);
            if (minion != null &&
                !GameObjects.EnemyHeroes.Any(t => t.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(t)+100f)) &&
                MenuClass.Miscellaneous["minionsorbwalk"].As<MenuBool>().Enabled)
            {
                UtilityClass.Player.IssueOrder(OrderType.AttackUnit, minion);
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (bestTarget != null)
                {
                    var collisions = SpellClass.Q.GetPrediction(bestTarget).CollisionObjects
                        .Where(c => Extensions.GetAllGenericMinionsTargetsInRange(SpellClass.Q.Range).Contains(c))
                        .ToList();
                    if (collisions.Any())
                    {
                        if (collisions.All(c => c.GetRealHealth() <= UtilityClass.Player.GetSpellDamage(c, SpellSlot.Q)))
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
        public void RendCombo(args)
        {
            /// <summary>
            ///     The E Combo Minion Harass Logic.
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