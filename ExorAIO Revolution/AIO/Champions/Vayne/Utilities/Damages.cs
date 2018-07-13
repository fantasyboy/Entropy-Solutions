using System;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;

namespace AIO.Champions
{
	/// <summary>
	///     The definitions class.
	/// </summary>
	internal partial class Vayne
	{
		public double GetQBonusDamage(AIBaseClient target)
		{
			var qLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.Q).Level;
			var playerData = UtilityClass.Player.CharIntermediate;

			var qBaseDamage = new[] { 0.5f, 0.55f, 0.6f, 0.65f, 0.7f }[qLevel - 1] * playerData.TotalAttackDamage();
			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical, qBaseDamage);
		}

		public double GetWDamage(AIBaseClient target)
		{
			if (target.GetBuffCount("vaynesilvereddebuff") != 2)
			{
				return 0d;
			}

			var wLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.W).Level;

			var wThreshold = new[] { 50, 65, 80, 95f, 110 }[wLevel - 1];
			var wBaseDamage = new[] { 0.04f, 0.06f, 0.08f, 0.1f, 0.12f }[wLevel - 1] * target.MaxHP;

			switch (target.Type.TypeID)
			{
				case GameObjectTypeID.AIMinionClient:
					var jungleMinion = target as AIMinionClient;
					if (jungleMinion != null)
					{
						if (jungleMinion.IsJungleMinion())
						{
							const int wDamageCapAgainstMonsters = 200;
							return Math.Max(wThreshold, Math.Min(wDamageCapAgainstMonsters, wBaseDamage));
						}

						return Math.Max(wThreshold, wBaseDamage);
					}
					break;
				case GameObjectTypeID.AIHeroClient:
					return Math.Max(wThreshold, wBaseDamage);
			}

			return 0;
		}

		public double GetEDamage(AIBaseClient target)
		{
			var eLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.E).Level;
			var playerData = UtilityClass.Player.CharIntermediate;

			var eBaseDamage = new[] { 50, 90, 120, 155, 190 }[eLevel - 1] + 0.5f * playerData.FlatPhysicalDamageMod;
			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical, eBaseDamage);
		}
	}
}