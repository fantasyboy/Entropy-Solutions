
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Corki
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PostAttackEventArgs" /> instance containing the event data.</param>
        public void Jungleclear(object sender, PostAttackEventArgs args)
        {
            var jungleTarget = args.Target as Obj_AI_Minion;
            if (jungleTarget == null ||
                !Extensions.GetGenericJungleMinionsTargets().Contains(jungleTarget) ||
                jungleTarget.GetRealHealth() < UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 2)
            {
                return;
            }

            /// <summary>
            ///     The Q Jungleclear Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                jungleTarget.IsValidTarget(SpellClass.Q.Range) &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["jungleclear"]) &&
                MenuClass.Spells["q"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.Q.Cast(jungleTarget);
                return;
            }

            /// <summary>
            ///     The E Jungleclear Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                jungleTarget.IsValidTarget(SpellClass.E.Range) &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["jungleclear"]) &&
                MenuClass.Spells["e"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.E.Cast();
                return;
            }

            /// <summary>
            ///     The R Jungleclear Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                jungleTarget.IsValidTarget(SpellClass.R.Range) &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.R.Slot, MenuClass.Spells["r"]["jungleclear"]) &&
                MenuClass.Spells["r"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.R.Cast(jungleTarget);
            }
        }

        #endregion
    }
}