
using System.Linq;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The menu class.
    /// </summary>
    internal partial class Quinn
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the menu.
        /// </summary>
        public void Menus()
        {
            /// <summary>
            ///     Sets the spells menu.
            /// </summary>
            MenuClass.Spells = new Menu("spells", "Spells");
            {
                /// <summary>
                ///     Sets the menu for the Q.
                /// </summary>
                MenuClass.Q = new Menu("q", "Use Q to:");
                {
                    MenuClass.Q.Add(new MenuBool("combo", "Combo"));
                    MenuClass.Q.Add(new MenuBool("killsteal", "Killsteal"));
                    MenuClass.Q.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.Q.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.Q.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the Q Whitelist.
                        /// </summary>
                        MenuClass.WhiteList = new Menu("whitelist", "Q Harass: Whitelist");
                        {
                            foreach (var target in GameObjects.EnemyHeroes)
                            {
                                MenuClass.WhiteList.Add(new MenuBool(target.ChampionName.ToLower(), "Harass: " + target.ChampionName));
                            }
                        }
                        MenuClass.Q.Add(MenuClass.WhiteList);
                    }
                    else
                    {
                        MenuClass.Q.Add(new MenuSeperator("exseparator", "Whitelist not needed"));
                    }

                    /// <summary>
                    ///     Sets the customization menu for the Q spell.
                    /// </summary>
                    MenuClass.Q2 = new Menu("customization", "Customization:");
                    {
                        //MenuClass.Q2.Add(new MenuSeperator("separator1", "Laneclear settings:"));
                        MenuClass.Q2.Add(new MenuSlider("laneclear", "Laneclear / if hittable minions >= x%", 4, 1, 10));
                    }
                    MenuClass.Q.Add(MenuClass.Q2);

                    /// <summary>
                    ///     Sets the modes menu for the Q spell.
                    /// </summary>
                    MenuClass.Q3 = new Menu("modes", "Modes:");
                    {
                        MenuClass.Q3.Add(new MenuList("combo", "Combo Mode:", new[] { "After Attack", "Always" }, 0));
                        MenuClass.Q3.Add(new MenuList("harass", "Harass Mode:", new[] { "Always" }, 0));
                    }
                    MenuClass.Q.Add(MenuClass.Q3);
                }
                MenuClass.Spells.Add(MenuClass.Q);

                /*
                /// <summary>
                ///     Sets the menu for the W.
                /// </summary>
                MenuClass.W = new Menu("w", "Use W to:");
                {
                    MenuClass.W.Add(new MenuBool("vision", "On Enemies entering bush"));
                }
                MenuClass.Spells.Add(MenuClass.W);
                */

                /// <summary>
                ///     Sets the menu for the E.
                /// </summary>
                MenuClass.E = new Menu("e", "Use E to:");
                {
                    MenuClass.E.Add(new MenuBool("combo", "Combo"));
                    MenuClass.E.Add(new MenuBool("killsteal", "Killsteal"));
                    MenuClass.E.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the E Whitelist.
                        /// </summary>
                        MenuClass.WhiteList2 = new Menu("whitelist", "E Harass: Whitelist");
                        {
                            foreach (var target in GameObjects.EnemyHeroes)
                            {
                                MenuClass.WhiteList2.Add(new MenuBool(target.ChampionName.ToLower(), "Harass: " + target.ChampionName));
                            }
                        }
                        MenuClass.E.Add(MenuClass.WhiteList2);
                    }
                    else
                    {
                        MenuClass.E.Add(new MenuSeperator("exseparator", "Whitelist not needed"));
                    }

                    if (GameObjects.EnemyHeroes.Any(x => Gapcloser.Spells.Any(spell => x.ChampionName == spell.ChampionName)))
                    {
                        /// <summary>
                        ///     Sets the menu for the Anti-Gapcloser E.
                        /// </summary>
                        MenuClass.Gapcloser = new Menu("gapcloser", "Anti-Gapcloser");
                        {
                            MenuClass.Gapcloser.Add(new MenuBool("enabled", "Enable"));
                            MenuClass.Gapcloser.Add(new MenuSeperator(string.Empty));
                            MenuClass.E.Add(MenuClass.Gapcloser);

                            foreach (var enemy in GameObjects.EnemyHeroes.Where(x => Gapcloser.Spells.Any(spell => x.ChampionName == spell.ChampionName)))
                            {
                                MenuClass.SubGapcloser = new Menu(enemy.ChampionName.ToLower(), enemy.ChampionName);
                                {
                                    foreach (var spell in Gapcloser.Spells.Where(x => x.ChampionName == enemy.ChampionName))
                                    {
                                        MenuClass.SubGapcloser.Add(new MenuBool(
                                            $"{enemy.ChampionName.ToLower()}.{spell.SpellName.ToLower()}",
                                            $"Slot: {spell.Slot} ({spell.SpellName})"));
                                    }
                                }
                                MenuClass.Gapcloser.Add(MenuClass.SubGapcloser);
                            }
                        }
                    }
                    else
                    {
                        MenuClass.E.Add(new MenuSeperator(string.Empty, "Anti-Gapcloser not needed"));
                    }
                }
                MenuClass.Spells.Add(MenuClass.E);

                /// <summary>
                ///     Sets the menu for the R.
                /// </summary>
                MenuClass.R = new Menu("r", "Use R to:");
                {
                    MenuClass.R.Add(new MenuBool("logical", "Automatically R in base"));
                }
                MenuClass.Spells.Add(MenuClass.R);
            }
            MenuClass.Root.Add(MenuClass.Spells);

            /// <summary>
            ///     Sets the miscellaneous menu.
            /// </summary>
            MenuClass.Miscellaneous = new Menu("miscellaneous", "Miscellaneous");
            {
                MenuClass.Miscellaneous.Add(new MenuBool("focuspassive", "Focus enemies with Passive mark"));
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
            }
            MenuClass.Root.Add(MenuClass.Drawings);
        }

        #endregion
    }
}