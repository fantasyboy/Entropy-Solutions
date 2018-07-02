
#pragma warning disable 1587

namespace AIO.Champions
{
    using Entropy;

    using Entropy.SDK.Extensions;
    using Utilities;

    using Entropy.SDK.Menu.Components;

    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Ezreal
    {
        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Combo()
        {
            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.W.Range-150f);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical) &&
                    UtilityClass.Player.TotalAbilityDamage >= GetMinimumApForApMode())
                {
                    SpellClass.W.Cast(bestTarget);
                }
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range-100f);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget) &&
                    !bestTarget.IsValidTarget(UtilityClass.Player.GetFullAttackRange(bestTarget)))
                {
                    SpellClass.Q.Cast(bestTarget);
                }
            }
        }
    }
}