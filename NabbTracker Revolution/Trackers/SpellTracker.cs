using System;
using System.Drawing;
using System.Linq;
using Entropy;
using Entropy.SDK.Menu.Components;

namespace NabbTracker
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
            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(h =>
                !h.IsDead &&
                h.IsVisible &&
                Math.Abs(h.FloatingHealthBarPosition.X) > 0))
            {
                if (hero.Name.Equals("Target Dummy"))
                {
                    continue;
                }

                if (hero.IsMe &&
                    !MenuClass.SpellTracker["me"].As<MenuBool>().Enabled)
                {
                    continue;
                }

                if (hero.IsEnemy &&
                    !MenuClass.SpellTracker["enemies"].As<MenuBool>().Enabled)
                {
                    continue;
                }

                if (!hero.IsMe &&
                    hero.IsAlly &&
                    !MenuClass.SpellTracker["allies"].As<MenuBool>().Enabled)
                {
                    continue;
                }

                for (var spell = 0; spell < UtilityClass.SpellSlots.Length; spell++)
                {
                    var xSpellOffset = (int)hero.FloatingHealthBarPosition.X + UtilityClass.SpellXAdjustment(hero) + spell * 25;
                    var ySpellOffset = (int)hero.FloatingHealthBarPosition.Y + UtilityClass.SpellYAdjustment(hero);
                    var spellColor = UtilityClass.GetUnitSpellStateColor(hero, spell);
                    var spellCooldown = UtilityClass.GetUnitSpellCooldown(hero, spell);

                    Render.Text(spellCooldown, new Vector2(xSpellOffset, ySpellOffset), RenderTextFlags.None, Colors.GetRealColor(spellColor));

                    for (var level = 0; level <= hero.SpellBook.GetSpell(UtilityClass.SpellSlots[spell]).Level - 1; level++)
                    {
                        var xLevelOffset = xSpellOffset + level * 3 - 4;
                        var yLevelOffset = ySpellOffset + 4;

                        Render.Text(".", new Vector2(xLevelOffset, yLevelOffset), RenderTextFlags.None, Color.White);
                    }
                }

                for (var summonerSpell = 0; summonerSpell < UtilityClass.SummonerSpellSlots.Length; summonerSpell++)
                {
                    var xSummonerSpellOffset = (int)hero.FloatingHealthBarPosition.X-35 + UtilityClass.SummonerSpellXAdjustment(hero) + summonerSpell * 100;
                    var ySummonerSpellOffset = (int)hero.FloatingHealthBarPosition.Y + UtilityClass.SummonerSpellYAdjustment(hero);
                    var summonerSpellColor = UtilityClass.GetUnitSummonerSpellStateColor(hero, summonerSpell);
                    var summonerSpellCooldown = UtilityClass.GetUnitSummonerSpellFixedName(hero, summonerSpell) + ": " + UtilityClass.GetUnitSummonerSpellCooldown(hero, summonerSpell);

                    Render.Text(summonerSpellCooldown, new Vector2(xSummonerSpellOffset, ySummonerSpellOffset), RenderTextFlags.None, Colors.GetRealColor(summonerSpellColor));
                }
            }
        }

        #endregion
    }
}