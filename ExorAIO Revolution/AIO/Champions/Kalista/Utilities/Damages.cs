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
			var qLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.Q).Level;
			var playerData = UtilityClass.Player.CharIntermediate;

			var qBaseDamage =  new[] { 10, 70, 130, 190, 250 }[qLevel - 1] + playerData.TotalAttackDamage();
			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical, qBaseDamage);
		}

		public double GetEDamage(AIBaseClient target)
		{
			var eLevel = UtilityClass.Player.Spellbook.GetSpell(SpellSlot.E).Level;
			var playerData = UtilityClass.Player.CharIntermediate;

			var eBaseDamage  = new[] { 20, 30, 40, 50, 60 }[eLevel - 1] + 0.6f * playerData.TotalAttackDamage();
			var eStackDamage = new[] { 10, 14, 19, 25, 32 }[eLevel - 1] + new[] { 0.20f, 0.2375f, 0.275f, 0.3125f, 0.35f }[eLevel - 1] * playerData.TotalAttackDamage();

			var eStacksOnTarget = target.GetBuffCount("kalistaexpungemarker");
			if (eStacksOnTarget == 0)
			{
				return 0;
			}

			var total = eBaseDamage + eStackDamage * (eStacksOnTarget - 1);
			return LocalPlayer.Instance.CalculateDamage(target, DamageType.Physical, total);
		}

		#endregion
	}
}