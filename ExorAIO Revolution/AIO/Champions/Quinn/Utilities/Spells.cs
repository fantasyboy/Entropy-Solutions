using Entropy;
using Entropy.SDK.Prediction.Skillshots;
using AIO.Utilities;
using Spell = Entropy.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The spells class.
    /// </summary>
    internal partial class Quinn
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 1025f);
            SpellClass.W = new Spell(SpellSlot.W, 2100f);
            SpellClass.E = new Spell(SpellSlot.E, 675f + UtilityClass.Player.BoundingRadius);
            SpellClass.R = new Spell(SpellSlot.R);

            SpellClass.Q.SetSkillshot(0.25f, 90f, 1550f, true, SkillshotType.Line, hitchance: HitChance.Medium);
        }

        #endregion
    }
}