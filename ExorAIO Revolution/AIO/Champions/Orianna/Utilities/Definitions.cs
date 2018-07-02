﻿
using System;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Orianna
    {
        #region Fields

        /// <summary>
        ///     The default position of the ball.
        /// </summary>
        public Vector3? BallPosition;

        /// <summary>
        ///     The default drawing position of the ball.
        /// </summary>
        public Vector3? DrawingBallPosition;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the position of the ball.
        /// </summary>
        public Vector3? GetBallPosition()
        {
            var possiblePosition1 = GameObjects.AllyMinions.FirstOrDefault(m =>
                    Math.Abs(m.GetRealHealth()) > 0 &&
                    m.UnitSkinName.Equals("OriannaBall"));
            if (possiblePosition1 != null)
            {
                return possiblePosition1.ServerPosition;
            }

            var possiblePosition2 = GameObjects.AllyHeroes.FirstOrDefault(a =>
                    !a.IsMe &&
                    a.ValidActiveBuffs().Any(b =>
                        b.Caster.IsMe &&
                        b.Name.Equals("orianaghost")));
            if (possiblePosition2 != null)
            {
                return possiblePosition2.ServerPosition;
            }

            if (UtilityClass.Player.HasBuff("orianaghostself"))
            {
                return UtilityClass.Player.ServerPosition;
            }

            return null;
        }

        /// <summary>
        ///     Gets the position of the ball.
        /// </summary>
        public Vector3? GetBallDrawingPosition()
        {
            var possiblePosition1 = GameObjects.AllyMinions.FirstOrDefault(m =>
                Math.Abs(m.GetRealHealth()) > 0 &&
                m.UnitSkinName.Equals("OriannaBall"));
            if (possiblePosition1 != null)
            {
                return possiblePosition1.Position;
            }

            var possiblePosition2 = GameObjects.AllyHeroes.FirstOrDefault(a =>
                    !a.IsMe &&
                    a.ValidActiveBuffs().Any(b =>
                        b.Caster.IsMe &&
                        b.Name.Equals("orianaghost")));
            if (possiblePosition2 != null)
            {
                return possiblePosition2.Position;
            }

            if (UtilityClass.Player.HasBuff("orianaghostself"))
            {
                return UtilityClass.Player.Position;
            }

            return null;
        }

        /// <summary>
        ///     Updates the position of the ball.
        /// </summary>
        public void UpdateBallPosition()
        {
            BallPosition = GetBallPosition();
        }

        /// <summary>
        ///     Updates the position of the ball.
        /// </summary>
        public void UpdateDrawingBallPosition()
        {
            DrawingBallPosition = GetBallDrawingPosition();
        }

        #endregion
    }
}