// ReSharper disable ArrangeMethodOrOperatorBody


using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;

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
        public Vector2Geometry.Rectangle QRectangle(Obj_AI_Base unit)
        {
            return new Vector2Geometry.Rectangle(
                (Vector2)UtilityClass.Player.ServerPosition,
                (Vector2)UtilityClass.Player.ServerPosition.Extend(unit.ServerPosition, SpellClass.Q2.Range - 100f),
                SpellClass.Q2.Width);
        }

        #endregion
    }
}