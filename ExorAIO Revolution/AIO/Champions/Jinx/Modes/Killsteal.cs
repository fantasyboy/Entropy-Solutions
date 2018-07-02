
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
    internal partial class Jinx
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal()
        {
            /// <summary>
            ///     The KillSteal W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                !UtilityClass.Player.IsUnderEnemyTurret() &&
                UtilityClass.Player.CountEnemyHeroesInRange(SpellClass.Q.Range) < 3 &&
                MenuClass.Spells["w"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.W.Range).Where(t =>
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.W) >= t.GetRealHealth()))
                {
                    SpellClass.W.Cast(target);
                    break;
                }
            }
        }

        #endregion
    }
}