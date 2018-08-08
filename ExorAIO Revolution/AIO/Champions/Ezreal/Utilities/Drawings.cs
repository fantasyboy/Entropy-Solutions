using AIO.Utilities;
using Entropy;
using Color = SharpDX.Color;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Ezreal
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the drawings.
        /// </summary>
        public void OnRender(EntropyEventArgs args)
        {
            /// <summary>
            ///     Loads the Q drawing.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Drawings["q"].Enabled)
            {
                Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.Q.Range, Color.LightGreen);
            }

            /// <summary>
            ///     Loads the W drawing.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Drawings["w"].Enabled)
            {
                Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.W.Range, Color.Yellow);
            }

            /// <summary>
            ///     Loads the E drawing.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Drawings["e"].Enabled)
            {
                Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.E.Range, Color.Cyan);
            }
        }

        #endregion
    }
}