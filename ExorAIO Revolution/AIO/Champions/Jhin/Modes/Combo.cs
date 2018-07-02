
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
    internal partial class Jhin
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Combo()
        {
            /// <summary>
            ///     The R Shooting Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                IsUltimateShooting() &&
                MenuClass.Spells["r"]["combo"].As<MenuBool>().Value)
            {
                var validEnemiesInsideCone = Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.R.Range)
                    .Where(t => t.IsValidTarget() && !Invulnerable.Check(t) && UltimateCone().IsInside((Vector2)t.ServerPosition))
                    .ToList();
                if (validEnemiesInsideCone.Any())
                {
                    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                    if (MenuClass.Spells["r"]["customization"]["nearmouse"].As<MenuBool>().Value)
                    {
                        var target = validEnemiesInsideCone.MinBy(o => o.Distance(Game.CursorPos));
                        if (target != null)
                        {
                            SpellClass.R.Cast(target);
                        }
                    }
                    else
                    {
                        var target = validEnemiesInsideCone.FirstOrDefault();
                        if (target != null)
                        {
                            SpellClass.R.Cast(validEnemiesInsideCone.FirstOrDefault());
                        }
                    }
                }
                else
                {
                    SpellClass.R.Cast(Game.CursorPos);
                }
            }

            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                !UtilityClass.Player.IsUnderEnemyTurret() &&
                MenuClass.Spells["w"]["combo"].As<MenuBool>().Value)
            {
                if (!IsReloading() &&
                    GameObjects.EnemyHeroes.Any(t => t.Distance(UtilityClass.Player) < UtilityClass.Player.GetFullAttackRange(t)) &&
                    MenuClass.Spells["w"]["customization"]["noenemiesaa"].As<MenuBool>().Value)
                {
                    return;
                }

                foreach (var target in GameObjects.EnemyHeroes.Where(t =>
                    t.HasBuff("jhinespotteddebuff") &&
                    t.IsValidTarget(SpellClass.W.Range - 100f) &&
                    !Invulnerable.Check(t, DamageType.Magical, false) &&
                    MenuClass.Spells["w"]["whitelist"][t.ChampionName.ToLower()].As<MenuBool>().Value))
                {
                    if (MenuClass.Spells["w"]["customization"]["onlyslowed"].As<MenuBool>().Value)
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
                MenuClass.Spells["q"]["customization"]["comboonreload"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (bestTarget.IsValidTarget() &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical, false))
                {
                    UtilityClass.CastOnUnit(SpellClass.Q, bestTarget);
                }
            }

            /// <summary>
            ///     The E Combo Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (bestTarget.IsValidTarget() &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical, false))
                {
                    if (MenuClass.Spells["e"]["customization"]["comboonreload"].As<MenuBool>().Enabled)
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