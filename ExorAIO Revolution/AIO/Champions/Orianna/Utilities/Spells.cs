using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Spell = Entropy.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The spell class.
    /// </summary>
    internal partial class Orianna
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 825f);
            SpellClass.W = new Spell(SpellSlot.W);
            SpellClass.E = new Spell(SpellSlot.E, 1100f);
            SpellClass.R = new Spell(SpellSlot.R);

            SpellClass.Q.SetSkillshot(0.25f, 80f, 1400f, false, SkillshotType.Circle);
            SpellClass.W.SetSkillshot(0.25f, 255f, 3200f, false, SkillshotType.Circle);
            SpellClass.E.SetSkillshot(0f, 85f, 1850f, false, SkillshotType.Line);
            SpellClass.R.SetSkillshot(0.75f, 410f, 1000f, false, SkillshotType.Circle);
        }

        #endregion
    }
}