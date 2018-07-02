// ReSharper disable ArrangeMethodOrOperatorBody

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
        public static void PiercingArrowLogicalCast(Obj_AI_Base target)
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
        public static double GetRealPiercingArrowDamage(Obj_AI_Base target)
        {
            return
                UtilityClass.Player.GetSpellDamage(target, SpellSlot.Q) +
                UtilityClass.Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.Buff);
        }

        /// <summary>
        ///     Returns the real E Damage;
        /// </summary>
        /// <param name="target">The target.</param>
        public static double GetRealHailOfArrowsDamage(Obj_AI_Base target)
        {
            return
                UtilityClass.Player.GetSpellDamage(target, SpellSlot.E) +
                UtilityClass.Player.GetSpellDamage(target, SpellSlot.E, DamageStage.Buff);
        }

        /// <summary>
        ///     Returns the real R Damage;
        /// </summary>
        /// <param name="target">The target.</param>
        public static double GetRealChainOfCorruptionDamage(Obj_AI_Base target)
        {
            return
                UtilityClass.Player.GetSpellDamage(target, SpellSlot.R) +
                UtilityClass.Player.GetSpellDamage(target, SpellSlot.R, DamageStage.Buff);
        }

        /// <summary>
        ///     Returns the number of Blight Stacks a determined target has;
        /// </summary>
        /// <param name="target">The target.</param>
        public int GetBlightStacks(Obj_AI_Base target)
        {
            return target.GetRealBuffCount("VarusWDebuff");
        }

        #endregion
    }
}