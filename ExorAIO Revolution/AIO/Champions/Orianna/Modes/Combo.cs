
using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Orianna
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Combo()
        {
            if (BallPosition == null)
            {
                return;
            }

            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
            {
                if (GameObjects.EnemyHeroes.Any(t =>
                        !Invulnerable.Check(t, DamageType.Magical, false) &&
                        t.IsValidTarget(SpellClass.W.Width - t.BoundingRadius - SpellClass.W.Delay * t.BoundingRadius, false, false, (Vector3)BallPosition)))
                {
                    SpellClass.W.Cast();
                }
            }

            /// <summary>
            ///     The E Logics.
            /// </summary>
            if (SpellClass.E.Ready)
            {
                /// <summary>
                ///     The E Engager Logic.
                /// </summary>
                if (MenuClass.Spells["r"]["aoe"] != null &&
                    MenuClass.Spells["r"]["aoe"].As<MenuSliderBool>().Enabled &&
                    MenuClass.Spells["e"]["engager"].As<MenuBool>().Enabled)
                {
                    var bestAllies = GameObjects.AllyHeroes
                        .Where(a =>
                            !a.IsMe &&
                            a.IsValidTarget(SpellClass.E.Range, true) &&
                            MenuClass.Spells["e"]["engagerswhitelist"][a.ChampionName.ToLower()].As<MenuBool>().Enabled);

                    var bestAlly = bestAllies
                        .FirstOrDefault(a =>
                            GameObjects.EnemyHeroes.Count(t =>
                                !Invulnerable.Check(t, DamageType.Magical, false) &&
                                t.IsValidTarget(SpellClass.R.Width - t.BoundingRadius - SpellClass.R.Delay * t.BoundingRadius, false, false, a.ServerPosition)) >= MenuClass.Spells["r"]["aoe"].As<MenuSliderBool>().Value);

                    if (bestAlly != null)
                    {
                        UtilityClass.CastOnUnit(SpellClass.E, bestAlly);
                    }
                }

                /// <summary>
                ///     The E Combo Logic.
                /// </summary>
                if (MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
                {
                    var bestAllies = GameObjects.AllyHeroes
                        .Where(a =>
                            a.IsValidTarget(SpellClass.E.Range, true) &&
                            MenuClass.Spells["e"]["combowhitelist"][a.ChampionName.ToLower()].As<MenuBool>().Enabled)
                        .OrderBy(o => o.GetRealHealth());

                    foreach (var ally in bestAllies)
                    {
                        var allyToBallRectangle = new Vector2Geometry.Rectangle(
                            (Vector2)ally.ServerPosition,
                            (Vector2)ally.ServerPosition.Extend((Vector3)BallPosition, ally.Distance((Vector3)BallPosition) + 30f),
                            SpellClass.E.Width);

                        if (GameObjects.EnemyHeroes.Any(t =>
                                t.IsValidTarget() &&
                                !Invulnerable.Check(t, DamageType.Magical) &&
                                allyToBallRectangle.IsInside((Vector2)t.ServerPosition)))
                        {
                            UtilityClass.CastOnUnit(SpellClass.E, ally);
                            return;
                        }
                    }
                }
            }

            /// <summary>
            ///     The Combo Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (bestTarget != null)
                {
                    if (SpellClass.E.Ready &&
                        !UtilityClass.Player.HasBuff("orianaghostself") &&
                        bestTarget.Distance((Vector3)BallPosition) >= bestTarget.Distance(UtilityClass.Player) &&
                        MenuClass.E2["gaine"].As<MenuBool>().Enabled)
                    {
                        UtilityClass.CastOnUnit(SpellClass.E, UtilityClass.Player);
                        return;
                    }

                    SpellClass.Q.GetPredictionInput(bestTarget).From = (Vector3)BallPosition;
                    SpellClass.Q.Cast(SpellClass.Q.GetPrediction(bestTarget).CastPosition);
                }
            }
        }

        #endregion
    }
}