
using System.Linq;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Util;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The menu class.
    /// </summary>
    internal partial class Ezreal
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
                    //MenuClass.Q.Add(new MenuSeperator("separator"));
                    //MenuClass.Q.Add(new MenuSeperator("separator1", "It will cast Q on the minions the"));
                    //MenuClass.Q.Add(new MenuSeperator("separator2", "orbwalker cannot reach in time to kill."));
                    MenuClass.Q.Add(new MenuSliderBool("farmhelper", "FarmHelper / if Mana >= x%", true, 50, 0, 99));

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
                }
                MenuClass.Spells.Add(MenuClass.Q);

                /// <summary>
                ///     Sets the menu for the W.
                /// </summary>
                MenuClass.W = new Menu("w", "Use W to:");
                {
                    MenuClass.W.Add(new MenuSliderBool("apmode", "Enable AP Mode / if AP >= x", true, 35, 0, 250));
                    MenuClass.W.Add(new MenuSeperator("separator"));
                    MenuClass.W.Add(new MenuBool("combo", "Combo"));
                    MenuClass.W.Add(new MenuBool("killsteal", "Killsteal"));
                    MenuClass.W.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
                    //MenuClass.W.Add(new MenuSeperator("separator"));
                    //MenuClass.W.Add(new MenuSeperator("separator1", "If enabled, it will cast W on allies while"));
                    //MenuClass.W.Add(new MenuSeperator("separator2", "Sieging Towers, Inhibitors and Nexus"));
                    //MenuClass.W.Add(new MenuSeperator("separator3", "or doing Dragon, Baron or Herald"));
                    //MenuClass.W.Add(new MenuSeperator("separator4", "or during a Teamfight"));

                    if (GameObjects.AllyHeroes.Any(a => !a.IsMe))
                    {
                        MenuClass.Buff = new Menu("buff", "W Buff Logic Settings");
                        {
                            MenuClass.Buff.Add(new MenuSliderBool("logical", "Buff Allies (Not in AP Mode) / if Mana >= x%", true, 50, 0, 99));

                            /// <summary>
                            ///     Sets the menu for the W Buff Mode Whitelist.
                            /// </summary>
                            MenuClass.WhiteList2 = new Menu("orbwhitelist", "W Buff: Orbwalking Mode Whitelist");
                            {
                                MenuClass.WhiteList2.Add(new MenuBool("combo", "Use in: Combo (Ally attacking Enemy)"));
                                MenuClass.WhiteList2.Add(new MenuBool("harass", "Use in: Harass (Ally attacking Enemy)"));
                                MenuClass.WhiteList2.Add(new MenuBool("laneclear", "Use in: Laneclear (Ally attacking Objectives)"));
                            }
                            MenuClass.Buff.Add(MenuClass.WhiteList2);

                            /// <summary>
                            ///     Sets the menu for the W Buff Ally Whitelist.
                            /// </summary>
                            MenuClass.WhiteList3 = new Menu("allywhitelist", "W Buff: Ally Whitelist");
                            {
                                foreach (var target in GameObjects.AllyHeroes.Where(a => !a.IsMe))
                                {
                                    MenuClass.WhiteList3.Add(new MenuBool(target.ChampionName.ToLower(), "Buff: " + target.ChampionName));
                                }
                            }
                            MenuClass.Buff.Add(MenuClass.WhiteList3);
                        }
                        MenuClass.W.Add(MenuClass.Buff);
                    }
                    else
                    {
                        MenuClass.W.Add(new MenuSeperator("exseparator", "No allies found, no need for a Buff Logic Settings Menu."));
                    }

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the W Harass Whitelist.
                        /// </summary>
                        MenuClass.WhiteList4 = new Menu("whitelist", "W Harass: Whitelist");
                        {
                            foreach (var target in GameObjects.EnemyHeroes)
                            {
                                MenuClass.WhiteList4.Add(new MenuBool(target.ChampionName.ToLower(), "Harass: " + target.ChampionName));
                            }
                        }
                        MenuClass.W.Add(MenuClass.WhiteList4);
                    }
                    else
                    {
                        MenuClass.W.Add(new MenuSeperator("exseparator2", "Whitelist not needed"));
                    }
                }
                MenuClass.Spells.Add(MenuClass.W);

                /// <summary>
                ///     Sets the menu for the E.
                /// </summary>
                MenuClass.E = new Menu("e", "Use E to:");
                {
                    MenuClass.E.Add(new MenuBool("antigrab", "Anti-Grab"));
                    MenuClass.E.Add(new MenuSeperator("separator"));

                    if (GameObjects.EnemyHeroes.Any(x => x.IsMelee && Gapcloser.Spells.Any(spell => x.ChampionName == spell.ChampionName)))
                    {
                        /// <summary>
                        ///     Sets the menu for the Anti-Gapcloser E.
                        /// </summary>
                        MenuClass.Gapcloser = new Menu("gapcloser", "Anti-Gapcloser");
                        {
                            MenuClass.Gapcloser.Add(new MenuBool("enabled", "Enable"));
                            MenuClass.Gapcloser.Add(new MenuSeperator(string.Empty));
                            MenuClass.E.Add(MenuClass.Gapcloser);

                            foreach (var enemy in GameObjects.EnemyHeroes.Where(x => x.IsMelee && Gapcloser.Spells.Any(spell => x.ChampionName == spell.ChampionName)))
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
                    //MenuClass.R.Add(new MenuSeperator("separator1", "It will ult the lowest on health,"));
                    //MenuClass.R.Add(new MenuSeperator("separator2", "whitelisted and non-invulnerable enemy in range."));
                    MenuClass.R.Add(new MenuBool("bool", "Semi-Automatic R"));
                    MenuClass.R.Add(new MenuKeyBind("key", "Key:", KeyCode.T, KeybindType.Press));

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the R Whitelist.
                        /// </summary>
                        MenuClass.WhiteList5 = new Menu("whitelist", "Ultimate: Whitelist");
                        {
                            foreach (var target in GameObjects.EnemyHeroes)
                            {
                                MenuClass.WhiteList5.Add(new MenuBool(target.ChampionName.ToLower(), "Use against: " + target.ChampionName));
                            }
                        }
                        MenuClass.R.Add(MenuClass.WhiteList5);
                    }
                    else
                    {
                        MenuClass.R.Add(new MenuSeperator("exseparator", "Whitelist not needed"));
                    }
                }
                MenuClass.Spells.Add(MenuClass.R);
            }
            MenuClass.Root.Add(MenuClass.Spells);

            /// <summary>
            ///     Sets the miscellaneous menu.
            /// </summary>
            MenuClass.Miscellaneous = new Menu("miscellaneous", "Miscellaneous");
            {
                MenuClass.Miscellaneous.Add(new MenuSliderBool("tear", "Stack Tear / if Mana >= x%", true, 80, 1, 99));
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