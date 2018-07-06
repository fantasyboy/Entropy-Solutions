
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Tristana
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Killsteal(args)
        {
            /// <summary>
            ///     The R KillSteal Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.R.Range).Where(t =>
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.R) + (IsCharged(t) ? GetTotalExplosionDamage(t) : 0) >= t.GetRealHealth()))
                {
                    SpellClass.R.CastOnUnit(target);
                    break;
                }
            }
        }

        #endregion
    }
}