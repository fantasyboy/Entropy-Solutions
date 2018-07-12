
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

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
        public void Combo(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q2.Range+60f);
            if (SpellClass.Q.Ready &&
                bestTarget != null &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                var minEnemies = MenuClass.Spells["q"]["customization"]["minenemies"];
                var bestTargetDistanceToPlayer = UtilityClass.Player.Distance(bestTarget);
                var enemiesInSplashRange = bestTarget.EnemyHeroesCount(SplashRange);

                if (!IsUsingFishBones())
                {
                    if (minEnemies != null &&
                        enemiesInSplashRange >= minEnemies.As<MenuSlider>().Value)
                    {
                        SpellClass.Q.Cast();
                    }

                    else if (bestTargetDistanceToPlayer > SpellClass.Q.Range + bestTarget.BoundingRadius)
                    {
                        SpellClass.Q.Cast();
                    }
                }
                else
                {
                    if (minEnemies != null &&
                        enemiesInSplashRange < minEnemies.As<MenuSlider>().Value &&
                        bestTargetDistanceToPlayer <= SpellClass.Q.Range + bestTarget.BoundingRadius)
                    {
                        SpellClass.Q.Cast();
                    }
                }
            }

            /// <summary>
            ///     The E AoE Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["aoe"] != null &&
                MenuClass.Spells["e"]["aoe"].As<MenuSliderBool>().Enabled)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(h => !Invulnerable.Check(h) && h.IsValidTarget(SpellClass.E.Range)))
                {
                    if (GameObjects.EnemyHeroes.Count(h2 =>
                            !Invulnerable.Check(h2) &&
                            h2.IsValidTarget(SpellClass.E.Range) &&
                            h2.Distance(target) < SpellClass.E.Width*2) >= MenuClass.Spells["e"]["aoe"].As<MenuSliderBool>().Value)
                    {
                        SpellClass.E.Cast(target);
                    }
                }
            }

            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                !UtilityClass.Player.Position.IsUnderEnemyTurret())
            {
                if (UtilityClass.Player.EnemyHeroesCount(SpellClass.Q2.Range) >
                        MenuClass.Spells["w"]["customization"]["wsafetycheck"].As<MenuSliderBool>().Value &&
                    MenuClass.Spells["w"]["customization"]["wsafetycheck"].As<MenuSliderBool>().Enabled)
                {
                    return;
                }

                var target = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.W.Range);
                if (target != null &&
                    !Invulnerable.Check(target))
                {
                    var targetDistanceToPlayer = target.DistanceToPlayer();
                    switch (MenuClass.Spells["w"]["mode"].As<MenuList>().Value)
                    {
                        case 0:
                            if (targetDistanceToPlayer >= SpellClass.Q.Range * 1.1)
                            {
                                SpellClass.W.Cast(target);
                            }
                            break;
                        case 1:
                            if (targetDistanceToPlayer >= SpellClass.Q2.Range * 1.1)
                            {
                                SpellClass.W.Cast(target);
                            }
                            break;
                    }
                }
            }
        }

        #endregion
    }
}