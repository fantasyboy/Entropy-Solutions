
using Entropy;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Evelynn
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Combo()
        {
            /// <summary>
            ///     The R AoE Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["aoe"] != null &&
                MenuClass.Spells["r"]["aoe"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.R.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical))
                {
                    var positionAfterR = UtilityClass.Player.ServerPosition.Extend(SpellClass.R.GetPrediction(bestTarget).CastPosition, -LastCaressPushBackDistance());
                    if (MenuClass.Spells["r"]["customization"]["aoecheck"] != null &&
                        positionAfterR.CountEnemyHeroesInRange(MenuClass.Spells["r"]["customization"]["safetyrange"].As<MenuSlider>().Value) >= MenuClass.Spells["r"]["customization"]["aoecheck"].As<MenuSlider>().Value)
                    {
                        return;
                    }

                    if (positionAfterR.PointUnderEnemyTurret() &&
                        MenuClass.Spells["r"]["customization"]["turretcheck"].As<MenuBool>().Enabled)
                    {
                        return;
                    }

                    SpellClass.R.CastIfWillHit(bestTarget, MenuClass.Spells["r"]["aoe"].As<MenuSliderBool>().Value);
                }
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(GetRealQRange());
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical))
                {
                    if (IsAllured(bestTarget))
                    {
                        if (IsFullyAllured(bestTarget) || !MenuClass.Spells["q"]["onlyiffullyallured"].As<MenuBool>().Enabled)
                        {
                            if (IsHateSpikeSkillshot())
                            {
                                SpellClass.Q.Cast(bestTarget);
                            }
                            else
                            {
                                UtilityClass.CastOnUnit(SpellClass.Q, bestTarget);
                            }
                        }
                    }
                    else
                    {
                        SpellClass.Q.Cast(bestTarget);
                    }
                }
            }

            /// <summary>
            ///     The E Combo Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical))
                {
                    if (IsAllured(bestTarget))
                    {
                        if (IsFullyAllured(bestTarget) || !MenuClass.Spells["e"]["onlyiffullyallured"].As<MenuBool>().Enabled)
                        {
                            UtilityClass.CastOnUnit(SpellClass.E, bestTarget);
                        }
                    }
                    else
                    {
                        UtilityClass.CastOnUnit(SpellClass.E, bestTarget);
                    }
                }
            }
        }

        #endregion
    }
}