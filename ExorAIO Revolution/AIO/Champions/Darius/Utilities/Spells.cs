using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using AIO.Utilities;
using Spell = Aimtec.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The spell class.
    /// </summary>
    internal partial class Darius
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 425f);
            SpellClass.W = new Spell(SpellSlot.W);
            SpellClass.E = new Spell(SpellSlot.E, 550f);
            SpellClass.R = new Spell(SpellSlot.R, 460f);

            SpellClass.E.SetSkillshot(0.25f, UtilityClass.GetAngleByDegrees(50f), 1000f, false, SkillshotType.Cone);
        }

        #endregion
    }
}