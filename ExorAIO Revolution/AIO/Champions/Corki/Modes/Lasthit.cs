
using System.Linq;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587
namespace AIO.Champions
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Corki
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Lasthit()
        {
            /// <summary>
            ///     The LastHit R Out of AA Range Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.R.Slot, MenuClass.Spells["r"]["lasthitaa"]) &&
                MenuClass.Spells["r"]["lasthitaa"].As<MenuSliderBool>().Enabled)
            {
                var bestMinionTarget = Extensions.GetEnemyLaneMinionsTargets().FirstOrDefault(m => m.GetRealHealth() < GetMissileDamage(m));
                if (bestMinionTarget != null &&
                    bestMinionTarget.IsValidTarget(SpellClass.R.Range) &&
                    !bestMinionTarget.IsValidTarget(UtilityClass.Player.GetFullAttackRange(bestMinionTarget)))
                {
                    SpellClass.R.Cast(bestMinionTarget);
                }
            }
        }

        #endregion
    }
}