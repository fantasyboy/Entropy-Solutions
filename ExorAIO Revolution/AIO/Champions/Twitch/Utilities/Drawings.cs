
using System.Drawing;
using System.Linq;
using Entropy;
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
    internal partial class Twitch
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the drawings.
        /// </summary>
        public void Drawings()
        {
            /// <summary>
            ///     Loads the Q duration drawing.
            /// </summary>
            if (!UtilityClass.Player.Spellbook.GetSpell(SpellSlot.Q).State.HasFlag(SpellState.NotLearned) &&
                MenuClass.Drawings["qduration"].As<MenuBool>().Enabled)
            {
                var buff = UtilityClass.Player.GetBuff("TwitchHideInShadows");
                if (buff != null)
                {
                    Render.Circle(UtilityClass.Player.Position, (buff.EndTime - Game.ClockTime) * UtilityClass.Player.MoveSpeed, 30, Color.Green);
                }
                else
                {
                    var qDuration = new[] { 10, 11, 12, 13, 14 }[UtilityClass.Player.Spellbook.GetSpell(SpellSlot.Q).Level - 1];
                    Render.Circle(UtilityClass.Player.Position, qDuration * UtilityClass.Player.MoveSpeed, 30, Color.Green);
                    Vector2Geometry.DrawCircleOnMinimap(UtilityClass.Player.Position, qDuration * UtilityClass.Player.MoveSpeed, Color.White);
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
            if (SpellClass.E.Ready)
            {
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
                        IsPerfectExpungeTarget(h) &&
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
                        var totalDamage = GetTotalExpungeDamage(unit);

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
        }

        #endregion
    }
}