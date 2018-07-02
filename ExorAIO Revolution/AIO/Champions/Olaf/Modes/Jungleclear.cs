
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
    internal partial class Olaf
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PreAttackEventArgs" /> instance containing the event data.</param>
        public void Jungleclear(object sender, PreAttackEventArgs args)
        {
            var jungleTarget = args.Target as Obj_AI_Minion;
            if (jungleTarget == null ||
                !Extensions.GetGenericJungleMinionsTargets().Contains(jungleTarget) ||
                jungleTarget.GetRealHealth() < UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 2)
            {
                return;
            }

            /// <summary>
            ///     The Jungleclear Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["jungleclear"]) &&
                MenuClass.Spells["q"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.Q.Cast(SpellClass.Q.GetPrediction(jungleTarget).CastPosition.Extend(UtilityClass.Player.ServerPosition, -50f - jungleTarget.BoundingRadius));
            }

            /// <summary>
            ///     The Jungleclear E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                if (UtilityClass.Player.HealthPercent()
                        >= MenuClass.Spells["e"]["jungleclear"].As<MenuSliderBool>().Value ||
                    UtilityClass.Player.GetSpellDamage(jungleTarget, SpellSlot.E) >= jungleTarget.GetRealHealth())
                {
                    UtilityClass.CastOnUnit(SpellClass.E, jungleTarget);
                }
            }

            /// <summary>
            ///     The Jungleclear W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["jungleclear"]) &&
                MenuClass.Spells["w"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.W.Cast();
            }
        }

        #endregion
    }
}