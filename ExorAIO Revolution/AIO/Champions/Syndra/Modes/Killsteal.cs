
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Syndra
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal(args)
        {
            /// <summary>
            ///     The Q KillSteal Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range).Where(t =>
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.Q) >= t.GetRealHealth()))
                {
                    SpellClass.Q.Cast(SpellClass.Q.GetPrediction(target).CastPosition);
                    break;
                }
            }

            /// <summary>
            ///     The W KillSteal Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.W.Range).Where(t =>
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.W) >= t.GetRealHealth()))
                {
                    if (IsHoldingForceOfWillObject())
                    {
                        SpellClass.W.Cast(UtilityClass.Player.Position.Extend(SpellClass.W.GetPrediction(target).CastPosition, target.BoundingRadius));
                    }
                }
            }

            /// <summary>
            ///     The R KillSteal Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.R.Range).Where(t =>
                    GetTotalUnleashedPowerDamage(t) >= t.GetRealHealth()))
                {
                    SpellClass.R.CastOnUnit(target);
                    break;
                }
            }
        }

        #endregion
    }
}