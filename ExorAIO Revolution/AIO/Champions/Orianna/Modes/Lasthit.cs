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
        public void LastHit(EntropyEventArgs args)
        {
            if (GetBall() == null)
            {
                return;
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
        }

        #endregion
    }
}