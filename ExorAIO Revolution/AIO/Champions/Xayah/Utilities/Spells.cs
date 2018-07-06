using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Spell = Entropy.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The settings class.
    /// </summary>
    internal partial class Xayah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 1100f);
            SpellClass.W = new Spell(SpellSlot.W);
            SpellClass.E = new Spell(SpellSlot.E, float.MaxValue);
            SpellClass.R = new Spell(SpellSlot.R, 1100f);

            SpellClass.Q.SetSkillshot(0.25f, 60f, 1400f, false, SkillshotType.Line);
            SpellClass.E.SetSkillshot(0.5f,  45f, 2000f, false, SkillshotType.Line);
            SpellClass.R.SetSkillshot(0.25f, 80f, 2000f, false, SkillshotType.Line);
        }

        #endregion
    }
}