using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using AIO.Utilities;
using Spell = Aimtec.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The spell class.
    /// </summary>
    internal partial class Ashe
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q);
            SpellClass.W = new Spell(SpellSlot.W, UtilityClass.Player.BoundingRadius + 1200f);
            SpellClass.E = new Spell(SpellSlot.E, 2000f);
            SpellClass.R = new Spell(SpellSlot.R, 2000f);

            SpellClass.W.SetSkillshot(0.25f, UtilityClass.GetAngleByDegrees(67.5f), 1500f, true, SkillshotType.Cone);
            SpellClass.E.SetSkillshot(0.25f, 130f, 1600f, false, SkillshotType.Line);
            SpellClass.R.SetSkillshot(0.25f, 130f, 1600f, false, SkillshotType.Line);
        }

        #endregion
    }
}