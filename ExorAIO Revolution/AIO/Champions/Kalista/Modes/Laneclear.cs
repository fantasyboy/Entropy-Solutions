
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

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
        public void LaneClear()
        {
            /// <summary>
            ///     The Q Laneclear Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
					> ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["laneclear"]) &&
                MenuClass.Spells["q"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
				/*
                var farmLocation = Extensions.GetAllGenericMinionsTargets().Where(m => m.GetRealHealth(DamageType.Physical) < (float)UtilityClass.Player.GetSpellDamage(m, SpellSlot.Q)).ToList();
                if (SpellClass.Q.GetLineFarmLocation(farmLocation, SpellClass.Q.Width).MinionsHit >= 3)
                {
                    SpellClass.Q.Cast(SpellClass.Q.GetLineFarmLocation(farmLocation, SpellClass.Q.Width).Position);
                }
                */
			}

			/// <summary>
			///     The E Laneclear Logics.
			/// </summary>
			if (SpellClass.E.Ready &&
	            MenuClass.Spells["e"]["laneclear"].As<MenuSliderBool>().Enabled)
	        {
		        if (Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.E.Range).Count(m =>
			                                                                                   IsPerfectRendTarget(m) &&
			                                                                                   m.GetRealHealth(DamageType.Physical) <= GetEDamage(m)) >= MenuClass.Spells["e"]["laneclear"].As<MenuSliderBool>().Value)
		        {
			        SpellClass.E.Cast();
		        }

		        else if (Extensions.GetEnemyPetsInRange(SpellClass.E.Range).Any(m =>
			                                                                        IsPerfectRendTarget(m) &&
			                                                                        m.GetRealHealth(DamageType.Physical) <= GetEDamage(m)))
		        {
			        SpellClass.E.Cast();
		        }
	        }
		}

        #endregion
    }
}