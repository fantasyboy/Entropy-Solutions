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
	internal partial class Kaisa
	{
		public double GetPassiveExplodeDamage(AIBaseClient target)
		{
			if (target.IsStructure())
			{
				return 0f;
			}

			var playerLevel = UtilityClass.Player.Level();
			var passiveLevelScaling =
				playerLevel < 4
					? 0.15
					: playerLevel < 7
						? 0.16
						: playerLevel < 10
							? 0.17
							: playerLevel < 13
								? 0.18
								: playerLevel < 16
									? 0.19
									: 0.2;
			var passiveDamage =
				(passiveLevelScaling +
				 0.0375f * Math.Floor(UtilityClass.PlayerData.TotalAbilityDamage() / 100)) *
				(target.MaxHP - target.HP);

			switch (target.Type.TypeID)
			{
				case GameObjectTypeID.AIMinionClient:
					var jungleMonster = target as AIMinionClient;
					if (jungleMonster.IsJungleMinion())
					{
						const float minimumPassiveDamageAgainstMonsters = 400f;
						passiveDamage = Math.Min(minimumPassiveDamageAgainstMonsters, passiveDamage);
					}

					break;
			}

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Magical, (float) passiveDamage);
		}

		public double GetWDamage(AIBaseClient target)
		{
			var wLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.W).Level;

			var wBaseDamage = new[] {20f, 45f, 70f, 95f, 120f}[wLevel - 1]
			                  + 1.5f * UtilityClass.PlayerData.TotalAttackDamage()
			                  + 0.6f * UtilityClass.PlayerData.TotalAbilityDamage();

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Magical, wBaseDamage);
		}
	}
}