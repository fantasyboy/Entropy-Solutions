
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

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
        public void LastHit(EntropyEventArgs args)
        {
            /// <summary>
            ///     The LastHit R Out of AA Range Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.R.Slot, MenuClass.Spells["r"]["lasthitaa"]) &&
                MenuClass.Spells["r"]["lasthitaa"].As<MenuSliderBool>().Enabled)
            {
                var bestMinionTarget = Extensions.GetEnemyLaneMinionsTargets().FirstOrDefault(m => m.GetRealHealth() < GetMissileDamage(m));
                if (bestMinionTarget != null &&
                    bestMinionTarget.IsValidTarget(SpellClass.R.Range) &&
                    !bestMinionTarget.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(bestMinionTarget)))
                {
                    SpellClass.R.Cast(bestMinionTarget);
                }
            }
        }

        #endregion
    }
}