// ReSharper disable ArrangeMethodOrOperatorBody

using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using SharpDX;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Lucian
    {
        #region Fields

        /// <summary>
        ///     Returns true if the player is using the ultimate.
        /// </summary>
        public bool IsCulling()
        {
            return UtilityClass.Player.HasBuff("LucianR");
        }

        /// <summary>
        ///     The Q Rectangle.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public Vector2Geometry.Rectangle QRectangle(AIBaseClient unit)
        {
            return new Vector2Geometry.Rectangle(
                (Vector2)UtilityClass.Player.Position,
                (Vector2)UtilityClass.Player.Position.Extend(unit.Position, SpellClass.Q2.Range - 100f),
                SpellClass.Q2.Width);
        }

        #endregion
    }
}