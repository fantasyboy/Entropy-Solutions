// ReSharper disable ArrangeMethodOrOperatorBody


using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Quinn
    {
        #region Fields

        /// <summary>
        ///     Returns the time when the last R cast has been issued.
        /// </summary>
        public double LastSpellCastTime = 0;

        /// <summary>
        ///     Returns true if it can cast R, else, false.
        /// </summary>
        public bool CanCastBehindEnemyLines()
        {
            return Game.ClockTime - LastSpellCastTime >= 5;
        }

        /// <summary>
        ///     Returns true if it's in R stance, else, false.
        /// </summary>
        public bool IsUsingBehindEnemyLines()
        {
            return UtilityClass.Player.HasBuff("QuinnR");
        }

        /// <summary>
        ///     Returns true if the target is marked by the passive, else, false.
        /// </summary>
        /// <param name="target">The target.</param>
        public bool IsVulnerable(Obj_AI_Base target)
        {
            return target.HasBuff("QuinnW");
        }

        #endregion
    }
}