
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    using Entropy;
    using Utilities;

	/// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Ezreal
    {
        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Combo(EntropyEventArgs args)
        {
            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.W["combo"].Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.W.Range-150f);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical) &&
                    UtilityClass.Player.CharIntermediate.TotalAbilityDamage() >= GetMinimumApForApMode())
                {
                    SpellClass.W.Cast(bestTarget);
                }
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Q["combo"].Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range-100f);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget) &&
                    !bestTarget.IsValidTargetEx(UtilityClass.Player.GetAutoAttackRange(bestTarget)))
                {
                    SpellClass.Q.Cast(bestTarget);
                }
            }
        }
    }
}