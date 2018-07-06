
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
    internal partial class Varus
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
                (UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["harass"]) || IsChargingPiercingArrow()) &&
                MenuClass.Spells["q"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.ChargedMaxRange);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget) &&
                    MenuClass.Spells["q"]["whitelist"][bestTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                {
                    if (UtilityClass.Player.Spellbook.GetSpell(SpellSlot.W).State.HasFlag(SpellState.NotLearned) ||
                        GetBlightStacks(bestTarget) >= MenuClass.Spells["q"]["customization"]["harassstacks"].As<MenuSlider>().Value)
                    {
                        PiercingArrowLogicalCast(bestTarget);
                    }
                }
            }

            /// <summary>
            ///     The E Harass Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["harass"]) &&
                MenuClass.Spells["e"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget) &&
                    MenuClass.Spells["e"]["whitelist"][bestTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                {
                    if (UtilityClass.Player.Spellbook.GetSpell(SpellSlot.W).State.HasFlag(SpellState.NotLearned) ||
                        GetBlightStacks(bestTarget) >= MenuClass.Spells["e"]["customization"]["harassstacks"].As<MenuSlider>().Value)
                    {
                        SpellClass.E.Cast(bestTarget);
                    }
                }
            }
        }

        #endregion
    }
}