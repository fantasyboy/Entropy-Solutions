using Entropy;
using AIO.Utilities;
using Entropy.SDK.Damage;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Spells;

namespace AIO.Champions
{
    /// <summary>
    ///     The settings class.
    /// </summary>
    internal partial class Caitlyn
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 1250f);
            SpellClass.Q2 = new Spell(SpellSlot.Q, 1250f);
            SpellClass.W = new Spell(SpellSlot.W, 800f);
            SpellClass.E = new Spell(SpellSlot.E, 750f);
            SpellClass.R = new Spell(SpellSlot.R, 1500f + 500f * UtilityClass.Player.Spellbook.GetSpell(SpellSlot.R).Level);

            SpellClass.Q.SetSkillshot(1f, 60f, 2200f, SkillshotType.Line, false);
            SpellClass.Q2.SetSkillshot(SpellClass.Q.Delay, SpellClass.Q.Width*2, SpellClass.Q.Speed, SpellClass.Q.Type, SpellClass.Q.Collision);
            SpellClass.W.SetSkillshot(1.5f, 67.5f, float.MaxValue, SkillshotType.Circle, false);
            SpellClass.E.SetSkillshot(0.30f, 70f, 2000f);
        }

        #endregion
    }
}