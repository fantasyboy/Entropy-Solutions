
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Darius
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
                    GetTotalNoxianGuillotineDamage(t) >= t.GetRealHealth()))
                {
                    UtilityClass.CastOnUnit(SpellClass.R, target);
                    break;
                }
            }

            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Enabled)
            {
                if (Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range).Any(t =>
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.Q, IsValidBladeTarget(t) ? DamageStage.Empowered : DamageStage.Default) >= t.GetRealHealth()))
                {
                    SpellClass.Q.Cast();
                }
            }
        }

        #endregion
    }
}