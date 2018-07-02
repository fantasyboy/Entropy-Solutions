using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using AIO.Utilities;
using Spell = Aimtec.SDK.Spell;

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
            SpellClass.E = new Spell(SpellSlot.E, 800f);
            SpellClass.R = new Spell(SpellSlot.R, 1500 + 1500 * SpellClass.R.Level);

            SpellClass.Q.SetSkillshot(0.25f, 100f, 3600f, true, SkillshotType.Line);
            SpellClass.W.SetSkillshot(1f, 200f, float.MaxValue, false, SkillshotType.Circle, true);
            SpellClass.E.SetSkillshot(0.30f, UtilityClass.GetAngleByDegrees(80f), float.MaxValue, false, SkillshotType.Cone);
        }

        #endregion
    }
}