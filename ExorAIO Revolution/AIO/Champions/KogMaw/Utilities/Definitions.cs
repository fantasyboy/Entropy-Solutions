
using System;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions;
using AIO.Utilities;

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
        public double GetTotalLivingArtilleryDamage(Obj_AI_Hero target)
        {
            var missingHealth = Math.Min(60, 100 / target.HealthPercent());
            var damage = UtilityClass.Player.GetSpellDamage(target, SpellSlot.R) * (1 + 0.833 * missingHealth / 100);

            return damage * (target.HealthPercent() < 40 ? 2 : 1);
        }

        #endregion
    }
}