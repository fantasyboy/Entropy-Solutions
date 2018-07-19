using AIO.Utilities;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;

namespace AIO.Champions
{
	/// <summary>
	///     The definitions class.
	/// </summary>
	internal partial class Caitlyn
	{
		public float GetQDamage(AIBaseClient target, bool secondForm = false)
		{
			var qLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.Q).Level;
			var baseDamage = new[] {30f, 70f, 110f, 150f, 190f}[qLevel - 1]
			                 + new[] {1.3f, 1.4f, 1.5f, 1.6f, 1.7f}[qLevel - 1] *
			                 UtilityClass.Player.CharIntermediate.TotalAttackDamage();

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical,
				secondForm ? baseDamage * 0.67f : baseDamage);
		}

		public float GetRDamage(AIBaseClient target)
		{
			var rLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.R).Level;
			var baseDamage = new[] {250f, 475f, 700f}[rLevel - 1] +
			                 2 * UtilityClass.Player.CharIntermediate.FlatPhysicalDamageMod;

			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical, baseDamage);
		}
	}
}