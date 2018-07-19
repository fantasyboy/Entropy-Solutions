using AIO.Utilities;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;

namespace AIO.Champions
{
	/// <summary>
	///     The definitions class.
	/// </summary>
	internal partial class Taliyah
	{
		public double GetQDamage(AIBaseClient target, int shards = 1)
		{
			var qLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.Q).Level;
			var playerData = UtilityClass.Player.CharIntermediate;

			var qBaseDamage = new[] {70, 95, 120, 145, 170}[qLevel - 1]
			                  + 0.45f * playerData.TotalAbilityDamage();

			var addSubsequentShardDamage = qBaseDamage;
			switch (target.Type.TypeID)
			{
				case GameObjectTypeID.AIHeroClient:
					addSubsequentShardDamage /= 2;
					break;
			}

			var totalDamage = qBaseDamage + addSubsequentShardDamage * (shards - 1);

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Magical, totalDamage);
		}

		public double GetWDamage(AIBaseClient target)
		{
			var wLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.W).Level;
			var playerData = UtilityClass.Player.CharIntermediate;

			var wBaseDamage = new[] {60, 80, 100, 120, 140}[wLevel - 1]
			                  + 0.4f * playerData.TotalAbilityDamage();
			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Magical, wBaseDamage);
		}

		public double GetEDamage(AIBaseClient target)
		{
			var eLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.E).Level;
			var playerData = UtilityClass.Player.CharIntermediate;

			var eBaseDamage = new[] {50, 75, 100, 125, 150}[eLevel - 1]
			                  + 0.4f * playerData.TotalAbilityDamage();
			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Magical, eBaseDamage);
		}
	}
}