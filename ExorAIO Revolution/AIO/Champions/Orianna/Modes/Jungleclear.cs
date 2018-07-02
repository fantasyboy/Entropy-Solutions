
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
    internal partial class Orianna
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Jungleclear()
        {
            if (BallPosition == null)
            {
                return;
            }

            /// <summary>
            ///     The Jungleclear W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["jungleclear"]) &&
                MenuClass.Spells["w"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                if (Extensions.GetGenericJungleMinionsTargets().Any(m => m.IsValidTarget(SpellClass.W.Width, false, true, (Vector3)BallPosition)))
                {
                    SpellClass.W.Cast();
                }
            }


            var jungleTarget = ObjectManager.Get<Obj_AI_Minion>()
                .Where(m => Extensions.GetGenericJungleMinionsTargets().Contains(m))
                .MinBy(m => m.Distance(UtilityClass.Player));
            if (jungleTarget == null ||
                jungleTarget.GetRealHealth() < UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 3)
            {
                return;
            }

            /// <summary>
            ///     The Jungleclear E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["jungleclear"]) &&
                MenuClass.Spells["e"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                var polygon = new Vector2Geometry.Rectangle(
                    (Vector2)UtilityClass.Player.ServerPosition,
                    (Vector2)UtilityClass.Player.ServerPosition.Extend((Vector3)BallPosition, UtilityClass.Player.Distance((Vector3)BallPosition)),
                    SpellClass.E.Width);

                if (!polygon.IsOutside((Vector2)jungleTarget.ServerPosition))
                {
                    UtilityClass.CastOnUnit(SpellClass.E, UtilityClass.Player);
                }
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
                SpellClass.Q.GetPredictionInput(jungleTarget).From = (Vector3)BallPosition;
                SpellClass.Q.Cast(SpellClass.Q.GetPrediction(jungleTarget).CastPosition);
            }
        }

        #endregion
    }
}