
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Akali
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal()
        {
            /// <summary>
            ///     The KillSteal R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.R.Range).Where(t =>
                    !t.IsValidTarget(UtilityClass.Player.GetFullAttackRange(t)) &&
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.R)*2 >= t.GetRealHealth()))
                {
                    UtilityClass.CastOnUnit(SpellClass.R, target);
                    break;
                }
            }

            /// <summary>
            ///     The KillSteal E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["killsteal"].As<MenuBool>().Enabled)
            {
                if (Extensions.GetBestSortedTargetsInRange(SpellClass.E.Range).Any(t => UtilityClass.Player.GetSpellDamage(t, SpellSlot.E) >= t.GetRealHealth()))
                {
                    SpellClass.E.Cast();
                }
            }

            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range).Where(t =>
                    !t.IsValidTarget(UtilityClass.Player.GetFullAttackRange(t)) &&
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.Q) >= t.GetRealHealth()))
                {
                    UtilityClass.CastOnUnit(SpellClass.Q, target);
                    break;
                }
            }
        }

        #endregion
    }
}