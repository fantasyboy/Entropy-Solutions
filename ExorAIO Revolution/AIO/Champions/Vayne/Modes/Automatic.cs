
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Vayne
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Semi-Automatic E Management.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.E["bool"].Enabled &&
                MenuClass.E["key"].Enabled)
            {
                var bestTarget = GameObjects.EnemyHeroes.Where(t =>
                        t.IsValidTarget(SpellClass.E.Range) &&
                        !Invulnerable.Check(t, DamageType.Magical, false))
                    .MinBy(o => o.DistanceToPlayer());
                if (bestTarget != null)
                {
                    SpellClass.E.CastOnUnit(bestTarget);
                }
            }
        }

        #endregion
    }
}