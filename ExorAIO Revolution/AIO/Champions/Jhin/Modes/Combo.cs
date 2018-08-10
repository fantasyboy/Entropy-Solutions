
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;
using Entropy.SDK.Caching;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Jhin
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Combo(EntropyEventArgs args)
        {
            /// <summary>
            ///     The R Shooting Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                IsUltimateShooting() &&
                MenuClass.R["combo"].As<MenuBool>().Value)
            {
                var validEnemiesInsideCone = Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.R2.Range)
                    .Where(t => t.IsValidTarget() && !Invulnerable.Check(t) && UltimateCone.IsInsidePolygon(t.Position))
                    .ToList();
                if (validEnemiesInsideCone.Any())
                {
                    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                    if (MenuClass.R["customization"]["nearmouse"].As<MenuBool>().Value)
                    {
                        var target = validEnemiesInsideCone.MinBy(o => o.Distance(Hud.CursorPositionUnclipped));
                        if (target != null)
                        {
                            SpellClass.R2.Cast(target);
                        }
                    }
                    else
                    {
                        var target = validEnemiesInsideCone.FirstOrDefault();
                        if (target != null)
                        {
                            SpellClass.R2.Cast(target);
                        }
                    }
                }
                else
                {
                    SpellClass.R2.Cast(Hud.CursorPositionUnclipped);
                }
            }

            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                !UtilityClass.Player.Position.IsUnderEnemyTurret() &&
                MenuClass.W["combo"].As<MenuBool>().Value)
            {
                if (!IsReloading() &&
                    ObjectCache.EnemyHeroes.Any(t => t.DistanceToPlayer() < UtilityClass.Player.GetAutoAttackRange(t)) &&
                    MenuClass.W["customization"]["noenemiesaa"].As<MenuBool>().Value)
                {
                    return;
                }

                foreach (var target in ObjectCache.EnemyHeroes.Where(t =>
                    t.HasBuff("jhinespotteddebuff") &&
                    t.IsValidTarget(SpellClass.W.Range - 100f) &&
                    !Invulnerable.Check(t, DamageType.Magical, false) &&
                    MenuClass.W["whitelist"][t.CharName.ToLower()].As<MenuBool>().Value))
                {
                    if (MenuClass.W["customization"]["onlyslowed"].As<MenuBool>().Value)
                    {
                        if (target.HasBuffOfType(BuffType.Slow))
                        {
                            SpellClass.W.Cast(target);
                        }
                    }
                    else
                    {
                        SpellClass.W.Cast(target);
                    }
                }
            }

            /// <summary>
            ///     The Q Combo on Reload Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                IsReloading() &&
                MenuClass.Q["customization"]["comboonreload"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (bestTarget.IsValidTarget() &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical, false))
                {
                    SpellClass.Q.CastOnUnit(bestTarget);
                }
            }

            /// <summary>
            ///     The E Combo Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.E["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (bestTarget.IsValidTarget() &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical, false))
                {
                    if (MenuClass.E["customization"]["comboonreload"].As<MenuBool>().Enabled)
                    {
                        if (IsReloading())
                        {
                            SpellClass.E.Cast(bestTarget);
                        }
                    }
                    else
                    {
                        SpellClass.E.Cast(bestTarget);
                    }
                }
            }
        }

        #endregion
    }
}