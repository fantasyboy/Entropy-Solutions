using AIO.Utilities;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;

namespace AIO.Champions
{
	/// <summary>
	///     The definitions class.
	/// </summary>
	internal partial class Orianna
	{
		public float GetQDamage(AIBaseClient target)
		{
			var qLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.Q).Level;
			var baseDamage = new[] { 60f, 90f, 120f, 150f, 180f }[qLevel - 1]
			                 + 0.5f * UtilityClass.Player.CharIntermediate.TotalAbilityDamage();

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Magical, baseDamage);
		}

		public float GetWDamage(AIBaseClient target)
		{
			var wLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.W).Level;
			var baseDamage = new[] { 60f, 105f, 150f, 195f, 240f }[wLevel - 1]
			                 + 0.7f * UtilityClass.Player.CharIntermediate.TotalAbilityDamage();

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Magical, baseDamage);
		}

		public float GetEDamage(AIBaseClient target)
		{
			var eLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.E).Level;
			var baseDamage = new[] { 60f, 90f, 120f, 150f, 180f }[eLevel - 1]
			                 + 0.3f * UtilityClass.Player.CharIntermediate.TotalAbilityDamage();

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Magical, baseDamage);
		}

		public float GetRDamage(AIBaseClient target)
		{
			var rLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.R).Level;
			var baseDamage = new[] { 150f, 225f, 300f }[rLevel - 1]
			                 + 0.7f * UtilityClass.Player.CharIntermediate.TotalAbilityDamage();

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Magical, baseDamage);
		}
	}
}