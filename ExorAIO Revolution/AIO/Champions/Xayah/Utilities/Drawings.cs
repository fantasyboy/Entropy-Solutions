using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;
using SharpDX;
using Color = System.Drawing.Color;

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
        public void Drawings()
        {
            /// <summary>
            ///     Loads the Q drawing.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Drawings["q"].As<MenuBool>().Enabled)
            {
                Render.Circle(UtilityClass.Player.Position, SpellClass.Q.Range, 30, Color.LightGreen);
            }

            /// <summary>
            ///     Loads the Feather linking drawing.
            /// </summary>
            if (!UtilityClass.Player.Spellbook.GetSpell(SpellSlot.E).State.HasFlag(SpellState.Cooldown) &&
                MenuClass.Drawings["feathers"].As<MenuBool>().Enabled)
            {
                foreach (var feather in Feathers)
                {
                    var drawFeatherPos = feather.Value.FixHeight();
                    var realFeatherHitbox = new Vector2Geometry.Rectangle((Vector2)UtilityClass.Player.Position, (Vector2)drawFeatherPos, SpellClass.E.Width);
                    var drawFeatherHitbox = new Vector3Geometry.Rectangle(UtilityClass.Player.Position, drawFeatherPos, SpellClass.E.Width);

                    drawFeatherHitbox.Draw(
                        GameObjects.EnemyHeroes.Any(h =>
                            h.IsValidTarget() &&
                            realFeatherHitbox.IsInside((Vector2)h.Position))
                                ? Color.Blue
                                : Color.Yellow);
                    Render.Circle(drawFeatherPos, SpellClass.E.Width, 5, Color.OrangeRed);
                }
            }

            /// <summary>
            ///     Loads the R drawing.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Drawings["r"].As<MenuBool>().Enabled)
            {
                Render.Circle(UtilityClass.Player.Position, SpellClass.R.Range, 30, Color.Red);
            }
        }

        #endregion
    }
}