using System;
using System.Drawing;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Menu.Components;

namespace NabbTracker
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal class ExpTracker
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the Experience Tracker.
        /// </summary>
        public static void Initialize()
        {
            foreach (var hero in ObjectManager.Get<AIHeroClient>().Where(h =>
                !h.IsDead &&
                h.IsVisible &&
                Math.Abs(h.InfoBarPosition.X) > 0))
            {
                if (hero.Name.Equals("Target Dummy"))
                {
                    continue;
                }

                if (hero.IsMe() &&
                    !MenuClass.ExpTracker["me"].As<MenuBool>().Enabled)
                {
                    continue;
                }

                if (hero.IsEnemy() &&
                    !MenuClass.ExpTracker["enemies"].As<MenuBool>().Enabled)
                {
                    continue;
                }

                if (!hero.IsMe() &&
                    hero.IsAlly() &&
                    !MenuClass.ExpTracker["allies"].As<MenuBool>().Enabled)
                {
                    continue;
                }

                var xOffset = (int)hero.InfoBarPosition.X + UtilityClass.ExpXAdjustment(hero);
                var yOffset = (int)hero.InfoBarPosition.Y + UtilityClass.ExpYAdjustment(hero);

                var actualExp = hero.Exp;
                if (hero.Level > 1)
                {
                    actualExp -= (280 + 80 + 100 * hero.Level) / 2 * (hero.Level - 1);
                }

                var levelLimit = hero.HasBuff("AwesomeBuff") ? 30 : 18;
                if (hero.Level < levelLimit)
                {
                    Render.Line(xOffset - 76, yOffset + 20, xOffset + 56, yOffset + 20, 7, true, Colors.GetRealColor(Color.Purple));

                    var neededExp = 180 + 100 * hero.Level;
                    var expPercent = (int)(actualExp / neededExp * 100);
                    if (expPercent > 0)
                    {
                        Render.Line(xOffset - 76, yOffset + 20, xOffset - 76 + (float)(1.32 * expPercent), yOffset + 20, 7, true, Colors.GetRealColor(Color.Red));
                    }

                    TextRendering.Render(expPercent > 0 ? expPercent + "%" : "0%", new Vector2(xOffset - 13, yOffset + 17), RenderTextFlags.None, Colors.GetRealColor(Color.Yellow));
                }
            }
        }

        #endregion
    }
}