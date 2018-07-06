
using Entropy;
using Entropy.SDK.Extensions;
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
        public void Combo(EntropyEventArgs args)
        {
            /// <summary>
            ///     The E Combo Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
            {
                var heroTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (heroTarget != null)
                {
                    if (!Invulnerable.Check(heroTarget) &&
                        !heroTarget.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(heroTarget)))
                    {
                        if (UtilityClass.Player.Spellbook.GetSpell(SpellSlot.W).State.HasFlag(SpellState.NotLearned) ||
                            GetBlightStacks(heroTarget) >= MenuClass.Spells["e"]["customization"]["combostacks"].As<MenuSlider>().Value)
                        {
                            SpellClass.E.Cast(heroTarget);
                        }
                    }
                }
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready)
            {
                var heroTarget = Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.Q.ChargedMaxRange).MaxBy(GetBlightStacks);
                if (heroTarget != null)
                {
                    if (IsChargingPiercingArrow() &&
                        !heroTarget.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(heroTarget)))
                    {
                        if (MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
                        {
                            SpellClass.Q.Cast(heroTarget);
                        }
                    }
                    else
                    {
                        if (!Invulnerable.Check(heroTarget) &&
                            MenuClass.Spells["q"]["combolong"].As<MenuBool>().Enabled &&
                            MenuClass.Spells["q"]["whitelist"][heroTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                        {
                            PiercingArrowLogicalCast(heroTarget);
                        }
                    }
                }
            }
        }

        #endregion
    }
}