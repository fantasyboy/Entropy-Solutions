using System.Linq;
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
		public void LaneClear()
		{
			/// <summary>
			///     Orbwalk on minions.
			/// </summary>
			var minion = ObjectCache.EnemyLaneMinions
				.Where(m => m.IsValidSpellTarget(UtilityClass.Player.GetAutoAttackRange(m)))
				.OrderBy(s => s.GetBuffCount("kalistaexpungemarker"))
				.MinBy(o => o.HP);
			if (minion != null)
			{
				Orbwalker.Attack(minion);
			}

			/// <summary>
			///     The Q Laneclear Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["laneclear"]) &&
			    MenuClass.Q["laneclear"].Enabled)
			{
				/*
                var farmLocation = Extensions.GetAllGenericMinionsTargets().Where(m => m.GetRealHealth(DamageType.Physical) < (float)UtilityClass.Player.GetSpellDamage(m, SpellSlot.Q)).ToList();
                if (SpellClass.Q.GetLineFarmLocation(farmLocation, SpellClass.Q.Width).MinionsHit >= 3)
                {
                    SpellClass.Q.Cast(SpellClass.Q.GetLineFarmLocation(farmLocation, SpellClass.Q.Width).Position);
                }
                */
			}

			/// <summary>
			///     The E Laneclear Logics.
			/// </summary>
			if (SpellClass.E.Ready &&
			    MenuClass.E["laneclear"].Enabled)
			{
				if (Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.E.Range)
						.Count(m => IsPerfectRendTarget(m) && m.HP <= GetEDamage(m)) >= MenuClass.E["laneclear"].Value)
				{
					SpellClass.E.Cast();
				}

				else if (Extensions.GetEnemyPetsInRange(SpellClass.E.Range)
					.Any(m => IsPerfectRendTarget(m) && m.HP <= GetEDamage(m)))
				{
					SpellClass.E.Cast();
				}
			}
		}

		#endregion
	}
}