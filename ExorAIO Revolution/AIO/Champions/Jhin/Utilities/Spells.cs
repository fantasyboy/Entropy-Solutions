using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using AIO.Utilities;
using Spell = Aimtec.SDK.Spell;

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
            SpellClass.R = new Spell(SpellSlot.R, 3500f);
            SpellClass.R2 = new Spell(SpellSlot.R, 3500f);

            SpellClass.W.SetSkillshot(0.75f, 40f, float.MaxValue, false, SkillshotType.Line, false, HitChance.Medium);
            SpellClass.E.SetSkillshot(1f, 260f, 1000f, false, SkillshotType.Circle);
            SpellClass.R.SetSkillshot(0.25f, 80f, 5000f, false, SkillshotType.Line);
            SpellClass.R2.SetSkillshot(0.25f, UtilityClass.GetAngleByDegrees(55), 5000f, false, SkillshotType.Line);
        }

        #endregion
    }
}