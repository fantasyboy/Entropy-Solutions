using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Spells;

namespace AIO.Champions
{
	/// <summary>
	///     The spells class.
	/// </summary>
	internal partial class Lucian
	{
		#region Public Methods and Operators

		/// <summary>
		///     Sets the spells.
		/// </summary>
		public void Spells()
		{
			SpellClass.Q = new Spell(SpellSlot.Q, 550f);
			SpellClass.Q2 = new Spell(SpellSlot.Q, SpellClass.Q.Range + 400f - UtilityClass.Player.BoundingRadius);
			SpellClass.W = new Spell(SpellSlot.W, 900f);
			SpellClass.E = new Spell(SpellSlot.E, UtilityClass.Player.GetAutoAttackRange() + 425f);
			SpellClass.R = new Spell(SpellSlot.R, 1150f);

			SpellClass.Q2.SetSkillshot(0.25f, 65f, float.MaxValue, collision: false);
			SpellClass.W.SetSkillshot(0.25f, 80f, 1600f, collision: false);
			SpellClass.R.SetSkillshot(0.25f, 110f, 2500f, collision: false);
		}

		#endregion
	}
}