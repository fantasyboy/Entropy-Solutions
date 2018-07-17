using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Caching;
using Entropy.SDK.UI;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The menu class.
    /// </summary>
    internal partial class Kaisa
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the menu.
        /// </summary>
        public void Menus()
        {
            /// <summary>
            ///     Sets the menu for the Q.
            /// </summary>
            MenuClass.Q = new Menu("q", "Use Q to:");
            {
                MenuClass.Q.Add(new MenuSliderBool("combo", "Combo / If can Hit >= x enemies", true, 1, 1, 6));
                MenuClass.Q.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
				MenuClass.Q.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
                MenuClass.Q.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));
				MenuClass.Q.Add(new MenuSliderBool("nonkillable", "On Non-Killable Minion / if Mana >= x%", true, 50, 0, 99));
			}
            MenuClass.Root.Add(MenuClass.Q);

            /// <summary>
            ///     Sets the menu for the W.
            /// </summary>
            MenuClass.W = new Menu("w", "Use W to:");
            {
	            MenuClass.W.Add(new MenuSliderBool("combo", "Combo / If Stacks on enemy >= x", true, 0, 0, 4));
	            MenuClass.W.Add(new MenuBool("killsteal", "KillSteal"));
	            MenuClass.W.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
				MenuClass.W.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

	            if (ObjectCache.EnemyHeroes.Any())
	            {
		            /// <summary>
		            ///     Sets the menu for the W Harass Whitelist.
		            /// </summary>
		            MenuClass.WhiteList = new Menu("whitelist", "Harass: Whitelist");
		            {
			            foreach (var target in ObjectCache.EnemyHeroes)
			            {
				            MenuClass.WhiteList.Add(new MenuBool(target.CharName.ToLower(), "Harass: " + target.CharName));
			            }
		            }
		            MenuClass.W.Add(MenuClass.WhiteList);
	            }
	            else
	            {
		            MenuClass.W.Add(new MenuSeperator("exseparator", "Whitelist not needed"));
	            }
			}
            MenuClass.Root.Add(MenuClass.W);

            /// <summary>
            ///     Sets the menu for the E.
            /// </summary>
            MenuClass.E = new Menu("e", "Use E to:");
            {
	            MenuClass.E.Add(new MenuBool("combo", "Combo"));
	            MenuClass.E.Add(new MenuBool("engage", "Engager")).SetToolTip("Casts if enemy gets out of AA range");
	            MenuClass.E.Add(new MenuSeperator("separator"));

	            if (GameObjects.EnemyHeroes.Any(x => x.IsMelee && Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
	            {
		            /// <summary>
		            ///     Sets the menu for the Anti-Gapcloser E.
		            /// </summary>
		            MenuClass.Gapcloser = new Menu("gapcloser", "Anti-Gapcloser");
		            {
			            MenuClass.Gapcloser.Add(new MenuBool("enabled", "Enable"));
			            MenuClass.Gapcloser.Add(new MenuSeperator(string.Empty));
			            MenuClass.E.Add(MenuClass.Gapcloser);

			            foreach (var enemy in GameObjects.EnemyHeroes.Where(x => x.IsMelee && Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
			            {
				            MenuClass.SubGapcloser = new Menu(enemy.CharName.ToLower(), enemy.CharName);
				            {
					            foreach (var spell in Gapcloser.Spells.Where(x => x.ChampionName == enemy.CharName))
					            {
						            MenuClass.SubGapcloser.Add(new MenuBool(
							            $"{enemy.CharName.ToLower()}.{spell.SpellName.ToLower()}",
							            $"Slot: {spell.Slot} ({spell.SpellName})"));
					            }
				            }
				            MenuClass.Gapcloser.Add(MenuClass.SubGapcloser);
			            }
		            }
	            }
	            else
	            {
		            MenuClass.E.Add(new MenuSeperator("antigapclosernotneeded", "Anti-Gapcloser not needed"));
	            }

	            MenuClass.E.Add(new MenuSeperator("separator2"));
				MenuClass.E.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
				MenuClass.E.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));
            }
            MenuClass.Root.Add(MenuClass.E);

            /// <summary>
            ///     Sets the menu for the R.
            /// </summary>
			MenuClass.R = new Menu("r", "Use R to:");
            {
	            MenuClass.R.Add(new MenuBool("bool", "Semi-Automatic R"));
	            MenuClass.R.Add(new MenuKeyBind("key", "Key:", WindowMessageWParam.U, KeybindType.Hold));
			}
            MenuClass.Root.Add(MenuClass.R);

            /// <summary>
            ///     Sets the miscellaneous menu.
            /// </summary>
            MenuClass.Miscellaneous = new Menu("miscellaneous", "Miscellaneous");
            {
                MenuClass.Miscellaneous.Add(new MenuBool("focusmark", "Focus enemies with Passive stacks"));
            }
            MenuClass.Root.Add(MenuClass.Miscellaneous);

            /// <summary>
            ///     Sets the drawings menu.
            /// </summary>
            MenuClass.Drawings = new Menu("drawings", "Drawings");
            {
	            MenuClass.Drawings.Add(new MenuBool("passivedmg", "Passive Damage"));
				MenuClass.Drawings.Add(new MenuBool("w", "W Range"));
				MenuClass.Drawings.Add(new MenuBool("r", "R Range"));
            }
            MenuClass.Root.Add(MenuClass.Drawings);
        }

        #endregion
    }
}