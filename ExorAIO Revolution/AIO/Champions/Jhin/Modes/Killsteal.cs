
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
    internal partial class Jhin
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
                IsUltimateShooting() &&
                MenuClass.Spells["r"]["killsteal"].As<MenuBool>().Value)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.R.Range).Where(t =>
                    UltimateCone().IsInside((Vector2)t.ServerPosition) &&
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.R, HasUltimateFourthShot() ? DamageStage.Empowered : DamageStage.Default) >= t.GetRealHealth()))
                {
                    SpellClass.R.Cast(target);
                    break;
                }
            }

            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Value)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range).Where(t =>
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.Q) >= t.GetRealHealth()))
                {
                    UtilityClass.CastOnUnit(SpellClass.Q, target);
                    break;
                }
            }

            /// <summary>
            ///     The KillSteal W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["killsteal"].As<MenuBool>().Value)
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