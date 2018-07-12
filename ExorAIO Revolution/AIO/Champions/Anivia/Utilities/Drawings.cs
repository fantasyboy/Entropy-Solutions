
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
    internal partial class Anivia
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
            ///     Loads the R drawing.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Drawings["r"].As<MenuBool>().Enabled)
            {
                CircleRendering.Render(Color.Red, SpellClass.R.Range, UtilityClass.Player);
            }

            /// <summary>
            ///     Loads the Q width drawing.
            /// </summary>
            if (FlashFrost != null &&
                MenuClass.Drawings["flashfrost"].As<MenuBool>().Enabled)
            {
                Render.Circle(FlashFrost.Position, SpellClass.Q.Width, 30, Color.Blue);
            }

            /// <summary>
            ///     Loads the R width drawing.
            /// </summary>
            if (GlacialStorm != null &&
                MenuClass.Drawings["glacialstorm"].As<MenuBool>().Enabled)
            {
                Render.Circle(GlacialStorm.Position, SpellClass.R.Width, 30, Color.Blue);
            }
        }

        #endregion
    }
}