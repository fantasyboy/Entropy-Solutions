
using System.Drawing;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Caitlyn
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
            ///     Loads the R drawing.
            /// </summary>
            if (SpellClass.R.Ready)
            {
                if (MenuClass.Drawings["r"].As<MenuBool>().Enabled)
                {
                    Render.Circle(UtilityClass.Player.Position, SpellClass.R.Range, 30, Color.Red);
                }

                if (MenuClass.Drawings["rmm"].As<MenuBool>().Enabled)
                {
                    Vector2Geometry.DrawCircleOnMinimap(UtilityClass.Player.Position, SpellClass.R.Range, Color.White);
                }
            }

            /// <summary>
            ///     Loads the R damage to healthbar.
            /// </summary>
            if (MenuClass.Drawings["rdmg"].As<MenuBool>().Enabled)
            {
                foreach (var hero in Extensions.GetEnemyHeroesTargetsInRange(SpellClass.R.Range).Where(h =>
                    !Invulnerable.Check(h) &&
                    h.FloatingHealthBarPosition.OnScreen()))
                {
                    var width = DrawingClass.SWidth;
                    var height = DrawingClass.SHeight;

                    var xOffset = DrawingClass.SxOffset(hero);
                    var yOffset = DrawingClass.SyOffset(hero);

                    var barPos = hero.FloatingHealthBarPosition;
                    barPos.X += xOffset;
                    barPos.Y += yOffset;

                    var unitHealth = hero.GetRealHealth();
                    var totalDamage = UtilityClass.Player.GetSpellDamage(hero, SpellSlot.R);

                    var barLength = 0;
                    if (unitHealth > totalDamage)
                    {
                        barLength = (int)(width * ((unitHealth - totalDamage) / hero.MaxHealth * 100 / 100));
                    }

                    var drawEndXPos = barPos.X + width * (hero.HealthPercent() / 100);
                    var drawStartXPos = barPos.X + barLength;

                    Render.Line(drawStartXPos, barPos.Y, drawEndXPos, barPos.Y, height, true, unitHealth < totalDamage ? Color.Blue : Color.Orange);
                    Render.Line(drawStartXPos, barPos.Y, drawStartXPos, barPos.Y + height + 1, 1, true, Color.Lime);
                }
            }
        }

        #endregion
    }
}