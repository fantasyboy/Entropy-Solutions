using Entropy;
using AIO.Utilities;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions;

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
        ///     Called on post attack.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
        public void Jungleclear(OnPostAttackEventArgs args)
        {
            var jungleTarget = args.Target as AIMinionClient;
            if (!jungleTarget.IsValidTargetEx() ||
                !Extensions.GetGenericJungleMinionsTargets().Contains(jungleTarget))
            {
                return;
            }

            /// <summary>
            ///     The Jungleclear Q Logics.
            /// </summary>
            if (SpellClass.Q.Ready &&
				!HasFourthShot() &&
				UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["jungleclear"]) &&
                MenuClass.Q["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.Q.CastOnUnit(jungleTarget);
            }
        }

        /// <summary>
        ///     Called on post attack.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
        public void Jungleclear(OnPreAttackEventArgs args)
        {
            var jungleTarget = args.Target as AIMinionClient;
            if (!HasFourthShot() ||
                !jungleTarget.IsValidTargetEx() ||
                !Extensions.GetGenericJungleMinionsTargets().Contains(jungleTarget))
            {
                return;
            }

            /// <summary>
            ///     The Jungleclear Q Logics.
            /// </summary>
            if (SpellClass.Q.Ready &&
				HasFourthShot() &&
				UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["jungleclear"]) &&
                MenuClass.Q["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.Q.CastOnUnit(jungleTarget);
            }

            /// <summary>
            ///     The Jungleclear E Logics.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.E["jungleclear"]) &&
                MenuClass.E["jungleclear"].As<MenuSliderBool>().Enabled &&
				jungleTarget?.HP > UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 4)
            {
				SpellClass.E.Cast(jungleTarget);
            }
        }

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void JungleClear(EntropyEventArgs args)
        {
			var jungleTarget = Extensions.GetGenericJungleMinionsTargetsInRange(SpellClass.Q.Range)
				.MinBy(m => m.Distance(Hud.CursorPositionUnclipped));
            if (jungleTarget == null)
            {
                return;
            }

            /// <summary>
            ///     The Jungleclear Q Logics.
            /// </summary>
            if (SpellClass.Q.Ready &&
                jungleTarget.IsValidTargetEx(SpellClass.Q.Range) &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["jungleclear"]) &&
                MenuClass.Q["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.Q.CastOnUnit(jungleTarget);
            }

            /// <summary>
            ///     The Jungleclear E Logics.
            /// </summary>
            if (SpellClass.E.Ready &&
                jungleTarget.IsValidTargetEx(SpellClass.E.Range) &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.E["jungleclear"]) &&
                MenuClass.E["jungleclear"].As<MenuSliderBool>().Enabled &&
				jungleTarget?.HP > UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 4)
            {
				SpellClass.E.Cast(jungleTarget);
            }
        }

        #endregion
    }
}