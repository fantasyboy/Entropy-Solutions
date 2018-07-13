
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
    internal partial class Ezreal
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void LaneClear(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Laneclear Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["laneclear"])
                && MenuClass.Spells["q"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                var minionTarget = Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.Q.Range)
                    .FirstOrDefault(m =>
                        m.GetRealHealth() < UtilityClass.Player.GetSpellDamage(m, SpellSlot.Q) &&
                        (m.GetRealHealth() > UtilityClass.Player.GetAutoAttackDamage(m) || !m.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(m))));
                if (minionTarget != null)
                {
                    SpellClass.Q.Cast(minionTarget);
                }
            }
        }

        #endregion
    }
}