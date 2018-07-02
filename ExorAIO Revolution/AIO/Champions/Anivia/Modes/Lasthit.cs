
using System.Linq;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587
namespace AIO.Champions
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Anivia
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Lasthit()
        {
            /// <summary>
            ///     The LastHit E Out of AA Range Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["lasthitaa"]) &&
                MenuClass.Spells["e"]["lasthitaa"].As<MenuSliderBool>().Enabled)
            {
                var bestMinionTarget = Extensions.GetEnemyLaneMinionsTargets().FirstOrDefault(m => m.GetRealHealth() <= GetFrostBiteDamage(m));
                if (bestMinionTarget != null &&
                    bestMinionTarget.IsValidTarget(SpellClass.E.Range) &&
                    !bestMinionTarget.IsValidTarget(UtilityClass.Player.GetFullAttackRange(bestMinionTarget)))
                {
                    SpellClass.E.Cast(bestMinionTarget);
                }
            }
        }

        #endregion
    }
}