
using System.Linq;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using Entropy.SDK.Orbwalking;
using AIO.Utilities;

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
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PostAttackEventArgs" /> instance containing the event data.</param>
        public void Jungleclear(object sender, PostAttackEventArgs args)
        {
            var jungleTarget = ObjectManager.Get<Obj_AI_Minion>()
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
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["jungleclear"]) &&
                MenuClass.Spells["q"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                switch (UtilityClass.Player.SpellBook.GetSpell(SpellSlot.Q).ToggleState)
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
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["jungleclear"]) &&
                MenuClass.Spells["e"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                if (IsChilled(jungleTarget))
                {
                    UtilityClass.CastOnUnit(SpellClass.E, jungleTarget);
                }
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
                switch (UtilityClass.Player.SpellBook.GetSpell(SpellSlot.R).ToggleState)
                {
                    case 1:
                        SpellClass.R.Cast(jungleTarget.ServerPosition);
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