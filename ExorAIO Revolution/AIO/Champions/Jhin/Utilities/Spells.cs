using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Spells;

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
            SpellClass.R = new Spell(SpellSlot.R);
            SpellClass.R2 = new Spell(SpellSlot.R, 3500f);

            SpellClass.W.SetSkillshot(0.75f, 40f, float.MaxValue, collision: false);
            SpellClass.E.SetSkillshot(1f, 260f, 1000f, SkillshotType.Circle, false);
            SpellClass.R2.SetSkillshot(0.25f, UtilityClass.GetAngleByDegrees(55), 5000f, collision: false);
        }

        #endregion
    }
}