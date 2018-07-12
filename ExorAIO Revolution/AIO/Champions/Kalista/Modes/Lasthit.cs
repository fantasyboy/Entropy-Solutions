using System.Linq;
using AIO.Utilities;
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
        public void LastHit()
        {
	        /// <summary>
	        ///     The E Lasthit Logics.
	        /// </summary>
	        if (SpellClass.E.Ready &&
	            Extensions.GetEnemyLaneMinionsTargets().Any(m =>
		                                                        IsPerfectRendTarget(m) &&
		                                                        m.GetRealHealth() <= GetEDamage(m)) &&
	            MenuClass.Spells["e"]["lasthit"].As<MenuBool>().Enabled)
	        {
		        SpellClass.E.Cast();
	        }
		}

        #endregion
    }
}