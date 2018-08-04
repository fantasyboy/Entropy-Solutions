using AIO.Utilities;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;

namespace AIO.Champions
{
	/// <summary>
	///     The definitions class.
	/// </summary>
	internal partial class Jhin
	{
		#region Fields

		public float GetQDamage(AIBaseClient target)
		{
			var qLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.Q).Level;

			var qDamage = new[] { 45, 70, 95, 120, 145 }[qLevel - 1]
				+ new[] { 0.4f, 0.475f, 0.55f, 0.625f, 0.7f }[qLevel - 1] * UtilityClass.PlayerData.TotalAttackDamage();

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical, qDamage);
		}

		public double GetWDamage(AIBaseClient target)
		{
			var wLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.W).Level;

			var wDamage = new[] { 50, 85, 120, 155, 190 }[wLevel - 1]
				+ 0.5f * UtilityClass.PlayerData.TotalAttackDamage();

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical, wDamage);
		}

		public double GetEDamage(AIBaseClient target, int hits = 1)
		{
			var eLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.E).Level;

			var eDamage = new[] { 20, 80, 140, 200, 260 }[eLevel - 1]
				+ 1.2f * UtilityClass.PlayerData.TotalAttackDamage()
				+ UtilityClass.PlayerData.TotalAbilityDamage();

			var eDamageReduced = 0.65f * eDamage;

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Magical, eDamage + (eDamageReduced * (hits - 1)));
		}

		public double GetRDamage(AIBaseClient target, bool isLastShot = false)
		{
			var rLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.R).Level;

			var rDamage = new[] { 50, 125, 200 }[rLevel - 1]
				+ 0.2f * UtilityClass.PlayerData.TotalAttackDamage() * (0.25f * target.MissingHPPercent());

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical, rDamage * (isLastShot ? 2 : 1));
		}

		#endregion
	}
}