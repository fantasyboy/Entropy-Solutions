
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression

using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Taliyah
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
            ///     The Jungleclear Rylai Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                jungleTarget.IsValidTarget(SpellClass.Q.Range) &&
                UtilityClass.Player.HasItem(ItemId.RylaisCrystalScepter) &&
                (IsNearWorkedGround() ||
                 UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["jungleclear"])) &&
                MenuClass.Spells["q"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                switch (MenuClass.Spells["q"]["modes"]["jungleclear"].As<MenuList>().Value)
                {
                    case 0:
                        if (!IsNearWorkedGround())
                        {
                            SpellClass.Q.Cast(jungleTarget);
                        }
                        break;
                    case 1:
                        SpellClass.Q.Cast(jungleTarget);
                        break;
                }
            }

            var targetPosAfterW = new Vector3();

            /// <summary>
            ///     The Jungleclear W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                jungleTarget.IsValidTarget(SpellClass.W.Range) &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["jungleclear"]) &&
                MenuClass.Spells["w"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                var bestBoulderHitPos = GetBestBouldersHitPosition(jungleTarget);
                var bestBoulderHitPosHitBoulders = GetBestBouldersHitPositionHitBoulders(jungleTarget);
                var jungleTargetPredPos = SpellClass.W.GetPrediction(jungleTarget).CastPosition;

                if (SpellClass.E.Ready)
                {
                    if (UtilityClass.Player.Distance(GetUnitPositionAfterPull(jungleTarget)) >= 200f)
                    {
                        targetPosAfterW = GetUnitPositionAfterPull(jungleTarget);
                    }
                    else
                    {
                        targetPosAfterW = GetUnitPositionAfterPush(jungleTarget);
                    }

                    //SpellClass.W.Cast(jungleTargetPredPos, targetPosAfterW);
                    SpellClass.W.Cast(targetPosAfterW, jungleTargetPredPos);
                }
                else if (bestBoulderHitPos != Vector3.Zero && bestBoulderHitPosHitBoulders > 0)
                {
                    //SpellClass.W.Cast(jungleTargetPredPos, bestBoulderHitPos);
                    SpellClass.W.Cast(bestBoulderHitPos, jungleTargetPredPos);
                }
            }

            /// <summary>
            ///     The Jungleclear E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                jungleTarget.IsValidTarget(SpellClass.E.Range) &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["jungleclear"]) &&
                MenuClass.Spells["e"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.E.Cast(SpellClass.W.Ready
                    ? targetPosAfterW
                    : jungleTarget.Position);
            }

            /// <summary>
            ///     The Jungleclear Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                jungleTarget.IsValidTarget(SpellClass.Q.Range) &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["jungleclear"]) &&
                MenuClass.Spells["q"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                switch (MenuClass.Spells["q"]["modes"]["jungleclear"].As<MenuList>().Value)
                {
                    case 0:
                        if (!IsNearWorkedGround())
                        {
                            SpellClass.Q.Cast(jungleTarget);
                        }
                        break;
                    case 1:
                        SpellClass.Q.Cast(jungleTarget);
                        break;
                }
            }
        }

        #endregion
    }
}