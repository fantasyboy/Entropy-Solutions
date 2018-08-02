
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
	/// <summary>
	///     The champion class.
	/// </summary>
	internal partial class Ezreal
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal(EntropyEventArgs args)
        {
            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Q["killsteal"].Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range).Where(t =>
                    !t.IsValidTargetEx(UtilityClass.Player.GetAutoAttackRange(t)) &&
                    GetQDamage(t) >= t.GetRealHealth()))
                {
                    SpellClass.Q.Cast(target);
                    break;
                }
            }

            /// <summary>
            ///     The KillSteal W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.W["killsteal"].Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.W.Range).Where(t =>
                    !t.IsValidTargetEx(UtilityClass.Player.GetAutoAttackRange(t)) &&
					GetWDamage(t) >= t.GetRealHealth()))
                {
                    SpellClass.W.Cast(target);
                    break;
                }
            }
        }

        #endregion
    }
}