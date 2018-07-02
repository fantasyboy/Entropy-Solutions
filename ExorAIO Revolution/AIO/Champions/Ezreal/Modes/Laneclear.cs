
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
    internal partial class Ezreal
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Laneclear()
        {
            /// <summary>
            ///     The Laneclear Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["laneclear"])
                && MenuClass.Spells["q"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                var minionTarget = Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.Q.Range)
                    .FirstOrDefault(m =>
                        m.GetRealHealth() < UtilityClass.Player.GetSpellDamage(m, SpellSlot.Q) &&
                        (m.GetRealHealth() > UtilityClass.Player.GetAutoAttackDamage(m) || !m.IsValidTarget(UtilityClass.Player.GetFullAttackRange(m))));
                if (minionTarget != null)
                {
                    SpellClass.Q.Cast(minionTarget);
                }
            }
        }

        #endregion
    }
}