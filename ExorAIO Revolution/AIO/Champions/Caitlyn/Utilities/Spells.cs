using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Spell = Entropy.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The settings class.
    /// </summary>
    internal partial class Caitlyn
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 1250f);
            SpellClass.Q2 = new Spell(SpellSlot.Q, 1250f);
            SpellClass.W = new Spell(SpellSlot.W, 800f);
            SpellClass.E = new Spell(SpellSlot.E, 950f);
            SpellClass.R = new Spell(SpellSlot.R, 1500f + 500f * SpellClass.R.Level);

            SpellClass.Q.SetSkillshot(0.65f, 60f, 2200f, false, SkillshotType.Line);
            SpellClass.Q2.SetSkillshot(0.65f, 120f, 2200f, false, SkillshotType.Line);
            SpellClass.W.SetSkillshot(1.5f, 67.5f, float.MaxValue, false, SkillshotType.Circle);
            SpellClass.E.SetSkillshot(0.30f, 70f, 2000f, true, SkillshotType.Line);
        }

        #endregion
    }
}