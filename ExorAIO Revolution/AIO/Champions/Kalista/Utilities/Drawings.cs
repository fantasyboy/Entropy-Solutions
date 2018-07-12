
using System.Drawing;
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Rendering;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Kalista
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
            ///     Loads the E drawings.
            /// </summary>
            if (SpellClass.E.Ready)
            {
                /// <summary>
                ///     Loads the E Range.
                /// </summary>
                if (MenuClass.Drawings["e"].As<MenuBool>().Enabled)
                {
                    CircleRendering.Render(Color.Cyan, SpellClass.E.Range, UtilityClass.Player);
                }

                /// <summary>
                ///     Loads the E damage to healthbar.
                /// </summary>
                if (MenuClass.Drawings["edmg"].As<MenuBool>().Enabled)
                {
                    foreach (var unit in ObjectManager.Get<AIBaseClient>().Where(h =>
                        IsPerfectRendTarget(h) &&
                        (h is AIHeroClient || UtilityClass.JungleList.Contains(h.CharName)) &&
                        h.InfoBarPosition.OnScreen()))
                    {
                        var heroUnit = unit as AIHeroClient;
                        var jungleList = UtilityClass.JungleList;
                        var mobOffset = DrawingClass.JungleHpBarOffsetList.FirstOrDefault(x => x.CharName.Equals(unit.CharName));

                        int width;
                        if (jungleList.Contains(unit.CharName))
                        {
                            width = mobOffset?.Width ?? DrawingClass.SWidth;
                        }
                        else
                        {
                            width = DrawingClass.SWidth;
                        }

                        int height;
                        if (jungleList.Contains(unit.CharName))
                        {
                            height = mobOffset?.Height ?? DrawingClass.SHeight;
                        }
                        else
                        {
                            height = DrawingClass.SHeight;
                        }

                        int xOffset;
                        if (jungleList.Contains(unit.CharName))
                        {
                            xOffset = mobOffset?.XOffset ?? DrawingClass.SxOffset(heroUnit);
                        }
                        else
                        {
                            xOffset = DrawingClass.SxOffset(heroUnit);
                        }

                        int yOffset;
                        if (jungleList.Contains(unit.CharName))
                        {
                            yOffset = mobOffset?.YOffset ?? DrawingClass.SyOffset(heroUnit);
                        }
                        else
                        {
                            yOffset = DrawingClass.SyOffset(heroUnit);
                        }

                        var barPos = unit.InfoBarPosition;
                        barPos.X += xOffset;
                        barPos.Y += yOffset;

                        var unitHealth = unit.GetRealHealth();
                        var totalDamage = GetTotalRendDamage(unit);

                        var barLength = 0;
                        if (unitHealth > totalDamage)
                        {
                            barLength = (int)(width * ((unitHealth - totalDamage) / unit.MaxHP * 100 / 100));
                        }

                        var drawEndXPos = barPos.X + width * (unit.HPPercent() / 100);
                        var drawStartXPos = barPos.X + barLength;

                        Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true, unitHealth < totalDamage ? Color.Blue : Color.Orange);
                        Render.Line(drawStartXPos, barPos.Y, drawStartXPos, barPos.Y + height + 1, 1, true, Color.Lime);
                    }
                }
            }

            /// <summary>
            ///     Loads the R drawing.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Drawings["r"].As<MenuBool>().Enabled)
            {
                CircleRendering.Render(Color.Red, SpellClass.R.Range, UtilityClass.Player);
            }

            /// <summary>
            ///     Loads the Soulbound drawing.
            /// </summary>
            if (SoulBound != null &&
                MenuClass.Drawings["soulbound"].As<MenuSliderBool>().Enabled)
            {
                for (var i = 0; i < MenuClass.Drawings["soulbound"].As<MenuSliderBool>().Value; i++)
                {
                    Render.Circle(SoulBound.Position, SoulBound.BoundingRadius + 5 * i, (uint)(5 + i), Color.Cyan);
                }
            }
        }

        #endregion
    }
}