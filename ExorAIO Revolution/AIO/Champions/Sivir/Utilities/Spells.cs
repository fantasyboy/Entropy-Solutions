using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using AIO.Utilities;
using Spell = Aimtec.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The spell class.
    /// </summary>
    internal partial class Sivir
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 1200f);
            SpellClass.W = new Spell(SpellSlot.W, UtilityClass.Player.AttackRange+UtilityClass.Player.BoundingRadius);
            SpellClass.E = new Spell(SpellSlot.E);

            SpellClass.Q.SetSkillshot(0.25f, 90f, 1350f, false, SkillshotType.Line);
        }

        #endregion
    }
}