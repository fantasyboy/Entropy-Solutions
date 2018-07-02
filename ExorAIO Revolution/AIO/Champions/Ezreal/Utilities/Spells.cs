using Entropy;
using Entropy.SDK.Prediction.Skillshots;
using AIO.Utilities;
using Spell = Entropy.SDK.Spell;

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

            SpellClass.Q.SetSkillshot(0.25f, 60f, 2000f, true, SkillshotType.Line, hitchance: HitChance.Medium);
            SpellClass.W.SetSkillshot(0.25f, 80f, 1600f, false, SkillshotType.Line);
            SpellClass.R.SetSkillshot(1f, 160f, 2000f, false, SkillshotType.Line);
        }

        #endregion
    }
}