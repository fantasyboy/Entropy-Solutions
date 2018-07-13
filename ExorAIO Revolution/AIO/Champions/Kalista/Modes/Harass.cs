
using System.Collections.Generic;
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Caching;
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
        public void Harass()
        {
            /// <summary>
            ///     The Q Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["harass"]) &&
                MenuClass.Spells["q"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (bestTarget != null &&
                    MenuClass.Spells["q"]["whitelist"][bestTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                {
                    var collisions = SpellClass.Q.GetPrediction(bestTarget).CollisionObjects
                        .Where(c => Extensions.GetAllGenericUnitTargets().Contains(c));
                    var objAiBases = collisions as IList<AIBaseClient> ?? collisions.ToList();
                    if (objAiBases.Any())
                    {
                        if (objAiBases.All(c => c.GetRealHealth() <= GetQDamage(c)))
                        {
                            SpellClass.Q.Cast(bestTarget);
                        }
                    }
                    else
                    {
                        SpellClass.Q.Cast(bestTarget);
                    }
                }
            }

	        /// <summary>
	        ///     The E Minion Harass Logic.
	        /// </summary>
	        if (SpellClass.E.Ready &&
	            Extensions.GetEnemyLaneMinionsTargets().Any(m =>
		                                                        IsPerfectRendTarget(m) &&
		                                                        m.GetRealHealth() <= GetEDamage(m)) &&
	            MenuClass.Spells["e"]["harass"].As<MenuBool>().Enabled)
	        {
		        if (ObjectCache.EnemyHeroes.Where(IsPerfectRendTarget)
		                       .Any(enemy => !enemy.HasBuffOfType(BuffType.Slow) || !MenuClass.Spells["e"]["dontharassslowed"].As<MenuBool>().Enabled))
		        {
			        SpellClass.E.Cast();
		        }
	        }
		}

        #endregion
    }
}