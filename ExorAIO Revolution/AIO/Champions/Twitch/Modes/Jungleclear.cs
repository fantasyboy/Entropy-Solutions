
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
    internal partial class Twitch
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
        public void Jungleclear(OnPostAttackEventArgs args)
        {
            var jungleTarget = args.Target as AIMinionClient;
            if (jungleTarget == null ||
                !Extensions.GetGenericJungleMinionsTargets().Contains(jungleTarget) ||
                jungleTarget.GetRealHealth() < UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 2)
            {
                return;
            }

            /// <summary>
            ///     The W Jungleclear Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.MPPercent()
                > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["jungleclear"]) &&
                MenuClass.Spells["w"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                if (jungleTarget.GetRealBuffCount("TwitchDeadlyVenom") <= 3)
                {
                    SpellClass.W.Cast(jungleTarget.Position);
                    return;
                }
            }

            /// <summary>
            ///     The Q Jungleclear Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["jungleclear"]) &&
                MenuClass.Spells["q"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.Q.Cast();
            }
        }

        /// <summary>
        ///     Fired as fast as possible.
        /// </summary>
        public void ExpungeJungleClear(EntropyEventArgs args)
        {
            /// <summary>
            ///     The E Jungleclear Logic.
            /// </summary>
            if (UtilityClass.Player.Level() >=
                    MenuClass.Spells["e"]["junglesteal"].As<MenuSliderBool>().Value &&
                MenuClass.Spells["e"]["junglesteal"].As<MenuSliderBool>().Enabled)
            {
                foreach (var minion in Extensions.GetGenericJungleMinionsTargets().Where(m =>
                    IsPerfectExpungeTarget(m) &&
                    m.GetRealHealth() <= GetTotalExpungeDamage(m)))
                {
                    if (UtilityClass.JungleList.Contains(minion.CharName) &&
                        MenuClass.Spells["e"]["whitelist"][minion.CharName].As<MenuBool>().Enabled)
                    {
                        SpellClass.E.Cast();
                    }

                    if (!UtilityClass.JungleList.Contains(minion.CharName) &&
                        MenuClass.General["junglesmall"].As<MenuBool>().Enabled)
                    {
                        SpellClass.E.Cast();
                    }
                }
            }
        }

        #endregion
    }
}