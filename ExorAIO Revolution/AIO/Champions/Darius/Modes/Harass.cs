
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Darius
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Harass(EntropyEventArgs args)
        {
            /// <summary>
            ///     The E Combo Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["harass"]) &&
                MenuClass.Spells["e"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var heroTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (heroTarget != null &&
                    !Invulnerable.Check(heroTarget, DamageType.Magical, false) &&
                    MenuClass.Spells["e"]["whitelist"][heroTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                {
                    SpellClass.E.Cast(heroTarget);
                }
            }

            /// <summary>
            ///     The Q Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["harass"]) &&
                MenuClass.Spells["q"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var heroTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (heroTarget != null &&
                    !Invulnerable.Check(heroTarget) &&
                    MenuClass.Spells["q"]["whitelist"][heroTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                {
                    switch (MenuClass.Spells["q"]["modes"]["harass"].As<MenuList>().Value)
                    {
                        case 0:
                            if (IsValidBladeTarget(heroTarget))
                            {
                                SpellClass.Q.Cast();
                            }
                            break;
                        case 1:
                            SpellClass.Q.Cast();
                            break;
                    }
                }
            }
        }

        /// <summary>
        ///     Called OnPostAttack.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
        public void Harass(OnPostAttackEventArgs args)
        {
            var heroTarget = args.Target as AIHeroClient;
            if (heroTarget == null || Invulnerable.Check(heroTarget))
            {
                return;
            }

            /// <summary>
            ///     The W Harass Weaving Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["harass"]) &&
                MenuClass.Spells["w"]["harass"].As<MenuSliderBool>().Enabled)
            {
                if (MenuClass.Spells["w"]["whitelist"][heroTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                {
                    SpellClass.W.Cast();
                }
            }
        }

        #endregion
    }
}