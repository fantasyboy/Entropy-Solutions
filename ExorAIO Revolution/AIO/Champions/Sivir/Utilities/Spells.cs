using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Objects;
using Spell = Entropy.SDK.Spell;

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
            SpellClass.W = new Spell(SpellSlot.W, UtilityClass.Player.GetAutoAttackRange()+UtilityClass.Player.BoundingRadius);
            SpellClass.E = new Spell(SpellSlot.E);

            SpellClass.Q.SetSkillshot(0.25f, 90f, 1350f, false, SkillshotType.Line);
        }

        #endregion
    }
}