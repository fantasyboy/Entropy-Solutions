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
    internal partial class Corki
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the total missile damage on a determined unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public double GetMissileDamage(AIBaseClient unit)
        {
            var player = UtilityClass.Player;
            return HasBigOne()
                       ? player.GetSpellDamage(unit, SpellSlot.R, DamageStage.SecondForm)
                       : player.GetSpellDamage(unit, SpellSlot.R);
        }

        /// <summary>
        ///     returns true if the player has the BigOne, else false.
        /// </summary>
        public bool HasBigOne()
        {
            return UtilityClass.Player.HasBuff("corkimissilebarragecounterbig");
        }

        /// <summary>
        ///     returns true if the player has the Package, else false.
        /// </summary>
        public bool HasPackage()
        {
            return UtilityClass.Player.HasBuff("corkiloaded");
        }

        #endregion
    }
}