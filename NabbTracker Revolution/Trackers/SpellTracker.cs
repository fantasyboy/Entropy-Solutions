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
    internal class SpellTracker
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the SpellTracker.
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
                    !MenuClass.SpellTracker["me"].Enabled)
                {
                    continue;
                }

                if (hero.IsEnemy() &&
                    !MenuClass.SpellTracker["enemies"].Enabled)
                {
                    continue;
                }

                if (!hero.IsMe() &&
                    hero.IsAlly() &&
                    !MenuClass.SpellTracker["allies"].Enabled)
                {
                    continue;
                }

                for (var spell = 0; spell < UtilityClass.SpellSlots.Length; spell++)
                {
                    var xSpellOffset = (int)hero.InfoBarPosition.X + UtilityClass.SpellXAdjustment(hero) + spell * 25;
                    var ySpellOffset = (int)hero.InfoBarPosition.Y + UtilityClass.SpellYAdjustment(hero);
                    var spellColor = UtilityClass.GetUnitSpellStateColor(hero, spell);
                    var spellCooldown = UtilityClass.GetUnitSpellCooldown(hero, spell);

                    TextRendering.Render(spellCooldown, Colors.GetRealColor(spellColor), new Vector2(xSpellOffset, ySpellOffset));

                    for (var level = 0; level <= hero.Spellbook.GetSpell(UtilityClass.SpellSlots[spell]).Level - 1; level++)
                    {
                        var xLevelOffset = xSpellOffset + level * 3 - 4;
                        var yLevelOffset = ySpellOffset + 4;

                        TextRendering.Render(".", Color.White, new Vector2(xLevelOffset, yLevelOffset));
                    }
                }

                for (var summonerSpell = 0; summonerSpell < UtilityClass.SummonerSpellSlots.Length; summonerSpell++)
                {
                    var xSummonerSpellOffset = (int)hero.InfoBarPosition.X-35 + UtilityClass.SummonerSpellXAdjustment(hero) + summonerSpell * 100;
                    var ySummonerSpellOffset = (int)hero.InfoBarPosition.Y + UtilityClass.SummonerSpellYAdjustment(hero);
                    var summonerSpellColor = UtilityClass.GetUnitSummonerSpellStateColor(hero, summonerSpell);
                    var summonerSpellCooldown = UtilityClass.GetUnitSummonerSpellFixedName(hero, summonerSpell) + ": " + UtilityClass.GetUnitSummonerSpellCooldown(hero, summonerSpell);

                    TextRendering.Render(summonerSpellCooldown, Colors.GetRealColor(summonerSpellColor), new Vector2(xSummonerSpellOffset, ySummonerSpellOffset));
                }
            }
        }

        #endregion
    }
}