
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Evelynn
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal()
        {
            /// <summary>
            ///     The E KillSteal Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.E.Range).Where(t =>
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.E, IsWhiplashEmpowered() ? DamageStage.Empowered : DamageStage.Default) >= t.GetRealHealth()))
                {
                    UtilityClass.CastOnUnit(SpellClass.E, target);
                    break;
                }
            }

            /// <summary>
            ///     The R KillSteal Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.R.Range).Where(t =>
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.R, t.HealthPercent() < 30 ? DamageStage.Empowered : DamageStage.Default) >= t.GetRealHealth()))
                {
                    SpellClass.R.Cast(target.ServerPosition);
                    break;
                }
            }
        }

        #endregion
    }
}