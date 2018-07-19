
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Caching;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Kaisa
    {
		#region Public Methods and Operators

		/// <summary>
		///     Fired on game update
		/// </summary>
		public void Laneclear(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Q Laneclear Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
				ObjectCache.EnemyLaneMinions.Count(m => m.DistanceToPlayer() < UtilityClass.Player.GetAutoAttackRange()) >= 3 &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["laneclear"]) &&
                MenuClass.Q["laneclear"].Enabled)
            {
	            SpellClass.Q.Cast();
            }

	        /// <summary>
	        ///     The E Laneclear Logic.
	        /// </summary>
	        if (SpellClass.E.Ready &&
	            ObjectCache.EnemyLaneMinions.Count(m => m.DistanceToPlayer() < UtilityClass.Player.GetAutoAttackRange()) >= 3 &&
				UtilityClass.Player.MPPercent()
					> ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.E["laneclear"]) &&
	            MenuClass.E["laneclear"].Enabled)
	        {
		        SpellClass.E.Cast();
	        }
		}

        #endregion
    }
}