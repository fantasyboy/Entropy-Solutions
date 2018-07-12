
using System.Drawing;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Rendering;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Evelynn
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
                // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                if (IsHateSpikeSkillshot())
                {
                    CircleRendering.Render(Color.LightGreen, SpellClass.Q.Range, UtilityClass.Player);
                }
                else
                {
                    Render.Circle(UtilityClass.Player.Position, SpellClass.Q2.Range, 30, Color.LightGreen);
                }
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
                ///     Loads the R range.
                /// </summary>
                if (MenuClass.Drawings["r"].As<MenuBool>().Enabled)
                {
                    CircleRendering.Render(Color.Red, SpellClass.R.Range, UtilityClass.Player);
                }

                /// <summary>
                ///     Loads the R safe position check drawing.
                /// </summary>
                if (UtilityClass.Player.Path.Length > 1  &&
                    MenuClass.Drawings["rsafepos"].As<MenuBool>().Enabled)
                {
                    Render.Circle(UtilityClass.Player.Position.Extend(UtilityClass.Player.Path[1], -LastCaressPushBackDistance()), UtilityClass.Player.BoundingRadius, 30, Color.Red);
                    Render.Circle(UtilityClass.Player.Position.Extend(UtilityClass.Player.Path[1], -LastCaressPushBackDistance()), MenuClass.Spells["r"]["customization"]["safetyrange"].As<MenuSlider>().Value, 30, Color.Red);
                }
            }
        }

        #endregion
    }
}