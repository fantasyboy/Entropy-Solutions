
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Anivia
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Harass()
        {
            /// <summary>
            ///     The R Harass Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var manaCheck =
                    UtilityClass.Player.ManaPercent() >
                    ManaManager.GetNeededMana(SpellClass.R.Slot, MenuClass.Spells["r"]["harass"]);
                switch (UtilityClass.Player.SpellBook.GetSpell(SpellSlot.R).ToggleState)
                {
                    case 1 when GlacialStorm == null:
                        var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.R.Range);
                        if (bestTarget != null)
                        {
                            if (manaCheck &&
                                !Invulnerable.Check(bestTarget, DamageType.Magical) &&
                                MenuClass.Spells["r"]["whitelist"][bestTarget.ChampionName.ToLower()].As<MenuBool>().Enabled)
                            {
                                SpellClass.R.Cast(bestTarget);
                            }
                        }
                        break;
                    case 2 when GlacialStorm != null:
                        if (!manaCheck ||
                            !GameObjects.EnemyHeroes.Any(t =>
                                !Invulnerable.Check(t, DamageType.Magical) &&
                                t.IsValidTarget(SpellClass.R.Width, checkRangeFrom: GlacialStorm.Position) &&
                                MenuClass.Spells["r"]["whitelist"][t.ChampionName.ToLower()].As<MenuBool>().Enabled))
                        {
                            SpellClass.R.Cast();
                        }
                        break;
                }
            }

            /// <summary>
            ///     The Q Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var manaCheck =
                    UtilityClass.Player.ManaPercent() >
                    ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["harass"]);
                switch (UtilityClass.Player.SpellBook.GetSpell(SpellSlot.Q).ToggleState)
                {
                    case 1 when FlashFrost == null:
                        var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                        if (bestTarget != null)
                        {
                            if (manaCheck &&
                                !Invulnerable.Check(bestTarget, DamageType.Magical) &&
                                MenuClass.Spells["q"]["whitelist"][bestTarget.ChampionName.ToLower()].As<MenuBool>().Enabled)
                            {
                                SpellClass.Q.Cast(bestTarget);
                            }
                        }
                        break;
                    case 2 when FlashFrost != null:
                        if (!GameObjects.EnemyHeroes.Any(t =>
                                !Invulnerable.Check(t, DamageType.Magical) &&
                                t.IsValidTarget(SpellClass.Q.Width, checkRangeFrom: FlashFrost.Position) &&
                                MenuClass.Spells["q"]["whitelist"][t.ChampionName.ToLower()].As<MenuBool>().Enabled))
                        {
                            SpellClass.Q.Cast();
                        }
                        break;
                }
            }

            /// <summary>
            ///     The E Harass Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["harass"]) &&
                MenuClass.Spells["e"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical) &&
                    MenuClass.Spells["e"]["whitelist"][bestTarget.ChampionName.ToLower()].As<MenuBool>().Enabled)
                {
                    switch (MenuClass.Spells["e"]["modes"]["harass"].As<MenuList>().Value)
                    {
                        case 0 when IsChilled(bestTarget):
                            UtilityClass.CastOnUnit(SpellClass.E, bestTarget);
                            break;
                        case 1:
                            UtilityClass.CastOnUnit(SpellClass.E, bestTarget);
                            break;
                    }
                }
            }
        }

        #endregion
    }
}