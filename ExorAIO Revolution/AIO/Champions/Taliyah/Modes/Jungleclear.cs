// ReSharper disable ConvertIfStatementToConditionalTernaryExpression

using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Caching;
using Entropy.SDK.Damage;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using SharpDX;

#pragma warning disable 1587

namespace AIO.Champions
{
	/// <summary>
	///     The logics class.
	/// </summary>
	internal partial class Taliyah
	{
		#region Public Methods and Operators

		/// <summary>
		///     Fired when the game is updated.
		/// </summary>
		public void JungleClear(EntropyEventArgs args)
		{
			var jungleTarget = ObjectCache.AllMinions
				.Where(m => Extensions.GetGenericJungleMinionsTargets().Contains(m))
				.MinBy(m => m.DistanceToPlayer());
			if (jungleTarget == null ||
			    jungleTarget.GetRealHealth() < UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 3)
			{
				return;
			}

			/// <summary>
			///     The Jungleclear Rylai Q Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    jungleTarget.IsValidTargetEx(SpellClass.Q.Range) &&
			    UtilityClass.Player.HasItem(ItemID.RylaisCrystalScepter) &&
			    (IsNearWorkedGround() ||
			     UtilityClass.Player.MPPercent()
			     > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["jungleclear"])) &&
			    MenuClass.Q["jungleclear"].Enabled)
			{
				switch (MenuClass.Q["modes"]["jungleclear"].Value)
				{
					case 0:
						if (!IsNearWorkedGround())
						{
							SpellClass.Q.Cast(jungleTarget);
						}

						break;
					case 1:
						SpellClass.Q.Cast(jungleTarget);
						break;
				}
			}

			var targetPosAfterW = new Vector3();

			/// <summary>
			///     The Jungleclear W Logic.
			/// </summary>
			if (SpellClass.W.Ready &&
			    jungleTarget.IsValidTargetEx(SpellClass.W.Range) &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.W["jungleclear"]) &&
			    MenuClass.W["jungleclear"].Enabled)
			{
				var bestBoulderHitPos = GetBestBouldersHitPosition(jungleTarget);
				var bestBoulderHitPosHitBoulders = GetBestBouldersHitPositionHitBoulders(jungleTarget);
				var jungleTargetPredPos = SpellClass.W.GetPrediction(jungleTarget).CastPosition;

				if (SpellClass.E.Ready)
				{
					if (UtilityClass.Player.Distance(GetUnitPositionAfterPull(jungleTarget)) >= 200f)
					{
						targetPosAfterW = GetUnitPositionAfterPull(jungleTarget);
					}
					else
					{
						targetPosAfterW = GetUnitPositionAfterPush(jungleTarget);
					}

					//SpellClass.W.Cast(jungleTargetPredPos, targetPosAfterW);
					SpellClass.W.Cast(targetPosAfterW, jungleTargetPredPos);
				}
				else if (bestBoulderHitPos != Vector3.Zero && bestBoulderHitPosHitBoulders > 0)
				{
					//SpellClass.W.Cast(jungleTargetPredPos, bestBoulderHitPos);
					SpellClass.W.Cast(bestBoulderHitPos, jungleTargetPredPos);
				}
			}

			/// <summary>
			///     The Jungleclear E Logic.
			/// </summary>
			if (SpellClass.E.Ready &&
			    jungleTarget.IsValidTargetEx(SpellClass.E.Range) &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.E["jungleclear"]) &&
			    MenuClass.E["jungleclear"].Enabled)
			{
				SpellClass.E.Cast(SpellClass.W.Ready
					? targetPosAfterW
					: jungleTarget.Position);
			}

			/// <summary>
			///     The Jungleclear Q Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    jungleTarget.IsValidTargetEx(SpellClass.Q.Range) &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["jungleclear"]) &&
			    MenuClass.Q["jungleclear"].Enabled)
			{
				switch (MenuClass.Q["modes"]["jungleclear"].Value)
				{
					case 0:
						if (!IsNearWorkedGround())
						{
							SpellClass.Q.Cast(jungleTarget);
						}

						break;
					case 1:
						SpellClass.Q.Cast(jungleTarget);
						break;
				}
			}
		}

		#endregion
	}
}