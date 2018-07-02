
using System.Linq;
using Entropy.SDK.Menu;
using Entropy.SDK.Menu.Components;
using Entropy.SDK.Util;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The menu class.
    /// </summary>
    internal partial class Jinx
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
                    MenuClass.Q.Add(new MenuBool("combo", "Use Fishbones if target out of PowPow range"));
                    //MenuClass.Q.Add(new MenuSeperator("separator2"));
                    MenuClass.Q.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.Q.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.Q.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

                    /// <summary>
                    ///     Sets the menu for the Q customization.
                    /// </summary>
                    MenuClass.Q2 = new Menu("customization", "Fishbones Customization:");
                    {
                        //MenuClass.Q2.Add(new MenuSeperator("separator1", "General settings:"));
                        //MenuClass.Q2.Add(new MenuSeperator("separator2"));
                        //MenuClass.Q2.Add(new MenuSeperator("separator3", "Combo settings:"));
                        //MenuClass.Q2.Add(new MenuSeperator("separator4", "This option will also be valid for the PowPow range."));
                        if (GameObjects.EnemyHeroes.Count() >= 2)
                        {
                            MenuClass.Q2.Add(new MenuSlider("minenemies", "Use Fishbones if hittable enemies >= x", 3, 2, GameObjects.EnemyHeroes.Count()));
                        }
                        else
                        {
                            MenuClass.Q2.Add(new MenuSeperator("separator", "Use Fishbones / Less than 2 Enemies in the game."));
                        }
                        //MenuClass.Q2.Add(new MenuSeperator("separator5"));
                        //MenuClass.Q2.Add(new MenuSeperator("separator6", "Laneclear settings:"));
                        MenuClass.Q2.Add(new MenuSlider("laneclear", "Laneclear: Fishbones if Hittable minions >= x", 3, 2, 5));
                        //MenuClass.Q2.Add(new MenuSeperator("separator7"));
                        //MenuClass.Q2.Add(new MenuSeperator("separator8", "Jungleclear settings:"));
                        MenuClass.Q2.Add(new MenuSlider("jungleclear", "Jungleclear: Fishbones if Hittable minions >= x", 2, 1, 5));
                    }
                    MenuClass.Q.Add(MenuClass.Q2);

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the Q Whitelist.
                        /// </summary>
                        MenuClass.WhiteList = new Menu("whitelist", "Harass: Whitelist");
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
                    MenuClass.W.Add(new MenuList("mode", "W Cast Mode", new[] { "out of PowPow range", "out of Fishbones range", "Don't use W in Combo" }, 0));
                    MenuClass.W.Add(new MenuBool("killsteal", "KillSteal"));
                    MenuClass.W.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.W.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

                    /// <summary>
                    ///     Sets the menu for the Q customization.
                    /// </summary>
                    MenuClass.W2 = new Menu("customization", "Fishbones Customization:");
                    {
                        if (GameObjects.EnemyHeroes.Any())
                        {
                            var count = GameObjects.EnemyHeroes.Count();
                            MenuClass.W2.Add(new MenuSliderBool("wsafetycheck", "W only if enemies in Fishbones Range <= x", true, count >= 2 ? 2 : 1, 0, GameObjects.EnemyHeroes.Count()));
                        }
                        else
                        {
                            MenuClass.W2.Add(new MenuSeperator("wsafetycheck", "W safety check not needed"));
                        }
                    }
                    MenuClass.W.Add(MenuClass.W2);

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the W Whitelist.
                        /// </summary>
                        MenuClass.WhiteList2 = new Menu("whitelist", "Harass: Whitelist");
                        {
                            foreach (var target in GameObjects.EnemyHeroes)
                            {
                                MenuClass.WhiteList2.Add(new MenuBool(target.ChampionName.ToLower(), "Harass: " + target.ChampionName));
                            }
                        }
                        MenuClass.W.Add(MenuClass.WhiteList2);
                    }
                    else
                    {
                        MenuClass.W.Add(new MenuSeperator("exseparator", "Whitelist not needed"));
                    }
                }
                MenuClass.Spells.Add(MenuClass.W);

                /// <summary>
                ///     Sets the menu for the E.
                /// </summary>
                MenuClass.E = new Menu("e", "Use E to:");
                {
                    if (GameObjects.EnemyHeroes.Count() >= 2)
                    {
                        MenuClass.E.Add(new MenuSliderBool("aoe", "AoE / If can hit >= x enemies", false, 3, 2, GameObjects.EnemyHeroes.Count()));
                    }
                    else
                    {
                        MenuClass.E.Add(new MenuSeperator("separator", "AoE / Not enough enemies found."));
                    }
                    MenuClass.E.Add(new MenuBool("logical", "On Hard-CC'd/Stasis Enemies"));
                    MenuClass.E.Add(new MenuBool("teleport", "On Teleport"));
                    MenuClass.E.Add(new MenuSeperator("separator2"));

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

                    MenuClass.E.Add(new MenuSeperator("separator3"));
                    MenuClass.E.Add(new MenuBool("interrupter", "On Channelling Immobile Targets"));
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
                        MenuClass.WhiteList3 = new Menu("whitelist", "Ultimate: Whitelist");
                        {
                            foreach (var target in GameObjects.EnemyHeroes)
                            {
                                MenuClass.WhiteList3.Add(new MenuBool(target.ChampionName.ToLower(), "Use against: " + target.ChampionName));
                            }
                        }
                        MenuClass.R.Add(MenuClass.WhiteList3);
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
                MenuClass.Miscellaneous.Add(new MenuBool("forcelasthit", "Force PowPow switch in LastHit mode"));
                MenuClass.Miscellaneous.Add(new MenuBool("forcepowpow", "Force PowPow switch after switching Orbwalking Mode"));
            }
            MenuClass.Root.Add(MenuClass.Miscellaneous);

            /// <summary>
            ///     Sets the drawings menu.
            /// </summary>
            MenuClass.Drawings = new Menu("drawings", "Drawings");
            {
                MenuClass.Drawings.Add(new MenuBool("q", "Q Range"));
                MenuClass.Drawings.Add(new MenuBool("w", "W Range", false));
                MenuClass.Drawings.Add(new MenuBool("e", "E Range", false));
            }
            MenuClass.Root.Add(MenuClass.Drawings);
        }

        #endregion
    }
}