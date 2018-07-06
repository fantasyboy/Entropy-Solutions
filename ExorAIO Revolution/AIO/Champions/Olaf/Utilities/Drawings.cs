
using System.Drawing;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Olaf
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the drawings.
        /// </summary>
        public void Drawings()
        {
            /// <summary>
            ///     Loads the Q range drawing.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Drawings["q"].As<MenuBool>().Enabled)
            {
                Render.Circle(UtilityClass.Player.Position, SpellClass.Q.Range, 30, Color.LightGreen);
            }

            /// <summary>
            ///     Loads the Q path drawing.
            /// </summary>
            if (MenuClass.Drawings["qpath"].As<MenuBool>().Enabled)
            {
                foreach (var axe in Axes)
                {
                    var drawAxePos = axe.Value.FixHeight();
                    var axeRectangle = new Vector3Geometry.Rectangle(UtilityClass.Player.Position, drawAxePos, UtilityClass.Player.BoundingRadius);

                    axeRectangle.Draw(Color.Yellow);
                    Render.Circle(drawAxePos, UtilityClass.Player.BoundingRadius, 5, Color.OrangeRed);
                }
            }
        }

        #endregion
    }
}