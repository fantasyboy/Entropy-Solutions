
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Corki
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Harass()
        {
            /// <summary>
            ///     The Q Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["harass"]) &&
                MenuClass.Spells["q"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical) &&
                    MenuClass.Spells["q"]["whitelist"][bestTarget.ChampionName.ToLower()].As<MenuBool>().Enabled)
                {
                    SpellClass.Q.Cast(bestTarget);
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
                    SpellClass.E.Cast();
                }
            }

            /// <summary>
            ///     The R Harass Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.R.Slot, MenuClass.Spells["r"]["harass"]) &&
                MenuClass.Spells["r"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.R.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical) &&
                    MenuClass.Spells["r"]["whitelist"][bestTarget.ChampionName.ToLower()].As<MenuBool>().Enabled)
                {
                    SpellClass.R.Cast(bestTarget);
                }
            }
        }

        #endregion
    }
}