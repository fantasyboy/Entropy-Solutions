using System.Linq;
using AIO.Utilities;
using Entropy;
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
				if (Extensions.GetGenericJungleMinionsTargets()
				              .Any(m => IsPerfectRendTarget(m) && m.HP <= GetEDamage(m) && MenuClass.E["whitelist"][m.ModelName.ToLower()].Enabled))
				{
					SpellClass.E.Cast();
				}
			}

			var jungleTarget = Extensions.GetGenericJungleMinionsTargets()
			                             .MinBy(m => Hud.CursorPositionUnclipped.DistanceToPlayer());
			if (jungleTarget == null ||
			    jungleTarget.HP < UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 3)
			{
				return;
			}

			/// <summary>
			///     The Q Jungleclear Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    jungleTarget.IsValidTarget(SpellClass.Q.Range) &&
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