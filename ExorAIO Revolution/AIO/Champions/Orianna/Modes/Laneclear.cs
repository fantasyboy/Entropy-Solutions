
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;

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
        public void LaneClear(EntropyEventArgs args)
        {
            if (GetBall() == null)
            {
                return;
            }

            /// <summary>
            ///     The Laneclear W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.W["laneclear"]) &&
                MenuClass.W["laneclear"].Enabled)
            {
                if (Extensions.GetEnemyLaneMinionsTargets().Count(m => m.IsValidTargetEx(SpellClass.W.Width, false, true, GetBall().Position))
                    >= MenuClass.W["customization"]["laneclear"].Value)
                {
                    SpellClass.W.Cast();
                }
            }

            /// <summary>
            ///     The Laneclear E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.E["laneclear"]) &&
                MenuClass.E["laneclear"].Enabled)
            {
                if (Extensions.GetEnemyLaneMinionsTargets()
	                    .Count(t => t.IsValidTargetEx() && ERectangle(t).IsInsidePolygon(t.Position)) >= MenuClass.E["customization"]["laneclear"].Value)
                {
                    SpellClass.E.CastOnUnit(UtilityClass.Player);
                }
            }

            /// <summary>
            ///     The Q Farmhelper Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                > ManaManager.GetNeededMana(SpellSlot.Q, MenuClass.Q["farmhelper"]) &&
                MenuClass.Q["farmhelper"].Enabled)
            {
                foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.Q.Range).Where(m => !m.IsValidTargetEx(UtilityClass.Player.GetAutoAttackRange(m))))
                {
                    if (minion.GetRealHealth() < GetQDamage(minion))
                    {
                        SpellClass.Q.GetPredictionInput(minion).From = GetBall().Position;
                        SpellClass.Q.Cast(SpellClass.Q.GetPrediction(minion).CastPosition);
                    }
                }
            }

            /// <summary>
            ///     The Laneclear Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["laneclear"]) &&
                MenuClass.Q["laneclear"].Enabled)
            {
                /*
                var farmLocation = SpellClass.Q.GetLinearFarmLocation(Extensions.GetEnemyLaneMinionsTargets(), SpellClass.Q.Width);
                if (farmLocation.MinionsHit >= MenuClass.Q["customization"]["laneclear"].Value)
                {
                    SpellClass.Q.Cast(farmLocation.Position);
                }
                */
            }
        }

        #endregion
    }
}