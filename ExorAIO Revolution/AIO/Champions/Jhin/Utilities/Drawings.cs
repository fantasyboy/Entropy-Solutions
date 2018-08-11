using AIO.Utilities;
using Entropy;
using Entropy.SDK.UI.Components;
using Color = SharpDX.Color;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Jhin
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
                MenuClass.Drawings["q"].As<MenuBool>().Enabled)
            {
                Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.Q.Range, Color.LightGreen);
            }

            /// <summary>
            ///     Loads the W drawing.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Drawings["w"].As<MenuBool>().Enabled)
            {
                Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.W.Range, Color.Yellow);
            }

            /// <summary>
            ///     Loads the E drawing.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Drawings["e"].As<MenuBool>().Enabled)
            {
                Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.E.Range, Color.Cyan);
            }

            /// <summary>
            ///     Loads the R drawings.
            /// </summary>
            if (SpellClass.R.Ready)
            {
				if (!IsUltimateShooting() &&
					MenuClass.Drawings["r"].As<MenuBool>().Enabled)
				{
					Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.R2.Range, Color.Red);
				}

				if (IsUltimateShooting() &&
					MenuClass.Drawings["rcone"].As<MenuBool>().Enabled)
				{
					UltimateCone().Render(Color.Red);
				}
            }
        }

        #endregion
    }
}