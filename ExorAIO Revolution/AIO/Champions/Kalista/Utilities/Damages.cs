using AIO.Utilities;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;

namespace AIO.Champions
{
	/// <summary>
	///     The definitions class.
	/// </summary>
	internal partial class Kalista
	{
		#region Fields

		public double GetQDamage(AIBaseClient target)
		{
			var QLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.E).Level;
			var playerData = UtilityClass.Player.CharIntermediate;

			var QBaseDamage =  new[] { 10, 70, 130, 190, 250 }[QLevel - 1] + playerData.TotalAttackDamage();
			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical, QBaseDamage);
		}

		public double GetEDamage(AIBaseClient target)
		{
			var ELevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.E).Level;
			var playerData = UtilityClass.Player.CharIntermediate;

			var EBaseDamage  = new[] { 20, 30, 40, 50, 60 }[ELevel - 1] + 0.6f * playerData.TotalAttackDamage();
			var EStackDamage = new[] { 10, 14, 19, 25, 32 }[ELevel - 1] + new[] { 0.20f, 0.2375f, 0.275f, 0.3125f, 0.35f }[ELevel - 1] * playerData.TotalAttackDamage();

			var EStacksOnTarget = target.GetBuffCount("kalistaexpungemarker");
			if (EStacksOnTarget == 0)
			{
				return 0;
			}

			var total = EBaseDamage + EStackDamage * (EStacksOnTarget - 1);
			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical, total);
		}

		#endregion
	}
}