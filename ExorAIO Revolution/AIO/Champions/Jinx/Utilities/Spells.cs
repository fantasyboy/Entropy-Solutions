using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Spell = Entropy.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The spells class.
    /// </summary>
    internal partial class Jinx
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 525f + UtilityClass.Player.BoundingRadius);
            SpellClass.Q2 = new Spell(SpellSlot.Q, SpellClass.Q.Range + 50f + 25f * SpellClass.Q.Level());
            SpellClass.W = new Spell(SpellSlot.W, 1450f);
            SpellClass.E = new Spell(SpellSlot.E, 900f);
            SpellClass.R = new Spell(SpellSlot.R, 1500f);

            SpellClass.W.SetSkillshot(0.75f, 60f+25f, 3300f, true, SkillshotType.Line);
            SpellClass.E.SetSkillshot(0.95f, 325f, 1750f, false, SkillshotType.Circle);
            SpellClass.R.SetSkillshot(1.1f, 140f, 2500f, false, SkillshotType.Line);
        }

        #endregion
    }
}