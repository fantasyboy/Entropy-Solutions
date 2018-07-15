
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Caching;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Kalista
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal()
        {
            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["killsteal"].Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range)
                                                 .Where(t => GetQDamage(t) >= t.GetRealHealth(DamageType.Physical)))
                {
                    var collisions = SpellClass.Q.GetPrediction(target).CollisionObjects
                        .Where(c => Extensions.GetAllGenericMinionsTargetsInRange(SpellClass.Q.Range).Contains(c))
                        .ToList();
                    if (collisions.Any())
                    {
                        if (collisions.All(c => c.GetRealHealth(DamageType.Physical) <= GetQDamage(c)))
                        {
                            SpellClass.Q.Cast(target);
                            break;
                        }
                    }
                    else
                    {
                        SpellClass.Q.Cast(target);
                        break;
                    }
                }
            }

	        /// <summary>
	        ///     The KillSteal E Logic.
	        /// </summary>
	        if (SpellClass.E.Ready &&
	            MenuClass.Spells["e"]["killsteal"].Enabled)
	        {
		        if (ObjectCache.EnemyHeroes.Any(t =>
			                                        IsPerfectRendTarget(t) &&
			                                        t.GetRealHealth(DamageType.Physical) < GetEDamage(t)))
		        {
			        SpellClass.E.Cast();
		        }
	        }
		}

        #endregion
    }
}