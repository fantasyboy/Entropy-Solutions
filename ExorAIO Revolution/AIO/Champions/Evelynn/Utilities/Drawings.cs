
using System.Drawing;
using Entropy;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using AIO.Utilities;

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
                    Render.Circle(UtilityClass.Player.Position, SpellClass.Q.Range, 30, Color.LightGreen);
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
                Render.Circle(UtilityClass.Player.Position, SpellClass.W.Range, 30, Color.Yellow);
            }

            /// <summary>
            ///     Loads the E drawing.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Drawings["e"].As<MenuBool>().Enabled)
            {
                Render.Circle(UtilityClass.Player.Position, SpellClass.E.Range, 30, Color.Cyan);
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
                    Render.Circle(UtilityClass.Player.Position, SpellClass.R.Range, 30, Color.Red);
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