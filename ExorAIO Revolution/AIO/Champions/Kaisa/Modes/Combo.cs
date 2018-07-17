using Entropy;
using AIO.Utilities;
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
        ///     Fired when the game is updated.
        /// </summary>
        public void Combo(EntropyEventArgs args)
        {
	        /// <summary>
	        ///     The E Engager Logic.
	        /// </summary>
	        if (SpellClass.E.Ready &&
	            MenuClass.Root["e"]["engage"].Enabled)
	        {
		        var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(UtilityClass.Player.GetAutoAttackRange() + 300);
		        if (bestTarget != null &&
		            !Invulnerable.Check(bestTarget) &&
		            !bestTarget.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(bestTarget)))
		        {
					SpellClass.E.Cast();
		        }
	        }

			/// <summary>
			///     The Q Combo Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
				UtilityClass.Player.EnemyHeroesCount(UtilityClass.Player.GetAutoAttackRange()) >= MenuClass.Root["q"]["combo"].Value &&
				MenuClass.Root["q"]["combo"].Enabled)
	        {
		        SpellClass.Q.Cast();
	        }
		}

        #endregion
    }
}