
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
    internal partial class Varus
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Q KillSteal Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range).Where(t =>
                    GetRealPiercingArrowDamage(t) >= t.GetRealHealth() &&
                    !t.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(t))))
                {
                    if (IsChargingPiercingArrow())
                    {
                        SpellClass.Q.Cast(target);
                    }
                    else
                    {
                        if (SpellClass.W.Ready &&
                            MenuClass.Spells["w"]["killsteal"].As<MenuBool>().Enabled)
                        {
                            SpellClass.W.Cast();
                        }

                        PiercingArrowLogicalCast(target);
                    }
                    break;
                }
            }

            /// <summary>
            ///     The E KillSteal Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.E.Range).Where(t =>
                    GetRealHailOfArrowsDamage(t) >= t.GetRealHealth() &&
                    !t.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(t))))
                {
                    SpellClass.E.Cast(target);
                    break;
                }
            }

            /// <summary>
            ///     The R KillSteal Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.R.Range).Where(t =>
                    GetRealChainOfCorruptionDamage(t) >= t.GetRealHealth() &&
                    !t.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(t))))
                {
                    SpellClass.R.Cast(target);
                    break;
                }
            }
        }

        #endregion
    }
}