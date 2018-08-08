using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Jhin
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Harass(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Q Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["harass"]) &&
                MenuClass.Q["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (!bestTarget.IsValidTargetEx() ||
                    Invulnerable.Check(bestTarget) ||
                    !MenuClass.Q["whitelist"][bestTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                {
                    return;
                }

                SpellClass.Q.CastOnUnit(bestTarget);
            }
        }

        #endregion
    }
}