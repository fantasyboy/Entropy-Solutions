using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Spells;

namespace AIO.Champions
{
	/// <summary>
	///     The spell class.
	/// </summary>
	internal partial class Taliyah
	{
		#region Public Methods and Operators

		/// <summary>
		///     Initializes the spells.
		/// </summary>
		public void Spells()
		{
			SpellClass.Q = new Spell(SpellSlot.Q, 1000f);
			SpellClass.W = new Spell(SpellSlot.W, 900f);
			SpellClass.E = new Spell(SpellSlot.E, 800f - 100f);
			SpellClass.R = new Spell(SpellSlot.R,
				1500 + 1500 * UtilityClass.Player.Spellbook.GetSpell(SpellSlot.R).Level);

			SpellClass.Q.SetSkillshot(0.5f, 100f, 3600f);
			SpellClass.W.SetSkillshot(1f, 200f, float.MaxValue, SkillshotType.Circle, false);
		}

		#endregion
	}
}