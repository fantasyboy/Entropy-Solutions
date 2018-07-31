using AIO.Utilities;
using Entropy;
using Entropy.SDK.Rendering;
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
                CircleRendering.Render(Color.LightGreen, SpellClass.Q.Range, UtilityClass.Player);
            }

            /// <summary>
            ///     Loads the E drawing.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Drawings["e"].Enabled)
            {
                CircleRendering.Render(Color.Cyan, SpellClass.E.Range, UtilityClass.Player);
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
                for (var i = 0; i < MenuClass.Drawings["ball"].Value; i++)
                {
	                CircleRendering.Render(Color.OrangeRed, 80 + 5 * i, GetBall());
				}
            }

            /// <summary>
            ///     Loads the W width drawing.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Drawings["ballw"].Enabled)
            {
				CircleRendering.Render(Color.Yellow, SpellClass.W.Width, GetBall());
			}

            /// <summary>
            ///     Loads the R width drawing.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Drawings["ballr"].Enabled)
            {
				CircleRendering.Render(Color.Red, SpellClass.R.Width, GetBall());
			}
        }

        #endregion
    }
}