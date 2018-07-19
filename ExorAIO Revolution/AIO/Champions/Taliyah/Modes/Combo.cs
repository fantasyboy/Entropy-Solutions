// ReSharper disable ConvertIfStatementToConditionalTernaryExpression

using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK;
using Entropy.SDK.Caching;
using Entropy.SDK.Enumerations;
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
		///     Called on tick update.
		/// </summary>
		public void Combo(EntropyEventArgs args)
		{
			/// <summary>
			///     The Rylai Q Combo Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    UtilityClass.Player.HasItem(ItemID.RylaisCrystalScepter))
			{
				var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
				if (bestTarget != null &&
				    !Invulnerable.Check(bestTarget, DamageType.Magical))
				{
					switch (MenuClass.Q["modes"]["combo"].Value)
					{
						case 0:
							if (!IsNearWorkedGround())
							{
								SpellClass.Q.Cast(bestTarget);
							}

							break;
						case 1:
							SpellClass.Q.Cast(bestTarget);
							break;
					}
				}
			}

			/// <summary>
			///     The W->Boulders Combo Logic.
			/// </summary>
			if (SpellClass.W.Ready &&
			    MenuClass.W["boulders"].Enabled)
			{
				var bestTargets = TargetSelector.GetOrderedTargets(ObjectCache.EnemyHeroes)
					.Where(t => MenuClass.W["selection"][t.CharName.ToLower()].Value < 4);

				var objAiHeroes = bestTargets as AIHeroClient[] ?? bestTargets.ToArray();
				foreach (var target in objAiHeroes)
				{
					var bestBoulderHitPos = GetBestBouldersHitPosition(target);
					var bestBoulderHitPosHitBoulders = GetBestBouldersHitPositionHitBoulders(target);
					if (bestBoulderHitPos != Vector3.Zero && bestBoulderHitPosHitBoulders > 0)
					{
						SpellClass.W.Cast(bestBoulderHitPos, SpellClass.W.GetPrediction(target).CastPosition);
					}
				}
			}

			/// <summary>
			///     The W-> E Combo Logic.
			/// </summary>
			if (SpellClass.W.Ready &&
			    (SpellClass.E.Ready || !MenuClass.W["customization"]["onlyeready"].Enabled) &&
			    MenuClass.W["combo"].Enabled)
			{
				var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.W.Range);
				if (bestTarget != null &&
				    !Invulnerable.Check(bestTarget, DamageType.Magical, false))
				{
					switch (MenuClass.Root["pattern"].Value)
					{
						case 0:
							SpellClass.W.Cast(GetTargetPositionAfterW(bestTarget),
								SpellClass.W.GetPrediction(bestTarget).CastPosition);
							break;
					}
				}
			}

			/// <summary>
			///     The E Combo Logic.
			/// </summary>
			if (SpellClass.E.Ready &&
			    MenuClass.E["combo"].Enabled)
			{
				if (!SpellClass.W.Ready &&
				    MenuClass.E["customization"]["onlywready"].Enabled &&
				    UtilityClass.Player.Spellbook.GetSpellState(SpellSlot.W) != SpellState.NotLearned)
				{
					return;
				}

				if (MenuClass.Root["pattern"].Value == 0)
				{
					return;
				}

				var bestETarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range - 150f);
				if (bestETarget != null &&
				    !Invulnerable.Check(bestETarget, DamageType.Magical))
				{
					SpellClass.E.Cast(bestETarget.Position);
				}
			}

			/// <summary>
			///     The Q Combo Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    MenuClass.Q["combo"].Enabled)
			{
				var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
				if (bestTarget != null &&
				    !Invulnerable.Check(bestTarget, DamageType.Magical))
				{
					switch (MenuClass.Q["modes"]["combo"].Value)
					{
						case 0:
							if (!IsNearWorkedGround())
							{
								SpellClass.Q.Cast(bestTarget);
							}

							break;
						case 1:
							SpellClass.Q.Cast(bestTarget);
							break;
					}
				}
			}
		}

		#endregion
	}
}