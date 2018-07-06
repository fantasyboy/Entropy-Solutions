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
    internal partial class Ezreal
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
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["harass"]) &&
                MenuClass.Spells["q"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range-100f);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget) &&
                    MenuClass.Spells["q"]["whitelist"][bestTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                {
                    SpellClass.Q.Cast(bestTarget);
                }
            }

            /// <summary>
            ///     The W Harass Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["harass"]) &&
                MenuClass.Spells["w"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.W.Range-150f);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget) &&
                    MenuClass.Spells["w"]["whitelist"][bestTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                {
                    SpellClass.W.Cast(bestTarget);
                }
            }
        }

        #endregion
    }
}