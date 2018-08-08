using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Caching;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;

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
		public void JungleClear()
		{
			/// <summary>
			///     The E Jungleclear Logic.
			/// </summary>
			if (SpellClass.E.Ready &&
			    UtilityClass.Player.Level() >=
					MenuClass.E["junglesteal"].Value &&
			    MenuClass.E["junglesteal"].Enabled)
			{
				foreach (var minion in Extensions.GetGenericJungleMinionsTargets()
					.Where(m => IsPerfectRendTarget(m) && m.HP <= GetEDamage(m)))
				{
					if (minion.IsLargeJungleMinion() &&
					    MenuClass.E["whitelist"][minion.CharName].Enabled)
					{
						SpellClass.E.Cast();
					}
					else if (!minion.IsLargeJungleMinion() &&
					         MenuClass.General["junglesmall"].Enabled)
					{
						SpellClass.E.Cast();
					}
				}
			}

			var jungleTarget = ObjectCache.EnemyMinions
				.Where(m => Extensions.GetGenericJungleMinionsTargets().Contains(m))
				.MinBy(m => m.DistanceToPlayer());
			if (jungleTarget == null ||
			    jungleTarget.HP < UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 3)
			{
				return;
			}

			/// <summary>
			///     The Q Jungleclear Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    jungleTarget.IsValidTargetEx(SpellClass.Q.Range) &&
			    UtilityClass.Player.MPPercent()
					> ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["jungleclear"]) &&
			    MenuClass.Q["jungleclear"].Enabled)
			{
				SpellClass.Q.Cast(jungleTarget);
			}
		}

		#endregion
	}
}