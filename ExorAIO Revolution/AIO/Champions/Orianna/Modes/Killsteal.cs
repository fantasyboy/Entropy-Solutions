
using System;
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Caching;

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
        public void Killsteal(EntropyEventArgs args)
        {
            if (GetBall() == null)
            {
                return;
            }

            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
				MenuClass.Q["killsteal"].Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range)
	                .Where(t => t.Distance(GetBall().Position) < t.DistanceToPlayer()))
                {
					SpellClass.Q.GetPredictionInput(target).From = GetBall().Position;

                    var collisions = SpellClass.Q.GetPrediction(target).CollisionObjects
                        .Where(c => Extensions.GetAllGenericMinionsTargetsInRange(SpellClass.Q.Range).Contains(c))
                        .ToList();
                    var multiplier = 1 - 0.10 * Math.Min(6, collisions.Count);
                    var damageToTarget = GetQDamage(target) * multiplier;

                    if (damageToTarget >= target.GetRealHealth(DamageType.Magical))
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
                MenuClass.W["killsteal"].Enabled)
            {
                if (ObjectCache.EnemyHeroes.Any(t =>
                        t.IsValidTarget(SpellClass.W.Width - SpellClass.W.Delay * t.BoundingRadius, GetBall().Position) &&
                        GetWDamage(t) >= t.GetRealHealth(DamageType.Magical)))
                {
                    SpellClass.W.Cast();
                }
            }

            /// <summary>
            ///     The KillSteal R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.R["killstealwhitelist"] != null &&
                MenuClass.R["killsteal"].Enabled)
            {
                foreach (var enemy in ObjectCache.EnemyHeroes.Where(t =>
                    MenuClass.R["killstealwhitelist"][t.CharName.ToLower()].Enabled &&
                    t.IsValidTarget(SpellClass.R.Width - t.BoundingRadius - SpellClass.R.Delay * t.BoundingRadius, GetBall().Position)))
                {
                    var dmg = GetRDamage(enemy);

                    if (SpellClass.Q.Ready &&
                        enemy.IsValidTarget(SpellClass.Q.Range))
                    {
                        dmg += GetQDamage(enemy);
                    }

                    if (SpellClass.W.Ready &&
                       enemy.IsValidTarget(SpellClass.W.Width, GetBall().Position))
                    {
                        dmg += GetWDamage(enemy);
                    }

                    if (UtilityClass.Player.Position.Distance(GetBall()) < UtilityClass.Player.GetAutoAttackRange())
                    {
                        dmg += UtilityClass.Player.GetAutoAttackDamage(enemy);
                    }

                    if (dmg >= enemy.GetRealHealth(DamageType.Magical))
                    {
                        SpellClass.R.Cast();
                    }
                }
            }
        }

        #endregion
    }
}