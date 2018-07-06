
using System.Linq;
using Entropy;
using AIO.Utilities;
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
    internal partial class Anivia
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
        public void Jungleclear(OnPostAttackEventArgs args)
        {
            var jungleTarget = ObjectManager.Get<AIMinionClient>()
                .Where(m => Extensions.GetGenericJungleMinionsTargets().Contains(m))
                .MinBy(m => m.Distance(UtilityClass.Player));
            if (jungleTarget == null ||
                jungleTarget.GetRealHealth() < UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 3)
            {
                return;
            }

            /// <summary>
            ///     The Q Jungleclear Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                jungleTarget.IsValidTarget(SpellClass.Q.Range) &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["jungleclear"]) &&
                MenuClass.Spells["q"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                switch (UtilityClass.Player.Spellbook.GetSpell(SpellSlot.Q).ToggleState)
                {
                    case 1:
                        SpellClass.Q.Cast(jungleTarget);
                        break;
                    case 2:
                        if (FlashFrost != null &&
                            Extensions.GetGenericJungleMinionsTargets().Any(m =>
                                m.IsValidTarget(SpellClass.Q.Width, false, true, FlashFrost.Position)))
                        {
                            SpellClass.Q.Cast();
                        }
                        break;
                }
            }

            /// <summary>
            ///     The E Jungleclear Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                jungleTarget.IsValidTarget(SpellClass.E.Range) &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["jungleclear"]) &&
                MenuClass.Spells["e"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                if (IsChilled(jungleTarget))
                {
                    SpellClass.E.CastOnUnit(jungleTarget);
                }
            }

            /// <summary>
            ///     The R Jungleclear Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                jungleTarget.IsValidTarget(SpellClass.R.Range) &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.R.Slot, MenuClass.Spells["r"]["jungleclear"]) &&
                MenuClass.Spells["r"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                switch (UtilityClass.Player.Spellbook.GetSpell(SpellSlot.R).ToggleState)
                {
                    case 1:
                        SpellClass.R.Cast(jungleTarget.Position);
                        break;
                    case 2:
                        if (UtilityClass.Player.InFountain())
                        {
                            return;
                        }

                        if (GlacialStorm != null &&
                            !Extensions.GetGenericJungleMinionsTargets().Any(m =>
                                m.IsValidTarget(SpellClass.R.Width, false, true, GlacialStorm.Position)))
                        {
                            SpellClass.R.Cast();
                        }
                        break;
                }
            }
        }

        #endregion
    }
}