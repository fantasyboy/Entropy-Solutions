
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Caching;
using SharpDX;

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
        public void Combo(EntropyEventArgs args)
        {
            if (GetBall() == null)
            {
                return;
            }

            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.W["combo"].Enabled)
            {
                if (ObjectCache.EnemyHeroes.Any(t =>
                        !Invulnerable.Check(t, DamageType.Magical, false) &&
                        t.IsValidTargetEx(SpellClass.W.Width - SpellClass.W.Delay * t.BoundingRadius, checkRangeFrom: GetBall().Position)))
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
                if (MenuClass.E["engager"].Enabled &&
                    ObjectCache.EnemyHeroes.Count() >= 2)
                {
					var bestAllies = ObjectCache.AllyHeroes
                        .Where(a =>
                            !a.IsMe() &&
                            a.IsValidTargetEx(SpellClass.E.Range, true) &&
                            MenuClass.E["engagerswhitelist"][a.CharName.ToLower()].Enabled);

                    var bestAlly = bestAllies
                        .FirstOrDefault(a =>
                            ObjectCache.EnemyHeroes.Count(t =>
                                !Invulnerable.Check(t, DamageType.Magical, false) &&
                                t.IsValidTargetEx(SpellClass.R.Width - SpellClass.R.Delay * t.BoundingRadius, checkRangeFrom: a.Position)) >= MenuClass.R["aoe"].Value);

                    if (bestAlly != null)
                    {
                        SpellClass.E.CastOnUnit(bestAlly);
                    }
                }

                /// <summary>
                ///     The E Combo Logic.
                /// </summary>
                if (MenuClass.E["combo"].Enabled)
                {
                    var bestAllies = ObjectCache.AllyHeroes
                        .Where(a =>
                            a.IsValidTargetEx(SpellClass.E.Range, true) &&
                            MenuClass.E["combowhitelist"][a.CharName.ToLower()].Enabled)
                        .OrderBy(o => o.GetRealHealth());

                    foreach (var ally in bestAllies)
                    {
                        if (ObjectCache.EnemyHeroes.Any(t =>
                                t.IsValidTargetEx() &&
                                !Invulnerable.Check(t, DamageType.Magical) &&
                                ERectangle(ally).IsInsidePolygon((Vector2)t.Position)))
                        {
                            SpellClass.E.CastOnUnit(ally);
                            return;
                        }
                    }
                }
            }

            /// <summary>
            ///     The Combo Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Q["combo"].Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (bestTarget != null)
                {
                    if (SpellClass.E.Ready &&
                        !UtilityClass.Player.HasBuff("orianaghostself") &&
                        bestTarget.Distance(GetBall().Position) >= bestTarget.DistanceToPlayer() &&
                        MenuClass.E2["gaine"].Enabled)
                    {
                        SpellClass.E.CastOnUnit(UtilityClass.Player);
                        return;
                    }

                    SpellClass.Q.GetPredictionInput(bestTarget).From = GetBall().Position;
                    SpellClass.Q.Cast(SpellClass.Q.GetPrediction(bestTarget).CastPosition);
                }
            }
        }

        #endregion
    }
}