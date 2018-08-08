using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Caching;
using Entropy.SDK.Extensions;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;

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
		public void Combo()
		{
			/// <summary>
			///     Orbwalk on minions.
			/// </summary>
			var minion = ObjectCache.EnemyLaneMinions
				.Where(m => m.IsValidSpellTarget(UtilityClass.Player.GetAutoAttackRange(m)))
				.OrderBy(s => s.GetBuffCount("kalistaexpungemarker"))
				.MinBy(o => o.HP);
			if (minion != null &&
			    !ObjectCache.EnemyHeroes.Any(t => t.IsValidTargetEx(UtilityClass.Player.GetAutoAttackRange(t)+25)) &&
			    MenuClass.Miscellaneous["minionsorbwalk"].Enabled)
			{
				Orbwalker.Attack(minion);
			}

			/// <summary>
			///     The Q Combo Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    MenuClass.Q["combo"].Enabled &&
				UtilityClass.Player.AutoAttacksPerSecond() < 1.5)
			{
				var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
				if (bestTarget != null)
				{
					var collisions = SpellClass.Q.GetPrediction(bestTarget).CollisionObjects
						.Where(c => Extensions.GetAllGenericMinionsTargetsInRange(SpellClass.Q.Range).Contains(c))
						.ToList();
					if (collisions.Any())
					{
						if (collisions.All(c => c.GetRealHealth(DamageType.Physical) <= GetQDamage(c)))
						{
							SpellClass.Q.Cast(bestTarget);
						}
					}
					else
					{
						SpellClass.Q.Cast(bestTarget);
					}
				}
			}

			/// <summary>
			///     The E Combo Minion Harass Logic.
			/// </summary>
			if (SpellClass.E.Ready &&
			    Extensions.GetEnemyLaneMinionsTargets()
					.Any(m => IsPerfectRendTarget(m) && m.HP <= GetEDamage(m)) &&
			    MenuClass.E["harass"].Enabled)
			{
				if (ObjectCache.EnemyHeroes.Where(IsPerfectRendTarget)
					.Any(enemy => !enemy.HasBuffOfType(BuffType.Slow) || !MenuClass.E["dontharassslowed"].Enabled))
				{
					SpellClass.E.Cast();
				}
			}
		}

		#endregion
	}
}