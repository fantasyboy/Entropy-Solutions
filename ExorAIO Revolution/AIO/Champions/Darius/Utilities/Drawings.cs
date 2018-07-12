
using System.Drawing;
using System.Linq;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Rendering;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Darius
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
            if (SpellClass.R.Ready)
            {
                if (MenuClass.Drawings["r"].As<MenuBool>().Enabled)
                {
                    CircleRendering.Render(Color.Red, SpellClass.R.Range, UtilityClass.Player);
                }

                /// <summary>
                ///     Loads the R damage to healthbar.
                /// </summary>
                if (MenuClass.Drawings["rdmg"].As<MenuBool>().Enabled)
                {
                    foreach (var hero in Extensions.GetEnemyHeroesTargetsInRange(SpellClass.R.Range).Where(h =>
                        !Invulnerable.Check(h) &&
                        h.InfoBarPosition.OnScreen()))
                    {
                        var width = DrawingClass.SWidth;
                        var height = DrawingClass.SHeight;
                        var xOffset = DrawingClass.SxOffset(hero);
                        var yOffset = DrawingClass.SyOffset(hero);

                        var barPos = hero.InfoBarPosition;
                        barPos.X += xOffset;
                        barPos.Y += yOffset;

                        var unitHealth = hero.GetRealHealth();
                        var totalDamage = GetTotalNoxianGuillotineDamage(hero);

                        var barLength = 0;
                        if (unitHealth > totalDamage)
                        {
                            barLength = (int)(width * ((unitHealth - totalDamage) / hero.MaxHP * 100 / 100));
                        }

                        var drawEndXPos = barPos.X + width * (hero.HPPercent() / 100);
                        var drawStartXPos = barPos.X + barLength;

                        Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true, unitHealth < totalDamage ? Color.Blue : Color.Orange);
                        Render.Line(drawStartXPos, barPos.Y, drawStartXPos, barPos.Y + height + 1, 1, true, Color.Lime);
                    }
                }
            }
        }

        #endregion
    }
}