using Entropy;
using Entropy.SDK.Prediction.Skillshots;
using AIO.Utilities;
using Spell = Entropy.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The spells class.
    /// </summary>
    internal partial class Ahri
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 1000f);
            SpellClass.W = new Spell(SpellSlot.W, 550f);
            SpellClass.E = new Spell(SpellSlot.E, 1000f);
            SpellClass.R = new Spell(SpellSlot.R, 450f);

            SpellClass.Q.SetSkillshot(0.25f, 100f, 2500f, false, SkillshotType.Line);
            SpellClass.E.SetSkillshot(0.25f, 60f, 1550f, true, SkillshotType.Line);
        }

        #endregion
    }
}