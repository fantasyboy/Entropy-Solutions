using AIO.Utilities;
using Entropy;
using Color = SharpDX.Color;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The prediction drawings class.
    /// </summary>
    internal partial class Orianna
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
            ///     Loads the E drawing.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Drawings["e"].Enabled)
            {
                Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.E.Range, Color.Cyan);
            }

            if (GetBall() == null)
            {
                return;
            }

            /// <summary>
            ///     Loads the Ball drawing.
            /// </summary>
            if (MenuClass.Drawings["ball"].Enabled)
            {
				Renderer.DrawCircularRangeIndicator(GetBall().Position, SpellClass.Q.Width, Color.Aquamarine);
            }

            /// <summary>
            ///     Loads the W width drawing.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Drawings["ballw"].Enabled)
            {
				Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.W.Width, Color.Yellow);
			}

            /// <summary>
            ///     Loads the R width drawing.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Drawings["ballr"].Enabled)
            {
				Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.R.Width, Color.Red);
			}
        }

        #endregion
    }
}