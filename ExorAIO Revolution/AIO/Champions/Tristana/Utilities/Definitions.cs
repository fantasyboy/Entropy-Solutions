
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Tristana
    {
        #region Fields

        /// <summary>
        ///     Returns true if the target is affected by the E charge, else, false.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public bool IsCharged(AIBaseClient unit)
        {
            return unit.HasBuff("TristanaECharge");
        }

        /// <summary>
        ///     Gets the total explosion damage on a determined unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public double GetTotalExplosionDamage(AIBaseClient unit)
        {
            var player = UtilityClass.Player;
            return player.GetSpellDamage(unit, SpellSlot.E) +
                   player.GetSpellDamage(unit, SpellSlot.E, DamageStage.Buff);
        }

        /// <summary>
        ///     Returns true if the target is a perfectly valid charge target.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public bool IsPerfectChargeTarget(AIBaseClient unit)
        {
            if (IsCharged(unit) &&
                unit.IsValidSpellTarget())
            {
                switch (unit.Type)
                {
                    case GameObjectType.AIMinionClient:
                        return true;

                    case GameObjectType.AIHeroClient:
                        var heroUnit = (AIHeroClient)unit;
                        return !Invulnerable.Check(heroUnit);
                }
            }

            return false;
        }

        #endregion
    }
}