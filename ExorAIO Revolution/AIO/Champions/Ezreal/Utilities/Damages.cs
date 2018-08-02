using AIO.Utilities;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;

namespace AIO.Champions
{
	/// <summary>
	///     The definitions class.
	/// </summary>
	internal partial class Ezreal
	{
		public double GetQDamage(AIBaseClient target)
		{
			var playerData = UtilityClass.Player.CharIntermediate;
			var qLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.Q).Level;

			var qBaseDamage = new[] { 15f, 40f, 65f, 90f, 115f }[qLevel - 1]
							  + 1.1f * playerData.TotalAttackDamage()
							  + 0.4f * playerData.TotalAbilityDamage();

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical, qBaseDamage);
		}

		public double GetWDamage(AIBaseClient target)
		{
			var playerData = UtilityClass.Player.CharIntermediate;
			var wLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.W).Level;

			var wBaseDamage = new[] { 70f, 115f, 160f, 205f, 250f }[wLevel - 1]
							  + 0.8f * playerData.TotalAbilityDamage();

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Magical, wBaseDamage);
		}

		public double GetRDamage(AIBaseClient target)
		{
			var playerData = UtilityClass.Player.CharIntermediate;
			var rLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.R).Level;

			var rBaseDamage = new[] { 350f, 500, 650 }[rLevel - 1]
							  + playerData.BonusPhysicalDamage()
							  + 0.9f * playerData.TotalAbilityDamage();

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Magical, rBaseDamage);
		}
	}
}