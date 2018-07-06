using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Objects;
using Spell = Entropy.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The spells class.
    /// </summary>
    internal partial class MissFortune
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, UtilityClass.Player.GetAutoAttackRange()+UtilityClass.Player.BoundingRadius);
            SpellClass.Q2 = new Spell(SpellSlot.Q, SpellClass.Q.Range + 500f);
            SpellClass.W = new Spell(SpellSlot.W);
            SpellClass.E = new Spell(SpellSlot.E, 1000f);
            SpellClass.R = new Spell(SpellSlot.R, 1400f);

            SpellClass.Q.SetSkillshot(0.25f, 80f, 1000f, false, SkillshotType.Line);
            SpellClass.Q2.SetSkillshot(0.25f, UtilityClass.GetAngleByDegrees(40), 1000f, false, SkillshotType.Line);
            SpellClass.E.SetSkillshot(0.5f, 350f, 500f, false, SkillshotType.Circle);
            SpellClass.R.SetSkillshot(0.334f, UtilityClass.GetAngleByDegrees(40), 780f, false, SkillshotType.Cone);
        }

        #endregion
    }
}