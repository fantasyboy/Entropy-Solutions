using AIO.Utilities;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;
using System;

namespace AIO.Champions
{
	/// <summary>
	///     The definitions class.
	/// </summary>
	internal partial class Xayah
	{
		public double GetQDamage(AIBaseClient target, bool collision = true)
		{
			var qLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.Q).Level;

			var qBaseDamage = new[] {45, 65, 85, 105, 125}[qLevel - 1]
			                  + 0.5f * UtilityClass.PlayerData.BonusPhysicalDamage();
			var totalDamage = qBaseDamage * 2 / (collision ? 2 : 1);

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical, totalDamage);
		}

		public double GetEDamage(AIBaseClient target, int feathers)
		{
			var eLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.E).Level;

			var eBaseDamage = (new[] {55, 65, 75, 85, 95}[eLevel - 1]
			                  + 0.6f * UtilityClass.PlayerData.BonusPhysicalDamage())
							  * 1 + 0.5f * UtilityClass.Player.CharIntermediate.Crit;

			var totalDamage = eBaseDamage;
			float multiplier = 1;
			for (var cycle = 1; cycle < feathers - 1; cycle++)
			{
				multiplier -= 0.1f * cycle;
				totalDamage += eBaseDamage * Math.Max(0.1f, multiplier);
			}

			var laneMinion = target as AIMinionClient;
			if (laneMinion != null &&
				laneMinion.IsLaneMinion())
			{
				totalDamage /= 2;
			}

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical, totalDamage);
		}
	}
}