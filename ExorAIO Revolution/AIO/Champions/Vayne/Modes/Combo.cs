using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Events;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Caching;

#pragma warning disable 1587

namespace AIO.Champions
{
	/// <summary>
	///     The champion class.
	/// </summary>
	internal partial class Vayne
	{
		#region Public Methods and Operators

		/// <summary>
		///     Fired when the game is updated.
		/// </summary>
		public void Combo(EntropyEventArgs args)
		{
			/// <summary>
			///     The Q Engager Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    MenuClass.Q["engage"].Enabled)
			{
				var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
				if (bestTarget != null &&
				    !Invulnerable.Check(bestTarget) &&
				    !bestTarget.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(bestTarget)))
				{
					var posAfterQ = UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, 300f);
					if (posAfterQ.EnemyHeroesCount(1000f) < 3 &&
					    UtilityClass.Player.Distance(Hud.CursorPositionUnclipped) >
					    UtilityClass.Player.GetAutoAttackRange() &&
					    bestTarget.Distance(posAfterQ) < UtilityClass.Player.GetAutoAttackRange(bestTarget))
					{
						SpellClass.Q.Cast(posAfterQ);
					}
				}
			}

			/// <summary>
			///     The E Stun Logic.
			/// </summary>
			if (SpellClass.E.Ready &&
			    !UtilityClass.Player.IsDashing() &&
			    MenuClass.E["emode"].Value != 2)
			{
				const int condemnPushDistance = 475;
				const int threshold = 75;

				foreach (var target in
					ObjectCache.EnemyHeroes.Where(t =>
						!t.IsDashing() &&
						t.IsValidTarget(SpellClass.E.Range) &&
						!Invulnerable.Check(t, DamageType.Magical, false) &&
						MenuClass.E["whitelist"][t.CharName.ToLower()].Enabled))
				{
					for (var i = UtilityClass.Player.BoundingRadius; i < condemnPushDistance - threshold; i += 10)
					{
						switch (MenuClass.E["emode"].Value)
						{
							case 0:
								if (IsPerfectWallPosition(target.Position, target, UtilityClass.Player.Position, i))
								{
									if (target.IsImmobile(SpellClass.E.Delay))
									{
										SpellClass.E.CastOnUnit(target);
										break;
									}

									var estimatedPosition = EstimatedPosition(target, SpellClass.E.Delay);
									if (IsPerfectWallPosition(estimatedPosition, target, UtilityClass.Player.Position, i))
									{
										SpellClass.E.CastOnUnit(target);
									}
								}
								break;

							default:
								if (IsPerfectWallPosition(target.Position, target, UtilityClass.Player.Position, i))
								{
									SpellClass.E.CastOnUnit(target);
								}
								break;
						}
					}
				}
			}
		}

		#endregion
	}
}