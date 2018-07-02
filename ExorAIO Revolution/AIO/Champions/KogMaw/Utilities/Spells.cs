using Entropy;
using Entropy.SDK.Prediction.Skillshots;
using AIO.Utilities;
using Spell = Entropy.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The spells class.
    /// </summary>
    internal partial class KogMaw
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 1175f);
            SpellClass.W = new Spell(SpellSlot.W, UtilityClass.Player.AttackRange + (60f + 30f * SpellClass.W.Level));
            SpellClass.E = new Spell(SpellSlot.E, 1280f);
            SpellClass.R = new Spell(SpellSlot.R, 900f + 300f * SpellClass.R.Level);

            SpellClass.Q.SetSkillshot(0.35f, 70f+25f, 1650f, true, SkillshotType.Line);
            SpellClass.E.SetSkillshot(0.25f, 120f, 1400f, false, SkillshotType.Line);
            SpellClass.R.SetSkillshot(1.25f, 240f, float.MaxValue, false, SkillshotType.Circle);
        }

        #endregion
    }
}