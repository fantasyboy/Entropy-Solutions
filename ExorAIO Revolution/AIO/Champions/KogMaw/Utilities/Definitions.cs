
using System;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class KogMaw
    {
        #region Fields

        /// <summary>
        ///     Returns true if the player is using BioArcaneBarrage, else, false;
        /// </summary>
        public bool IsUsingBioArcaneBarrage()
        {
            return UtilityClass.Player.HasBuff("KogMawBioArcaneBarrage");
        }

        /// <summary>
        ///     Gets the real Damage the R spell would deal to a determined enemy hero.
        /// </summary>
        /// <param name="target">The target.</param>
        public double GetTotalLivingArtilleryDamage(AIHeroClient target)
        {
            var missingHealth = Math.Min(60, 100 / target.HPPercent());
            var damage = UtilityClass.Player.GetSpellDamage(target, SpellSlot.R) * (1 + 0.833 * missingHealth / 100);

            return damage * (target.HPPercent() < 40 ? 2 : 1);
        }

        #endregion
    }
}