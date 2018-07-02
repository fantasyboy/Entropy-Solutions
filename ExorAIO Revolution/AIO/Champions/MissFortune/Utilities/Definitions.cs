﻿// ReSharper disable ArrangeMethodOrOperatorBody

using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class MissFortune
    {
        #region Public Methods and Operators

        /// <returns>
        ///     Returns the NetworkId of the passive target.
        /// </returns>
        public int LoveTapTargetNetworkId = 0;

        /// <returns>
        ///     Returns true if the player has the AA Fourth Shot, else false.
        /// </returns>
        public bool IsUltimateShooting()
        {
            return UtilityClass.Player.HasBuff("missfortunebulletsound");
        }

        /// <returns>
        ///     Returns the passive damage multiplier against minions.
        /// </returns>
        public double GetMinionLoveTapDamageMultiplier()
        {
            return
                UtilityClass.Player.TotalAttackDamage * (UtilityClass.Player.Level < 4
                                                                ? 0.25 :
                                                            UtilityClass.Player.Level < 7
                                                                ? 0.3 :
                                                            UtilityClass.Player.Level < 9
                                                                ? 0.35 :
                                                            UtilityClass.Player.Level < 11
                                                                ? 0.4 :
                                                            UtilityClass.Player.Level < 13
                                                                ? 0.45
                                                                : 0.5);
        }

        /// <returns>
        ///     Returns the passive damage multiplier against non-minion units.
        /// </returns>
        public float GetGenericLoveTapDamageMultiplier()
        {
            return (float)GetMinionLoveTapDamageMultiplier() * 2;
        }

        /// <returns>
        ///     Returns the passive damage multiplier against a target.
        /// </returns>
        public double GetLoveTapDamage(Obj_AI_Base target)
        {
            return LoveTapTargetNetworkId != target.NetworkId
                ? target is Obj_AI_Minion
                    ? GetMinionLoveTapDamageMultiplier()
                    : GetGenericLoveTapDamageMultiplier()
                : 0;
        }

        /// <returns>
        ///     Returns the real damage.
        /// </returns>
        public double GetRealMissFortuneDamage(double amount, Obj_AI_Base target)
        {
            return amount + UtilityClass.Player.CalculateDamage(target, DamageType.Physical, GetLoveTapDamage(target));
        }

        /// <returns>
        ///     Gets the Q Cone on a determined target unit.
        /// </returns>
        /// <param name="target">The target.</param>
        public Vector2Geometry.Sector QCone(Obj_AI_Base target)
        {
            var targetPos = target.ServerPosition;
            var range = SpellClass.Q2.Range - SpellClass.Q.Range - UtilityClass.Player.BoundingRadius;
            var dir = (targetPos - UtilityClass.Player.ServerPosition).Normalized();
            var spot = targetPos + dir * range;

            return new Vector2Geometry.Sector((Vector2)targetPos, (Vector2)spot, SpellClass.Q2.Width, range);
        }

        /// <returns>
        ///     Gets the Q Cone on a determined target unit.
        /// </returns>
        /// <param name="target">The target.</param>
        public Vector3Geometry.Sector DrawQCone(Obj_AI_Base target)
        {
            var targetPos = target.Position;
            var range = SpellClass.Q2.Range - SpellClass.Q.Range - UtilityClass.Player.BoundingRadius;
            var dir = (targetPos - UtilityClass.Player.Position).Normalized();
            var spot = targetPos + dir * range;

            return new Vector3Geometry.Sector(targetPos, spot, SpellClass.Q2.Width, range);
        }

        #endregion
    }
}