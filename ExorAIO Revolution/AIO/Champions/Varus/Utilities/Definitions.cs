﻿// ReSharper disable ArrangeMethodOrOperatorBody

using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Varus
    {
        #region Fields

        /// <summary>
        ///     Returns the last casted Q or E time;
        /// </summary>
        public static int LastCastedWeaving = 0;

        /// <summary>
        ///     Returns Q Charging Logic;
        /// </summary>
        /// <param name="target">The target.</param>
        public static void PiercingArrowLogicalCast(AIBaseClient target)
        {
            if (!IsChargingPiercingArrow() &&
                target.IsValidTarget(SpellClass.Q.ChargedMaxRange))
            {
                SpellClass.Q.StartCharging(SpellClass.Q.GetPrediction(target).CastPosition);
            }
            else if (IsChargingPiercingArrow() &&
                target.IsValidTarget(SpellClass.Q.Range))
            {
                SpellClass.Q.Cast(target);
            }
        }

        /// <summary>
        ///     Returns true if the player is charging Q;
        /// </summary>
        public static bool IsChargingPiercingArrow()
        {
            return UtilityClass.Player.HasBuff(SpellClass.Q.ChargedBuffName);
        }

        /// <summary>
        ///     Returns the real Q Damage;
        /// </summary>
        /// <param name="target">The target.</param>
        public static double GetRealPiercingArrowDamage(AIBaseClient target)
        {
            return
                UtilityClass.Player.GetSpellDamage(target, SpellSlot.Q) +
                UtilityClass.Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.Buff);
        }

        /// <summary>
        ///     Returns the real E Damage;
        /// </summary>
        /// <param name="target">The target.</param>
        public static double GetRealHailOfArrowsDamage(AIBaseClient target)
        {
            return
                UtilityClass.Player.GetSpellDamage(target, SpellSlot.E) +
                UtilityClass.Player.GetSpellDamage(target, SpellSlot.E, DamageStage.Buff);
        }

        /// <summary>
        ///     Returns the real R Damage;
        /// </summary>
        /// <param name="target">The target.</param>
        public static double GetRealChainOfCorruptionDamage(AIBaseClient target)
        {
            return
                UtilityClass.Player.GetSpellDamage(target, SpellSlot.R) +
                UtilityClass.Player.GetSpellDamage(target, SpellSlot.R, DamageStage.Buff);
        }

        /// <summary>
        ///     Returns the number of Blight Stacks a determined target has;
        /// </summary>
        /// <param name="target">The target.</param>
        public int GetBlightStacks(AIBaseClient target)
        {
            return target.GetRealBuffCount("VarusWDebuff");
        }

        #endregion
    }
}