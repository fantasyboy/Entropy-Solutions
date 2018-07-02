
using System.Linq;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Kalista
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Jungleclear()
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
                SpellClass.Q.Cast(jungleTarget);
            }
        }

        /// <summary>
        ///     Fired as fast as possible.
        /// </summary>
        public void RendJungleclear()
        {
            /// <summary>
            ///     The E Jungleclear Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.Level >=
                    MenuClass.Spells["e"]["junglesteal"].As<MenuSliderBool>().Value &&
                MenuClass.Spells["e"]["junglesteal"].As<MenuSliderBool>().Enabled)
            {
                foreach (var minion in Extensions.GetGenericJungleMinionsTargets().Where(m =>
                    IsPerfectRendTarget(m) &&
                    m.GetRealHealth() <= GetTotalRendDamage(m)))
                {
                    if (UtilityClass.JungleList.Contains(minion.UnitSkinName) &&
                        MenuClass.Spells["e"]["whitelist"][minion.UnitSkinName].As<MenuBool>().Enabled)
                    {
                        SpellClass.E.Cast();
                    }
                    else if (!UtilityClass.JungleList.Contains(minion.UnitSkinName) &&
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