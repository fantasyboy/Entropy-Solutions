
using System.Linq;
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
    internal partial class Anivia
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Harass(EntropyEventArgs args)
        {
            /// <summary>
            ///     The R Harass Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var manaCheck =
                    UtilityClass.Player.MPPercent() >
                    ManaManager.GetNeededMana(SpellClass.R.Slot, MenuClass.Spells["r"]["harass"]);
                switch (UtilityClass.Player.Spellbook.GetSpell(SpellSlot.R).ToggleState)
                {
                    case 1 when GlacialStorm == null:
                        var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.R.Range);
                        if (bestTarget != null)
                        {
                            if (manaCheck &&
                                !Invulnerable.Check(bestTarget, DamageType.Magical) &&
                                MenuClass.Spells["r"]["whitelist"][bestTarget.CharName.ToLower()].As<MenuBool>().Enabled)
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
                                MenuClass.Spells["r"]["whitelist"][t.CharName.ToLower()].As<MenuBool>().Enabled))
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
                    UtilityClass.Player.MPPercent() >
                    ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["harass"]);
                switch (UtilityClass.Player.Spellbook.GetSpell(SpellSlot.Q).ToggleState)
                {
                    case 1 when FlashFrost == null:
                        var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                        if (bestTarget != null)
                        {
                            if (manaCheck &&
                                !Invulnerable.Check(bestTarget, DamageType.Magical) &&
                                MenuClass.Spells["q"]["whitelist"][bestTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                            {
                                SpellClass.Q.Cast(bestTarget);
                            }
                        }
                        break;
                    case 2 when FlashFrost != null:
                        if (!GameObjects.EnemyHeroes.Any(t =>
                                !Invulnerable.Check(t, DamageType.Magical) &&
                                t.IsValidTarget(SpellClass.Q.Width, checkRangeFrom: FlashFrost.Position) &&
                                MenuClass.Spells["q"]["whitelist"][t.CharName.ToLower()].As<MenuBool>().Enabled))
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
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["harass"]) &&
                MenuClass.Spells["e"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical) &&
                    MenuClass.Spells["e"]["whitelist"][bestTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                {
                    switch (MenuClass.Spells["e"]["modes"]["harass"].As<MenuList>().Value)
                    {
                        case 0 when IsChilled(bestTarget):
                            SpellClass.E.CastOnUnit(bestTarget);
                            break;
                        case 1:
                            SpellClass.E.CastOnUnit(bestTarget);
                            break;
                    }
                }
            }
        }

        #endregion
    }
}