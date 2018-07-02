
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression

using System.Linq;
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
    internal partial class Akali
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Combo()
        {
            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                var heroTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (heroTarget != null &&
                    !heroTarget.HasBuff("AkaliMota") &&
                    !Invulnerable.Check(heroTarget, DamageType.Magical))
                {
                    UtilityClass.CastOnUnit(SpellClass.Q, heroTarget);
                }
            }

            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
            {
                var heroTarget = Extensions.GetBestEnemyHeroTargetInRange(UtilityClass.Player.AttackRange + SpellClass.W.Range);
                if (heroTarget != null &&
                    !Invulnerable.Check(heroTarget))
                {
                    SpellClass.W.Cast(UtilityClass.Player.ServerPosition.Extend(heroTarget.ServerPosition, SpellClass.W.Range));
                }
            }

            /// <summary>
            ///     The R Combo Logic.
            /// </summary>
            if (SpellClass.R.Ready)
            {
                var heroTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.R.Range);
                if ((!SpellClass.W.Ready || !MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled) &&
                    Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.R.Range * 2).Any(t => t.HasBuff("AkaliMota")))
                {
                    heroTarget = Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.R.Range).FirstOrDefault(t => t.HasBuff("AkaliMota"));
                }
                else if (!heroTarget.IsValidTarget(UtilityClass.Player.AttackRange + SpellClass.W.Range))
                {
                    heroTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.R.Range * 2);
                }
                else
                {
                    heroTarget = null;
                }

                if (heroTarget == null ||
                    !heroTarget.IsValidTarget() ||
                    Invulnerable.Check(heroTarget, DamageType.Magical))
                {
                    return;
                }

                if (heroTarget.IsUnderEnemyTurret() &&
                    MenuClass.Spells["r"]["customization"]["safe"].As<MenuBool>().Enabled)
                {
                    return;
                }

                if (heroTarget.IsValidTarget(SpellClass.R.Range) &&
                    MenuClass.Spells["r"]["combo"].As<MenuBool>().Enabled)
                {
                    if (heroTarget.IsValidTarget(UtilityClass.Player.GetFullAttackRange(heroTarget)) &&
                        MenuClass.Spells["r"]["customization"]["noraarange"].As<MenuBool>().Enabled)
                    {
                        return;
                    }

                    if (!heroTarget.HasBuff("AkaliMota") &&
                        MenuClass.Spells["r"]["customization"]["onlymarked"].As<MenuBool>().Enabled)
                    {
                        return;
                    }

                    if (UtilityClass.Player.GetRealBuffCount("AkaliShadowDance")
                            <= MenuClass.Spells["r"]["customization"]["keepstacks"].As<MenuSlider>().Value)
                    {
                        return;
                    }

                    if (MenuClass.Spells["r"]["whitelist"][heroTarget.ChampionName.ToLower()].Enabled)
                    {
                        UtilityClass.CastOnUnit(SpellClass.R, heroTarget);
                    }
                }
                else
                {
                    if (UtilityClass.Player.GetRealBuffCount("AkaliShadowDance")
                            <= MenuClass.Spells["r"]["customization"]["gapclose"].As<MenuSliderBool>().Value ||
                        !MenuClass.Spells["r"]["customization"]["gapclose"].As<MenuSliderBool>().Enabled)
                    {
                        return;
                    }

                    var bestEnemy = Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.R.Range * 2)
                        .FirstOrDefault(t => MenuClass.Spells["r"]["whitelist"][t.ChampionName.ToLower()].Enabled);
                    if (bestEnemy != null)
                    {
                        var bestMinion = Extensions.GetAllGenericMinionsTargetsInRange(SpellClass.R.Range)
                            .Where(m => m.Distance(bestEnemy) < SpellClass.Q.Range)
                            .MinBy(m => m.Distance(heroTarget));

                        if (bestMinion != null)
                        {
                            UtilityClass.CastOnUnit(SpellClass.R, bestMinion);
                        }
                    }
                }
            }
        }

        #endregion
    }
}