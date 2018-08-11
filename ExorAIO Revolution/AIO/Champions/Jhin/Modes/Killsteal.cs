
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.UI.Components;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Jhin
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal(EntropyEventArgs args)
        {
            /// <summary>
            ///     The KillSteal R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                IsUltimateShooting() &&
                MenuClass.R["killsteal"].As<MenuBool>().Value)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.R2.Range)
					.Where(t =>
						UltimateCone().IsInsidePolygon(t.Position) &&
						GetRDamage(t, HasUltimateFourthShot()) >= t.GetRealHealth(DamageType.Physical)))
                {
                    SpellClass.R2.Cast(target);
                    break;
                }
            }

            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Q["killsteal"].As<MenuBool>().Value)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range)
					.Where(t => GetQDamage(t) >= t.GetRealHealth(DamageType.Physical)))
                {
                    SpellClass.Q.CastOnUnit(target);
                    break;
                }
            }

            /// <summary>
            ///     The KillSteal W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.W["killsteal"].As<MenuBool>().Value)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.W.Range)
					.Where(t => GetWDamage(t) >= t.GetRealHealth(DamageType.Physical)))
                {
                    SpellClass.W.Cast(target);
                    break;
                }
            }
        }

        #endregion
    }
}