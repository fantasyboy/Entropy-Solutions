
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Sivir
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
                    !t.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(t)) &&
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.Q) +
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.Q, DamageStage.WayBack) >= t.GetRealHealth()))
                {
                    SpellClass.Q.Cast(target);
                    break;
                }
            }
        }

        #endregion
    }
}