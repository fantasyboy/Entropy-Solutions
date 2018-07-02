using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using AIO.Utilities;
using Spell = Aimtec.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The spell class.
    /// </summary>
    internal partial class Evelynn
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 800f);
            SpellClass.Q2 = new Spell(SpellSlot.Q, 550f);
            SpellClass.W = new Spell(SpellSlot.W, 1200f);
            SpellClass.E = new Spell(SpellSlot.E, 225f + UtilityClass.Player.BoundingRadius);
            SpellClass.R = new Spell(SpellSlot.R, 450f + UtilityClass.Player.BoundingRadius);

            SpellClass.Q.SetSkillshot(0.25f, 60f, 2400f, true, SkillshotType.Line);
            SpellClass.R.SetSkillshot(0.25f, UtilityClass.GetAngleByDegrees(180), float.MaxValue, false, SkillshotType.Cone);
        }

        #endregion
    }
}