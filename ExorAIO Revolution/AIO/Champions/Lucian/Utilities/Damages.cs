using AIO.Utilities;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;

namespace AIO.Champions
{
	/// <summary>
	///     The definitions class.
	/// </summary>
	internal partial class Lucian
	{
		public double GetQDamage(AIBaseClient target)
		{
			var qLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.Q).Level;
			var playerData = UtilityClass.Player.CharIntermediate;

			var qBaseDamage = new[] { 85f, 120f, 155f, 190f, 225f }[qLevel - 1]
			                  + new[] { 0.6f, 0.7f, 0.8f, 0.8f, 1f }[qLevel - 1] * playerData.FlatPhysicalDamageMod;
			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical, qBaseDamage);
		}

		public double GetWDamage(AIBaseClient target)
		{
			var wLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.W).Level;
			var playerData = UtilityClass.Player.CharIntermediate;

			var wBaseDamage = new[] { 85f, 125f, 165f, 205f, 245f }[wLevel - 1] + 0.9f * playerData.TotalAbilityDamage();

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Magical, wBaseDamage);
		}
	}
}