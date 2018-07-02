
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Damage.JSON;
using Entropy.SDK.Extensions;
using AIO.Utilities;

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
        public bool IsCharged(Obj_AI_Base unit)
        {
            return unit.HasBuff("TristanaECharge");
        }

        /// <summary>
        ///     Gets the total explosion damage on a determined unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public double GetTotalExplosionDamage(Obj_AI_Base unit)
        {
            var player = UtilityClass.Player;
            return player.GetSpellDamage(unit, SpellSlot.E) +
                   player.GetSpellDamage(unit, SpellSlot.E, DamageStage.Buff);
        }

        /// <summary>
        ///     Returns true if the target is a perfectly valid charge target.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public bool IsPerfectChargeTarget(Obj_AI_Base unit)
        {
            if (IsCharged(unit) &&
                unit.IsValidSpellTarget())
            {
                switch (unit.Type)
                {
                    case GameObjectType.obj_AI_Minion:
                        return true;

                    case GameObjectType.obj_AI_Hero:
                        var heroUnit = (Obj_AI_Hero)unit;
                        return !Invulnerable.Check(heroUnit);
                }
            }

            return false;
        }

        #endregion
    }
}