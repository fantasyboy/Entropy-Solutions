﻿
using System.Linq;
using AIO.Utilities;
using Entropy.SDK.Caching;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions;
using Entropy.SDK.Extensions.Geometry;
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
        public void JungleClear()
        {
            var jungleTarget = ObjectCache.EnemyMinions
                .Where(m => Extensions.GetGenericJungleMinionsTargets().Contains(m))
                .MinBy(m => m.DistanceToPlayer());
            if (jungleTarget == null ||
                jungleTarget.GetRealHealth() < UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 3)
            {
                return;
            }

            /// <summary>
            ///     The Q Jungleclear Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                jungleTarget.IsValidTarget(SpellClass.Q.Range) &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["jungleclear"]) &&
                MenuClass.Spells["q"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.Q.Cast(jungleTarget);
            }

	        /// <summary>
	        ///     The E Jungleclear Logic.
	        /// </summary>
	        if (SpellClass.E.Ready &&
	            UtilityClass.Player.Level() >=
	            MenuClass.Spells["e"]["junglesteal"].As<MenuSliderBool>().Value &&
	            MenuClass.Spells["e"]["junglesteal"].As<MenuSliderBool>().Enabled)
	        {
		        foreach (var minion in Extensions.GetGenericJungleMinionsTargets().Where(m =>
			                                                                                 IsPerfectRendTarget(m) &&
			                                                                                 m.GetRealHealth() <= GetEDamage(m)))
		        {
			        if (UtilityClass.JungleList.Contains(minion.CharName) &&
			            MenuClass.Spells["e"]["whitelist"][minion.CharName].As<MenuBool>().Enabled)
			        {
				        SpellClass.E.Cast();
			        }
			        else if (!UtilityClass.JungleList.Contains(minion.CharName) &&
			                 MenuClass.General["junglesmall"].As<MenuBool>().Enabled)
			        {
				        SpellClass.E.Cast();
			        }
		        }
	        }
		}

        #endregion
    }
}