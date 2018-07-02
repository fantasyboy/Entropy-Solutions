
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using AIO.Utilities;

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
        public void Harass()
        {
            /// <summary>
            ///     The E Combo Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["harass"]) &&
                MenuClass.Spells["e"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var heroTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (heroTarget != null &&
                    !Invulnerable.Check(heroTarget, DamageType.Magical, false) &&
                    MenuClass.Spells["e"]["whitelist"][heroTarget.ChampionName.ToLower()].As<MenuBool>().Enabled)
                {
                    SpellClass.E.Cast(heroTarget);
                }
            }

            /// <summary>
            ///     The Q Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["harass"]) &&
                MenuClass.Spells["q"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var heroTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (heroTarget != null &&
                    !Invulnerable.Check(heroTarget) &&
                    MenuClass.Spells["q"]["whitelist"][heroTarget.ChampionName.ToLower()].As<MenuBool>().Enabled)
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
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PostAttackEventArgs" /> instance containing the event data.</param>
        public void Harass(object sender, PostAttackEventArgs args)
        {
            var heroTarget = args.Target as Obj_AI_Hero;
            if (heroTarget == null || Invulnerable.Check(heroTarget))
            {
                return;
            }

            /// <summary>
            ///     The W Harass Weaving Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["harass"]) &&
                MenuClass.Spells["w"]["harass"].As<MenuSliderBool>().Enabled)
            {
                if (MenuClass.Spells["w"]["whitelist"][heroTarget.ChampionName.ToLower()].As<MenuBool>().Enabled)
                {
                    SpellClass.W.Cast();
                }
            }
        }

        #endregion
    }
}