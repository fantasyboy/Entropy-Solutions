
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Caching;

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
        public void Harass(EntropyEventArgs args)
        {
            if (GetBall() == null)
            {
                return;
            }

            /// <summary>
            ///     The Harass Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["harass"]) &&
                MenuClass.Q["harass"].Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (bestTarget.IsValidTarget() &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical) &&
                    MenuClass.Q["whitelist"][bestTarget.CharName.ToLower()].Enabled)
                {
                    SpellClass.Q.GetPredictionInput(bestTarget).From = GetBall().Position;
                    SpellClass.Q.Cast(SpellClass.Q.GetPrediction(bestTarget).CastPosition);
                }
            }

            /// <summary>
            ///     The Harass W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.W["harass"]) &&
                MenuClass.W["harass"].Enabled)
            {
                if (ObjectCache.EnemyHeroes.Any(t =>
                        !Invulnerable.Check(t, DamageType.Magical, false) &&
                        t.IsValidTarget(SpellClass.W.Width - SpellClass.W.Delay * t.BoundingRadius, GetBall().Position) &&
                        MenuClass.W["whitelist"][t.CharName.ToLower()].Enabled))
                {
                    SpellClass.W.Cast();
                }
            }
        }

        #endregion
    }
}