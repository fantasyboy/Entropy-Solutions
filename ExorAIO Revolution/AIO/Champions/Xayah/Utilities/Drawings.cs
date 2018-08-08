using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Rendering;
using Entropy.SDK.UI.Components;
using SharpDX;
using Entropy.SDK.Caching;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Xayah
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
			///     Loads the Feather linking drawing.
			/// </summary>
			if (!UtilityClass.Player.Spellbook.GetSpellState(SpellSlot.E).HasFlag(SpellState.Cooldown) &&
				MenuClass.Drawings["feathers"].As<MenuBool>().Enabled)
			{
				foreach (var feather in Feathers)
				{
					var drawFeatherPos = feather.Value.FixHeight();
					var realFeatherHitbox = new Entropy.SDK.Geometry.Rectangle(UtilityClass.Player.Position, drawFeatherPos, SpellClass.E.Width);

					realFeatherHitbox.Render(
						ObjectCache.EnemyHeroes.Any(h =>
							h.IsValidTargetEx() &&
							realFeatherHitbox.IsInsidePolygon((Vector2)h.Position))
								? Color.Blue
								: Color.Yellow);
					CircleRendering.Render(Color.OrangeRed, SpellClass.E.Width, 5, (Vector2)drawFeatherPos);
				}
			}

			/// <summary>
			///     Loads the R drawing.
			/// </summary>
			if (SpellClass.R.Ready &&
                MenuClass.Drawings["r"].As<MenuBool>().Enabled)
            {
				Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.R.Range, Color.Red);
			}
        }

        #endregion
    }
}