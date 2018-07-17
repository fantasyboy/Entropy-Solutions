using System.Linq;
using AIO.Utilities;
using Entropy.SDK.Caching;
using Entropy.SDK.UI;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The menu class.
    /// </summary>
    internal partial class Kalista
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
                MenuClass.Q.Add(new MenuBool("combo", "Combo"));
                MenuClass.Q.Add(new MenuBool("killsteal", "KillSteal"));
                MenuClass.Q.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
                MenuClass.Q.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
                MenuClass.Q.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

                /// <summary>
                ///     Sets the customization menu for the Q spell.
                /// </summary>
                MenuClass.Q2 = new Menu("customization", "Customization:");
                {
                    //MenuClass.Q2.Add(new MenuSeperator("separator1", "Laneclear settings:"));
                    MenuClass.Q2.Add(new MenuSlider("laneclear", "Only Laneclear if killable minions >= x%", 3, 1, 10));
                }
                MenuClass.Q.Add(MenuClass.Q2);

                if (ObjectCache.EnemyHeroes.Any())
                {
                    /// <summary>
                    ///     Sets the menu for the Q Harass Whitelist.
                    /// </summary>
                    MenuClass.WhiteList = new Menu("whitelist", "Harass: Whitelist");
                    {
                        foreach (var target in ObjectCache.EnemyHeroes)
                        {
                            MenuClass.WhiteList.Add(new MenuBool(target.CharName.ToLower(), "Harass: " + target.CharName));
                        }
                    }
                    MenuClass.Q.Add(MenuClass.WhiteList);
                }
                else
                {
                    MenuClass.Q.Add(new MenuSeperator("exseparator", "Whitelist not needed"));
                }
            }
            MenuClass.Root.Add(MenuClass.Q);

            /// <summary>
            ///     Sets the menu for the W.
            /// </summary>
            MenuClass.W = new Menu("w", "Use W to:");
            {
                MenuClass.W.Add(new MenuSliderBool("logical", "Check important Jungle spots / if Mana >= x%", true, 50, 0, 99));
            }
            MenuClass.Root.Add(MenuClass.W);

            /// <summary>
            ///     Sets the menu for the E.
            /// </summary>
            MenuClass.E = new Menu("e", "Use E to:");
            {
                /// <summary>
                ///     Sets the menu for the E Whitelist.
                /// </summary>
                MenuClass.WhiteList2 = new Menu("whitelist", "Junglesteal Rend: Whitelist");
                {
                    foreach (var target in UtilityClass.JungleList)
                    {
                        MenuClass.WhiteList2.Add(new MenuBool(target, "Rend: " + target));
                    }
                }
                MenuClass.E.Add(MenuClass.WhiteList2);

                MenuClass.E.Add(new MenuBool("killsteal", "KillSteal"));
                MenuClass.E.Add(new MenuSliderBool("beforedeath", "Before death", true, 10, 1, 15));
                MenuClass.E.Add(new MenuSeperator("separator"));
                MenuClass.E.Add(new MenuBool("harass", "Harass with minions"));
                MenuClass.E.Add(new MenuBool("dontharassslowed", "^ Don't Harass already slowed enemies"));
                MenuClass.E.Add(new MenuSeperator("separator2"));
                MenuClass.E.Add(new MenuBool("lasthit", "Lasthit"));
                MenuClass.E.Add(new MenuSliderBool("laneclear", "Laneclear / If killable minions >= x", true, 1, 1, 5));
                MenuClass.E.Add(new MenuSliderBool("junglesteal", "Junglesteal / If Level >=", true, 2, 1, 18));
                //MenuClass.E.Add(new MenuSeperator("separator"));
                //MenuClass.E.Add(new MenuSeperator("separator1", "It will cast E if there are any minions with"));
                //MenuClass.E.Add(new MenuSeperator("separator2", "stacks the Orbwalker cannot reach in time to kill them."));
            }
            MenuClass.Root.Add(MenuClass.E);

            /// <summary>
            ///     Sets the menu for the R Whitelist.
            /// </summary>
            MenuClass.R = new Menu("r", "Use R to:");
            {
                foreach (var element in RLogics)
                {
                    if (ObjectCache.AllyHeroes.Any(a => a.CharName == element.Key))
                    {
                        MenuClass.R.Add(new MenuBool(element.Value.Item2, $"Use {element.Value.Item2}"));
                    }
                    else
                    {
                        MenuClass.R.Add(new MenuSeperator(element.Value.Item2, $"Ally {element.Key} not found, no need for a {element.Value.Item2} option."));
                    }
                }

                MenuClass.R.Add(new MenuSliderBool("lifesaver", "Soulbound Lifesaver / If Soulbound Health <= X%", true, 15, 10, 90));
            }
            MenuClass.Root.Add(MenuClass.R);

            /// <summary>
            ///     Sets the miscellaneous menu.
            /// </summary>
            MenuClass.Miscellaneous = new Menu("miscellaneous", "Miscellaneous");
            {
                MenuClass.Miscellaneous.Add(new MenuBool("focusw", "Focus enemies to proc W Passive mark"));
                MenuClass.Miscellaneous.Add(new MenuBool("minionsorbwalk", "Orbwalk on Minions in Combo"));
            }
            MenuClass.Root.Add(MenuClass.Miscellaneous);

            /// <summary>
            ///     Sets the drawings menu.
            /// </summary>
            MenuClass.Drawings = new Menu("drawings", "Drawings");
            {
                MenuClass.Drawings.Add(new MenuBool("q", "Q Range", false));
                MenuClass.Drawings.Add(new MenuBool("w", "W Range", false));
                MenuClass.Drawings.Add(new MenuBool("e", "E Range", false));
                MenuClass.Drawings.Add(new MenuBool("edmg", "E Damage"));
                MenuClass.Drawings.Add(new MenuBool("r", "R Range", false));
                MenuClass.Drawings.Add(new MenuSliderBool("soulbound", "Soulbound / Draw X circles", true, 2, 1, 5));
            }
            MenuClass.Root.Add(MenuClass.Drawings);
		}

		#endregion
	}
}