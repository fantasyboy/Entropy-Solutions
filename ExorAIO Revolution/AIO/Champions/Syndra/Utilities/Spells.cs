using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Spell = Entropy.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The spells class.
    /// </summary>
    internal partial class Syndra
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 800f);
            SpellClass.W = new Spell(SpellSlot.W, 950f);
            SpellClass.E = new Spell(SpellSlot.E, 950f);
            SpellClass.R = new Spell(SpellSlot.R, 675f);

            SpellClass.Q.SetSkillshot(0.65f, 60f, float.MaxValue, false, SkillshotType.Circle);
            SpellClass.W.SetSkillshot(0.45f, 40f, 2000f, false, SkillshotType.Circle);
            SpellClass.E.SetSkillshot(0.25f, UtilityClass.GetAngleByDegrees(40), 2500f, false, SkillshotType.Cone);
        }

        #endregion
    }
}