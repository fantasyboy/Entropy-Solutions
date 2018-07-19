using System;
using System.Linq;
using Entropy.SDK.Caching;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Rendering;
using NabbTracker.Utilities;
using SharpDX;
using Color = SharpDX.Color;

namespace NabbTracker.Trackers
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
            foreach (var hero in ObjectCache.AllHeroes.Where(h =>
                !h.IsDead &&
                h.IsVisible &&
                Math.Abs(h.InfoBarPosition.X) > 0))
            {
                if (hero.Name.Equals("Target Dummy"))
                {
                    continue;
                }

                if (hero.IsMe() &&
                    !MenuClass.ExpTracker["me"].Enabled)
                {
                    continue;
                }

                if (hero.IsEnemy() &&
                    !MenuClass.ExpTracker["enemies"].Enabled)
                {
                    continue;
                }

                if (!hero.IsMe() &&
                    hero.IsAlly() &&
                    !MenuClass.ExpTracker["allies"].Enabled)
                {
                    continue;
                }

                var xOffset = (int)hero.InfoBarPosition.X + UtilityClass.ExpXAdjustment(hero);
                var yOffset = (int)hero.InfoBarPosition.Y + UtilityClass.ExpYAdjustment(hero);

                var actualExp = hero.HeroExperience.Exp;
                if (hero.Level() > 1)
                {
                    actualExp -= (280 + 80 + 100 * hero.Level()) / 2 * (hero.Level() - 1);
                }

                var levelLimit = hero.HasBuff("AwesomeBuff") ? 30 : 18;
                if (hero.Level() < levelLimit)
                {
                    LineRendering.Render(Colors.GetRealColor(Color.Purple), 7, new Vector2(xOffset - 76, yOffset + 20), new Vector2(xOffset + 56, yOffset + 20));

                    var neededExp = 180 + 100 * hero.Level();
                    var expPercent = (int)(actualExp / neededExp * 100);
                    if (expPercent > 0)
                    {
	                    LineRendering.Render(Colors.GetRealColor(Color.Red), 7, new Vector2(xOffset - 76, yOffset + 20), new Vector2(xOffset - 76 + (float)(1.32 * expPercent), yOffset + 20));
                    }

                    TextRendering.Render(expPercent > 0 ? expPercent + "%" : "0%", Colors.GetRealColor(Color.Yellow), new Vector2(xOffset - 13, yOffset + 17));
                }
            }
        }

        #endregion
    }
}