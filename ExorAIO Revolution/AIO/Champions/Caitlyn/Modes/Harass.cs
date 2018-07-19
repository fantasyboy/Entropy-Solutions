using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
	/// <summary>
	///     The champion class.
	/// </summary>
	internal partial class Caitlyn
	{
		#region Public Methods and Operators

		/// <summary>
		///     Fired when the game is updated.
		/// </summary>
		public void Harass(EntropyEventArgs args)
		{
			/// <summary>
			///     The Q Harass Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["harass"]) &&
			    MenuClass.Q["harass"].Enabled)
			{
				var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
				if (bestTarget != null &&
				    !Invulnerable.Check(bestTarget) &&
				    MenuClass.Q["whitelist"][bestTarget.CharName.ToLower()].Enabled)
				{
					SpellClass.Q.Cast(bestTarget);
				}
			}
		}

		#endregion
	}
}