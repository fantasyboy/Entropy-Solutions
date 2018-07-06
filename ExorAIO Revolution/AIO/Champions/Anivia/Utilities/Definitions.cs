// ReSharper disable ArrangeMethodOrOperatorBody


using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Anivia
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the FlashFrost Object.
        /// </summary>
        public GameObject FlashFrost = null;

        /// <summary>
        ///     Gets the GlacialStorm Object.
        /// </summary>
        public GameObject GlacialStorm = null;

        /// <summary>
        ///     Gets the total missile damage on a determined unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public double GetFrostBiteDamage(AIBaseClient unit)
        {
            var player = UtilityClass.Player;
            return IsChilled(unit)
                       ? player.GetSpellDamage(unit, SpellSlot.E, DamageStage.Empowered)
                       : player.GetSpellDamage(unit, SpellSlot.E);
        }

        /// <summary>
        ///     Returns true if the target is marked by Q or R, else false.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public bool IsChilled(AIBaseClient unit)
        {
            return unit.HasBuff("chilled");
        }

        #endregion
    }
}