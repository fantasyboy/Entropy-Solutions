using AIO.Utilities;
using Entropy;
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
        public void Harass(EntropyEventArgs args)
        {
	        /// <summary>
	        ///     The Q Combo Logic.
	        /// </summary>
	        if (SpellClass.Q.Ready &&
				UtilityClass.Player.EnemyHeroesCount(UtilityClass.Player.GetAutoAttackRange()) > 0 &&
	            UtilityClass.Player.MPPercent()
					> ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["harass"]) &&
	            MenuClass.Spells["q"]["harass"].Enabled)
			{
		        SpellClass.Q.Cast();
	        }

			/// <summary>
			///     The W Harass Logic.
			/// </summary>
			if (SpellClass.W.Ready &&
	            UtilityClass.Player.MPPercent()
					> ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["harass"]) &&
	            MenuClass.Spells["w"]["harass"].Enabled)
	        {
		        var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.W.Range / 2);
		        if (bestTarget != null &&
		            !Invulnerable.Check(bestTarget) &&
		            MenuClass.Spells["w"]["whitelist"][bestTarget.CharName.ToLower()].Enabled)
		        {
			        SpellClass.W.Cast(bestTarget);
		        }
			}
		}

        #endregion
    }
}