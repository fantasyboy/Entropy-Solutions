// ReSharper disable ConvertIfStatementToConditionalTernaryExpression

using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;
using SharpDX;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Taliyah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Combo(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Rylai Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.HasItem(ItemId.RylaisCrystalScepter))
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range-50f);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical))
                {
                    switch (MenuClass.Spells["q"]["modes"]["combo"].As<MenuList>().Value)
                    {
                        case 0:
                            if (!IsNearWorkedGround())
                            {
                                SpellClass.Q.Cast(bestTarget);
                            }
                            break;
                        case 1:
                            SpellClass.Q.Cast(bestTarget);
                            break;
                    }
                }
            }

            /// <summary>
            ///     The W->Boulders Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["boulders"].As<MenuBool>().Enabled)
            {
                var bestTargets = ImplementationClass.ITargetSelector.GetOrderedTargets(SpellClass.W.Range - 100f)
                    .Where(t => MenuClass.Spells["w"]["selection"][t.CharName.ToLower()].As<MenuList>().Value < 4);

                var objAiHeroes = bestTargets as AIHeroClient[] ?? bestTargets.ToArray();
                foreach (var target in objAiHeroes)
                {
                    var bestBoulderHitPos = GetBestBouldersHitPosition(target);
                    var bestBoulderHitPosHitBoulders = GetBestBouldersHitPositionHitBoulders(target);
                    if (bestBoulderHitPos != Vector3.Zero && bestBoulderHitPosHitBoulders > 0)
                    {
                        SpellClass.W.Cast(bestBoulderHitPos, SpellClass.W.GetPrediction(target).CastPosition);
                    }
                }
            }

            /// <summary>
            ///     The W-> E Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                (SpellClass.E.Ready || !MenuClass.Spells["w"]["customization"]["onlyeready"].As<MenuBool>().Enabled) &&
                MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.W.Range-100f);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical))
                {
                    switch (MenuClass.Spells["pattern"].As<MenuList>().Value)
                    {
                        case 0:
                            var targetPred = SpellClass.W.GetPrediction(bestTarget).CastPosition;
                            SpellClass.W.Cast(GetTargetPositionAfterW(bestTarget), targetPred);
                            break;
                    }
                }
            }

            /// <summary>
            ///     The E Combo Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                (!SpellClass.W.Ready || !MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled || MenuClass.Spells["pattern"].As<MenuList>().Value == 1) &&
                (SpellClass.W.Ready || !MenuClass.Spells["e"]["customization"]["onlywready"].As<MenuBool>().Enabled) &&
                MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
            {
                var bestETarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range - 100f);
                if (bestETarget != null &&
                    !Invulnerable.Check(bestETarget, DamageType.Magical))
                {
                    SpellClass.E.Cast(bestETarget.Position);
                }
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range - 150f);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical))
                {
                    switch (MenuClass.Spells["q"]["modes"]["combo"].As<MenuList>().Value)
                    {
                        case 0:
                            if (!IsNearWorkedGround())
                            {
                                SpellClass.Q.Cast(SpellClass.Q.GetPrediction(bestTarget).CastPosition);
                            }
                            break;
                        case 1:
                            SpellClass.Q.Cast(SpellClass.Q.GetPrediction(bestTarget).CastPosition);
                            break;
                    }
                }
            }
        }

        #endregion
    }
}