
using System.Linq;
using Aimtec;
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
        public void Jungleclear()
        {
            var jungleTarget = ObjectManager.Get<Obj_AI_Minion>()
                .Where(m => Extensions.GetGenericJungleMinionsTargets().Contains(m))
                .MinBy(m => m.Distance(UtilityClass.Player));
            if (jungleTarget == null)
            {
                return;
            }

            /// <summary>
            ///     The W Jungleclear Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                !IsAllured(jungleTarget) &&
                jungleTarget.IsValidTarget(SpellClass.W.Range) &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["jungleclear"]) &&
                MenuClass.Spells["w"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                var targetName = jungleTarget.UnitSkinName;
                if (UtilityClass.JungleList.Contains(targetName) &&
                    MenuClass.Spells["w"]["whitelist"][targetName].As<MenuBool>().Enabled)
                {
                    UtilityClass.CastOnUnit(SpellClass.W, jungleTarget);
                    return;
                }
            }

            if (IsFullyAllured(jungleTarget))
            {
                /// <summary>
                ///     The E Jungleclear Logic.
                /// </summary>
                if (SpellClass.E.Ready &&
                    IsWhiplashEmpowered() &&
                    jungleTarget.IsValidTarget(SpellClass.E.Range) &&
                    UtilityClass.Player.ManaPercent()
                        > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["jungleclear"]) &&
                    MenuClass.Spells["e"]["jungleclear"].As<MenuSliderBool>().Enabled)
                {
                    UtilityClass.CastOnUnit(SpellClass.E, jungleTarget);
                }
            }

            if (IsAllured(jungleTarget) && !IsFullyAllured(jungleTarget))
            {
                return;
            }

            /// <summary>
            ///     The Q Jungleclear Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                jungleTarget.IsValidTarget(SpellClass.Q.Range) &&
                (UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["jungleclear"]) || !IsHateSpikeSkillshot()) &&
                MenuClass.Spells["q"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                if (IsHateSpikeSkillshot())
                {
                    SpellClass.Q.Cast(jungleTarget);
                }
                else
                {
                    UtilityClass.CastOnUnit(SpellClass.Q, jungleTarget);
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
                UtilityClass.CastOnUnit(SpellClass.E, jungleTarget);
            }
        }

        #endregion
    }
}