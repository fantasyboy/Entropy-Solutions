
using System;
using System.Linq;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Orianna
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Killsteal()
        {
            if (BallPosition == null)
            {
                return;
            }

            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range))
                {
                    SpellClass.Q.GetPredictionInput(target).From = (Vector3)BallPosition;

                    var collisions = SpellClass.Q.GetPrediction(target).CollisionObjects
                        .Where(c => Extensions.GetAllGenericMinionsTargetsInRange(SpellClass.Q.Range).Contains(c))
                        .ToList();
                    var multiplier = 1 - 0.10 * Math.Min(6, collisions.Count);
                    var damageToTarget = UtilityClass.Player.GetSpellDamage(target, SpellSlot.Q) * multiplier;

                    if (damageToTarget >= target.GetRealHealth())
                    {
                        SpellClass.Q.Cast(target);
                        break;
                    }
                }
            }

            /// <summary>
            ///     The KillSteal W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["killsteal"].As<MenuBool>().Enabled)
            {
                if (GameObjects.EnemyHeroes.Any(t =>
                        t.IsValidTarget(SpellClass.W.Width, checkRangeFrom: (Vector3)BallPosition) &&
                        UtilityClass.Player.GetSpellDamage(t, SpellSlot.W) >= t.GetRealHealth()))
                {
                    SpellClass.W.Cast();
                }
            }

            /// <summary>
            ///     The KillSteal R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["killstealwhitelist"] != null &&
                MenuClass.Spells["r"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var enemy in GameObjects.EnemyHeroes.Where(t =>
                    MenuClass.Spells["r"]["killstealwhitelist"][t.ChampionName.ToLower()].As<MenuBool>().Enabled &&
                    t.IsValidTarget(SpellClass.R.Width - t.BoundingRadius - SpellClass.R.Delay * t.BoundingRadius, checkRangeFrom: (Vector3)BallPosition)))
                {
                    var dmg = UtilityClass.Player.GetSpellDamage(enemy, SpellSlot.R);

                    if (SpellClass.Q.Ready &&
                        enemy.IsValidTarget(SpellClass.Q.Range))
                    {
                        dmg += UtilityClass.Player.GetSpellDamage(enemy, SpellSlot.Q);
                    }

                    if (SpellClass.W.Ready &&
                       enemy.IsValidTarget(SpellClass.W.Width, checkRangeFrom: (Vector3)BallPosition))
                    {
                        dmg += UtilityClass.Player.GetSpellDamage(enemy, SpellSlot.W);
                    }

                    if (UtilityClass.Player.ServerPosition.Distance((Vector3)BallPosition) < UtilityClass.Player.AttackRange)
                    {
                        dmg += UtilityClass.Player.GetAutoAttackDamage(enemy);
                    }

                    if (dmg >= enemy.GetRealHealth())
                    {
                        SpellClass.R.Cast();
                    }
                }
            }
        }

        #endregion
    }
}