using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Objects;
using Spell = Entropy.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The settings class.
    /// </summary>
    internal partial class Twitch
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q);
            SpellClass.W = new Spell(SpellSlot.W, 950f);
            SpellClass.E = new Spell(SpellSlot.E, 1200f);
            SpellClass.R = new Spell(SpellSlot.R, UtilityClass.Player.GetAutoAttackRange() + 300f);

            SpellClass.W.SetSkillshot(0.25f, 120f, 1400f, false, SkillshotType.Circle);
        }

        #endregion
    }
}