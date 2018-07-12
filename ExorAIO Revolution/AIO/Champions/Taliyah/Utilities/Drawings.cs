
using System.Drawing;
using AIO.Utilities;
using Entropy.SDK.Rendering;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Taliyah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the drawings.
        /// </summary>
        public void Drawings()
        {
            /// <summary>
            ///     Loads the Q drawing.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Drawings["q"].As<MenuBool>().Enabled)
            {
                CircleRendering.Render(Color.LightGreen, SpellClass.Q.Range, UtilityClass.Player);
            }

            /// <summary>
            ///     Loads the W drawing.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Drawings["w"].As<MenuBool>().Enabled)
            {
                CircleRendering.Render(Color.Yellow, SpellClass.W.Range, UtilityClass.Player);
            }

            /// <summary>
            ///     Loads the E drawing.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Drawings["e"].As<MenuBool>().Enabled)
            {
                CircleRendering.Render(Color.Cyan, SpellClass.E.Range, UtilityClass.Player);
            }

            /// <summary>
            ///     Loads the R drawings.
            /// </summary>
            if (SpellClass.R.Ready)
            {
                /// <summary>
                ///     Loads the R range drawing.
                /// </summary>
                if (MenuClass.Drawings["r"].As<MenuBool>().Enabled)
                {
                    CircleRendering.Render(Color.Red, SpellClass.R.Range, UtilityClass.Player);
                }

                /// <summary>
                ///     Loads the R minimap drawing.
                /// </summary>
                if (MenuClass.Drawings["rmm"].As<MenuBool>().Enabled)
                {
                    Vector2Geometry.DrawCircleOnMinimap(UtilityClass.Player.Position, SpellClass.R.Range, Color.White);
                }
            }

            /// <summary>
            ///     Loads the WorkedGrounds drawing.
            /// </summary>
            if (MenuClass.Drawings["grounds"].As<MenuBool>().Enabled)
            {
                foreach (var ground in WorkedGrounds)
                {
                    Render.Circle(ground.Value, WorkedGroundWidth, 30, Color.Brown);
                }
            }

            /// <summary>
            ///     Loads the MineFields drawing.
            /// </summary>
            if (MenuClass.Drawings["boulders"].As<MenuBool>().Enabled)
            {
                foreach (var boulder in MineField)
                {
                    Render.Circle(boulder.Value, BouldersWidth, 30, Color.Brown);
                }
            }
        }

        #endregion
    }
}