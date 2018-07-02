
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Darius
    {
        #region Fields

        /// <summary>
        ///     Returns true if the target is a perfectly valid blade target.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public bool IsValidBladeTarget(Obj_AI_Base unit)
        {
            var unitDistanceToPlayer = unit.Distance(UtilityClass.Player);
            return
                unit.IsValidSpellTarget() &&
                unitDistanceToPlayer >= UtilityClass.Player.AttackRange &&
                unitDistanceToPlayer <= SpellClass.Q.Range;
        }

        /// <summary>
        ///     Gets the real Damage the R spell would deal to a determined enemy hero.
        /// </summary>
        /// <param name="target">The target.</param>
        public double GetTotalNoxianGuillotineDamage(Obj_AI_Hero target)
        {
            var player = UtilityClass.Player;
            return
                player.GetSpellDamage(target, SpellSlot.R) +
                player.GetSpellDamage(target, SpellSlot.R, DamageStage.Buff);
        }

        #endregion
    }
}