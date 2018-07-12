
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression

using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

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
        public void Combo(EntropyEventArgs args)
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
                    SpellClass.Q.CastOnUnit(heroTarget);
                }
            }

            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
            {
                var heroTarget = Extensions.GetBestEnemyHeroTargetInRange(UtilityClass.Player.GetAutoAttackRange() + SpellClass.W.Range);
                if (heroTarget != null &&
                    !Invulnerable.Check(heroTarget))
                {
                    SpellClass.W.Cast(UtilityClass.Player.Position.Extend(heroTarget.Position, SpellClass.W.Range));
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
                else if (!heroTarget.IsValidTarget(UtilityClass.Player.GetAutoAttackRange() + SpellClass.W.Range))
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

                if (heroTarget..Position.IsUnderEnemyTurret() &&
                    MenuClass.Spells["r"]["customization"]["safe"].As<MenuBool>().Enabled)
                {
                    return;
                }

                if (heroTarget.IsValidTarget(SpellClass.R.Range) &&
                    MenuClass.Spells["r"]["combo"].As<MenuBool>().Enabled)
                {
                    if (heroTarget.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(heroTarget)) &&
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

                    if (MenuClass.Spells["r"]["whitelist"][heroTarget.CharName.ToLower()].Enabled)
                    {
                        SpellClass.R.CastOnUnit(heroTarget);
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
                        .FirstOrDefault(t => MenuClass.Spells["r"]["whitelist"][t.CharName.ToLower()].Enabled);
                    if (bestEnemy != null)
                    {
                        var bestMinion = Extensions.GetAllGenericMinionsTargetsInRange(SpellClass.R.Range)
                            .Where(m => m.Distance(bestEnemy) < SpellClass.Q.Range)
                            .MinBy(m => m.Distance(heroTarget));

                        if (bestMinion != null)
                        {
                            SpellClass.R.CastOnUnit(bestMinion);
                        }
                    }
                }
            }
        }

        #endregion
    }
}