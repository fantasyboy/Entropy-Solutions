using Entropy;
using AIO.Utilities;
using Entropy.SDK.Spells;

namespace AIO.Champions
{
	/// <summary>
	///     The spells class.
	/// </summary>
	internal partial class Ezreal
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 1150f);
            SpellClass.W = new Spell(SpellSlot.W, 1000f);
            SpellClass.E = new Spell(SpellSlot.E, 475f);
            SpellClass.R = new Spell(SpellSlot.R, 1500f);

            SpellClass.Q.SetSkillshot(0.25f, 60f+20f, 2000f);
            SpellClass.W.SetSkillshot(0.25f, 80f, 1550f, collision: false);
            SpellClass.R.SetSkillshot(1f, 160f, 2000f, collision: false);
        }

        #endregion
    }
}