
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;

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
        public bool IsValidBladeTarget(AIBaseClient unit)
        {
            var unitDistanceToPlayer = unit.DistanceToPlayer();
            return
                unit.IsValidSpellTarget() &&
                unitDistanceToPlayer >= UtilityClass.Player.GetAutoAttackRange() &&
                unitDistanceToPlayer <= SpellClass.Q.Range;
        }

        /// <summary>
        ///     Gets the real Damage the R spell would deal to a determined enemy hero.
        /// </summary>
        /// <param name="target">The target.</param>
        public double GetTotalNoxianGuillotineDamage(AIHeroClient target)
        {
            var player = UtilityClass.Player;
            return
                player.GetSpellDamage(target, SpellSlot.R) +
                player.GetSpellDamage(target, SpellSlot.R, DamageStage.Buff);
        }

        #endregion
    }
}