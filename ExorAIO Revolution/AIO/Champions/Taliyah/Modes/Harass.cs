using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
	/// <summary>
	///     The logics class.
	/// </summary>
	internal partial class Taliyah
	{
		#region Public Methods and Operators

		/// <summary>
		///     Called on tick update.
		/// </summary>
		public void Harass(EntropyEventArgs args)
		{
			/// <summary>
			///     The Q Harass Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    (IsNearWorkedGround() ||
			     UtilityClass.Player.MPPercent()
			     > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["harass"])) &&
			    MenuClass.Q["harass"].Enabled)
			{
				var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range - 100f);
				if (bestTarget != null &&
				    !Invulnerable.Check(bestTarget, DamageType.Magical) &&
				    MenuClass.Q["whitelist"][bestTarget.CharName.ToLower()].Enabled)
				{
					switch (MenuClass.Q["modes"]["harass"].Value)
					{
						case 0:
							if (!IsNearWorkedGround())
							{
								SpellClass.Q.Cast(bestTarget);
							}

							break;
						case 1:
							SpellClass.Q.Cast(bestTarget);
							break;
					}
				}
			}
		}

		#endregion
	}
}