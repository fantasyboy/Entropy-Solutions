using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Spells;

namespace AIO.Champions
{
    /// <summary>
    ///     The settings class.
    /// </summary>
    internal partial class Jhin
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 600f);
            SpellClass.W = new Spell(SpellSlot.W, 2500f);
            SpellClass.E = new Spell(SpellSlot.E, 750f);
            SpellClass.R = new Spell(SpellSlot.R);
            SpellClass.R2 = new Spell(SpellSlot.R, 3500f);

            SpellClass.W.SetSkillshot(0.75f, 40, float.MaxValue, collision: false);
            SpellClass.E.SetSkillshot(1, 260, 1000, SkillshotType.Circle, false);

	        SpellClass.R.Width = 55; // Cone Angle degrees.
			SpellClass.R2.SetSkillshot(0.25f, 80, 5000, collision: false);
        }

        #endregion
    }
}