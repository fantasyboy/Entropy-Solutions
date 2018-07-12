using System.Linq;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Rendering;
using Entropy.SDK.UI.Components;
using SharpDX;
using Color = System.Drawing.Color;

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
                ///     Loads the R cone drawing.
                /// </summary>
                if (IsUltimateShooting() &&
                    MenuClass.Drawings["rcone"].As<MenuBool>().Enabled)
                {
                    if (End != Vector3.Zero)
                    {
                        DrawUltimateCone().Draw(GameObjects.EnemyHeroes.Any(t => t.IsValidTarget() && UltimateCone().IsInside((Vector2)t.Position))
                            ? Color.Green
                            : Color.Red);
                    }
                }
                /// <summary>
                ///     Loads the R range drawing.
                /// </summary>
                else if (!IsUltimateShooting() &&
                         MenuClass.Drawings["r"].As<MenuBool>().Enabled)
                {
                    CircleRendering.Render(Color.Red, SpellClass.R.Range, UtilityClass.Player);
                }
            }
        }

        #endregion
    }
}