using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Prediction.Skillshots;
using AIO.Utilities;
using Spell = Aimtec.SDK.Spell;

namespace AIO.Champions
{
    /// <summary>
    ///     The spell class.
    /// </summary>
    internal partial class Tristana
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q);
            SpellClass.W = new Spell(SpellSlot.W, 900f);

            var target = ImplementationClass.IOrbwalker.GetOrbwalkingTarget();
            SpellClass.E = new Spell(SpellSlot.E, target != null ? UtilityClass.Player.GetFullAttackRange(target) : UtilityClass.Player.AttackRange);
            SpellClass.R = new Spell(SpellSlot.R, target != null ? UtilityClass.Player.GetFullAttackRange(target) : UtilityClass.Player.AttackRange);

            SpellClass.W.SetSkillshot(0.75f, 60f, 1000f, false, SkillshotType.Circle);
        }

        #endregion
    }
}