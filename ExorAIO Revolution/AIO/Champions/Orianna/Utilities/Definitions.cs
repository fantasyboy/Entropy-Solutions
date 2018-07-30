
using System;
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Orianna
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the ball.
        /// </summary>
        public AIBaseClient GetBall()
        {
            var possiblePosition1 = GameObjects.AllyMinions.FirstOrDefault(m =>
                    Math.Abs(m.GetRealHealth()) > 0 &&
                    m.CharName.Equals("OriannaBall"));
            if (possiblePosition1 != null)
            {
                return possiblePosition1;
            }

            var possiblePosition2 = GameObjects.AllyHeroes.FirstOrDefault(a =>
                    !a.IsMe() &&
                    a.GetActiveBuffs().Any(b =>
                        b.Caster.IsMe() &&
                        b.Name.Equals("orianaghost")));
            if (possiblePosition2 != null)
            {
                return possiblePosition2;
            }

            if (UtilityClass.Player.HasBuff("orianaghostself"))
            {
                return UtilityClass.Player;
            }

            return null;
        }

        #endregion
    }
}