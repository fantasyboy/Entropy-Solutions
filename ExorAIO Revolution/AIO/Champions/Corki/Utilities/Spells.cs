using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Spell = Entropy.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The spell class.
    /// </summary>
    internal partial class Corki
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 825f);
            SpellClass.W = new Spell(SpellSlot.W, 600f);
            SpellClass.W2 = new Spell(SpellSlot.W, 1800f);
            SpellClass.E = new Spell(SpellSlot.E, 600f + UtilityClass.Player.BoundingRadius * 2);
            SpellClass.R = new Spell(SpellSlot.R, 1250f);
            SpellClass.R2 = new Spell(SpellSlot.R, 1250f);

            SpellClass.Q.SetSkillshot(0.55f, 250f, 1000f, false, SkillshotType.Circle);
            SpellClass.W.SetSkillshot(0.25f, 100f, 650f, true, SkillshotType.Line);
            SpellClass.W2.SetSkillshot(0.25f, 100f, 1500f, true, SkillshotType.Line);
            SpellClass.E.SetSkillshot(0.3f, UtilityClass.GetAngleByDegrees(35f), 1500f, false, SkillshotType.Cone);
            SpellClass.R.SetSkillshot(0.25f, 75f, 2000f, true, SkillshotType.Line);
            SpellClass.R2.SetSkillshot(0.25f, 150f, 2000f, true, SkillshotType.Line);
        }

        #endregion
    }
}