using Entropy;
using AIO.Utilities;
using Entropy.SDK.Spells;

namespace AIO.Champions
{
	/// <summary>
	///     The spell class.
	/// </summary>
	internal partial class Kaisa
	{
		#region Public Methods and Operators

		/// <summary>
		///     Sets the spells.
		/// </summary>
		public void Spells()
		{
			SpellClass.Q = new Spell(SpellSlot.Q);
			SpellClass.W = new Spell(SpellSlot.W, 3000f);
			SpellClass.E = new Spell(SpellSlot.E);
			SpellClass.R = new Spell(SpellSlot.R,
				1000f + 500f * UtilityClass.Player.Spellbook.GetSpell(SpellSlot.R).Level);

			SpellClass.W.SetSkillshot(0.50f, 80f, 1650f);
		}

		#endregion
	}
}