
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Lucian
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Combo(EntropyEventArgs args)
        {
            /// <summary>
            ///     The E Engager Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Root["e"]["engage"].Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget) &&
                    !bestTarget.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(bestTarget)))
                {
                    var posAfterE = UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, 425f);
                    if (posAfterE.EnemyHeroesCount(1000f) < 3 &&
                        UtilityClass.Player.Distance(Hud.CursorPositionUnclipped) > UtilityClass.Player.GetAutoAttackRange() &&
                        bestTarget.Distance(posAfterE) < UtilityClass.Player.GetAutoAttackRange(bestTarget))
                    {
                        SpellClass.E.Cast(posAfterE);
                    }
                }
            }
        }

        #endregion
    }
}