
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Caching;
using Entropy.SDK.Extensions.Objects;

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
        public void Automatic(EntropyEventArgs args)
        {
            if (GetBall() == null)
            {
                return;
            }

	        /// <summary>
            ///     The Automatic R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                ObjectCache.EnemyHeroes.Count() >= 2 &&
				MenuClass.R["aoe"].Enabled)
            {
                var countValidTargets = ObjectCache.EnemyHeroes.Count(t =>
                        !Invulnerable.Check(t, DamageType.Magical, false) &&
                        t.IsValidTarget(SpellClass.R.Width - SpellClass.R.Delay * t.BoundingRadius, GetBall().Position));
            
                if (countValidTargets >= MenuClass.R["aoe"].Value)
                {
                    SpellClass.R.Cast();
                }
            }
        }

        #endregion
    }
}