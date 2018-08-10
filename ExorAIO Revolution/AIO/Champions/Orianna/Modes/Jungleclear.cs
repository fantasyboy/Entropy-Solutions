
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using SharpDX;

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
        public void JungleClear(EntropyEventArgs args)
        {
            if (GetBall() == null)
            {
                return;
            }

            /// <summary>
            ///     The Jungleclear W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.W["jungleclear"]) &&
                MenuClass.W["jungleclear"].Enabled)
            {
                if (Extensions.GetGenericJungleMinionsTargets().Any(m => m.IsValidTarget(SpellClass.W.Width, GetBall().Position)))
                {
                    SpellClass.W.Cast();
                }
            }


			var jungleTarget = Extensions.GetGenericJungleMinionsTargetsInRange(SpellClass.Q.Range)
				.MinBy(m => m.Distance(Hud.CursorPositionUnclipped));
			if (jungleTarget == null ||
                jungleTarget.HP < UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 3)
            {
                return;
            }

            /// <summary>
            ///     The Jungleclear E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.E["jungleclear"]) &&
                MenuClass.E["jungleclear"].Enabled)
            {
                if (ERectangle(jungleTarget).IsInsidePolygon((Vector2)jungleTarget.Position))
                {
                    SpellClass.E.CastOnUnit(UtilityClass.Player);
                }
            }

            /// <summary>
            ///     The Jungleclear Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                jungleTarget.IsValidTarget(SpellClass.Q.Range) &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["jungleclear"]) &&
                MenuClass.Q["jungleclear"].Enabled)
            {
                SpellClass.Q.GetPredictionInput(jungleTarget).From = GetBall().Position;
                SpellClass.Q.Cast(SpellClass.Q.GetPrediction(jungleTarget).CastPosition);
            }
        }

        #endregion
    }
}