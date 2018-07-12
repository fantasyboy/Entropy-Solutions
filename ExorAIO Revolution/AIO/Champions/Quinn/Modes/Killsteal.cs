
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Quinn
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
                !IsUsingBehindEnemyLines() &&
                MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range).Where(t =>
                    !t.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(t)) &&
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.Q) + UtilityClass.Player.GetAutoAttackDamage(t) >= t.GetRealHealth()))
                {
                    SpellClass.Q.Cast(target);
                    break;
                }
            }

            /// <summary>
            ///     The KillSteal E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.E.Range).Where(t =>
                    !t.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(t)) &&
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.E) + UtilityClass.Player.GetAutoAttackDamage(t) >= t.GetRealHealth()))
                {
                    SpellClass.E.CastOnUnit(target);
                    break;
                }
            }
        }

        #endregion
    }
}