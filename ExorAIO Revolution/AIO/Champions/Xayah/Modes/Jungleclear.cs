
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Xayah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
        public void Jungleclear(OnPreAttackEventArgs args)
        {
            var jungleTarget = args.Target as AIMinionClient;
            if (jungleTarget == null ||
                !Extensions.GetGenericJungleMinionsTargets().Contains(jungleTarget) ||
                jungleTarget.GetRealHealth() < UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 2)
            {
                return;
            }

            /// <summary>
            ///     The Jungleclear Q Logics.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["jungleclear"]) &&
                MenuClass.Spells["q"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.Q.Cast(jungleTarget);
            }

            /// <summary>
            ///     The Jungleclear W Logics.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["jungleclear"]) &&
                MenuClass.Spells["w"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.W.Cast();
            }
        }

        /// <summary>
        ///     Fired as fast as possible.
        /// </summary>
        public void BladeCallerJungleClear(args)
        {
            /// <summary>
            ///     The Jungleclear E Logics.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["jungleclear"]) &&
                MenuClass.Spells["e"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                if (Extensions.GetGenericJungleMinionsTargets().Any(h =>
                        IsPerfectFeatherTarget(h) &&
                        h.GetRealHealth() < GetPerfectFeatherDamage(h, CountFeathersHitOnUnit(h))))
                {
                    SpellClass.E.Cast();
                }
            }
        }

        #endregion
    }
}