using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using AIO.Utilities;
using Spell = Aimtec.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The spell class.
    /// </summary>
    internal partial class Varus
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 925f+UtilityClass.Player.BoundingRadius);
            SpellClass.W = new Spell(SpellSlot.W);
            SpellClass.E = new Spell(SpellSlot.E, 925f);
            SpellClass.R = new Spell(SpellSlot.R, 1200f);

            SpellClass.Q.SetSkillshot(0.25f, 60f, 1850f, false, SkillshotType.Line);
            SpellClass.Q.SetCharged("VarusQ", "VarusQ", (int)(925+UtilityClass.Player.BoundingRadius), 1625, 1.3f);

            SpellClass.E.SetSkillshot(0.75f, 235f, 1000f, false, SkillshotType.Circle);
            SpellClass.R.SetSkillshot(0.25f, 120f, 1850f, false, SkillshotType.Line);
        }

        #endregion
    }
}