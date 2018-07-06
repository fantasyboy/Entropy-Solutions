
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Quinn
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(args)
        {
            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The Automatic R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                CanCastBehindEnemyLines() &&
                !IsUsingBehindEnemyLines() &&
                UtilityClass.Player.InFountain() &&
                UtilityClass.Player.CountEnemyHeroesInRange(1500f) == 0 &&
                MenuClass.Spells["r"]["logical"].As<MenuBool>().Value)
            {
                SpellClass.R.Cast();
                LastSpellCastTime = Game.ClockTime;
            }
        }

        #endregion
    }
}