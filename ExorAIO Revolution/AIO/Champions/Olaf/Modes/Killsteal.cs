
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
    internal partial class Olaf
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
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.Q) >= t.GetRealHealth()))
                {
                    SpellClass.Q.Cast(target);
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
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.E) >= t.GetRealHealth()))
                {
                    SpellClass.E.CastOnUnit(target);
                    break;
                }
            }
        }

        #endregion
    }
}