
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Jinx
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
            var bestTargetQ = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q2.Range+60f);
            if (SpellClass.Q.Ready &&
                bestTargetQ != null &&
                MenuClass.Spells["q"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var manaPercent = UtilityClass.Player.ManaPercent();
                var minEnemies = MenuClass.Spells["q"]["customization"]["minenemies"];
                var bestTargetDistanceToPlayer = UtilityClass.Player.Distance(bestTargetQ);
                var harassManaManager = MenuClass.Spells["q"]["harass"].As<MenuSliderBool>().Value;
                var enemiesInSplashRange = bestTargetQ.CountEnemyHeroesInRange(SplashRange);

                if (!IsUsingFishBones())
                {
                    if (manaPercent <= harassManaManager)
                    {
                        return;
                    }

                    if (minEnemies != null &&
                        enemiesInSplashRange >= minEnemies.As<MenuSlider>().Value)
                    {
                        SpellClass.Q.Cast();
                    }

                    if (bestTargetDistanceToPlayer > SpellClass.Q.Range + bestTargetQ.BoundingRadius)
                    {
                        SpellClass.Q.Cast();
                    }
                }
                else
                {
                    if (manaPercent <= harassManaManager)
                    {
                        SpellClass.Q.Cast();
                        return;
                    }

                    if (minEnemies != null &&
                        enemiesInSplashRange < minEnemies.As<MenuSlider>().Value &&
                        bestTargetDistanceToPlayer <= SpellClass.Q.Range + bestTargetQ.BoundingRadius)
                    {
                        SpellClass.Q.Cast();
                    }
                }
            }

            /// <summary>
            ///     The W Harass Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["harass"]) &&
                MenuClass.Spells["w"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.W.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget) &&
                    MenuClass.Spells["w"]["whitelist"][bestTarget.ChampionName.ToLower()].As<MenuBool>().Enabled)
                {
                    SpellClass.W.Cast(bestTarget);
                }
            }
        }

        #endregion
    }
}