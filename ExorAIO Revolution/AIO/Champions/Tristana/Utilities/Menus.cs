
using System.Linq;
using AIO.Utilities;
using Entropy.SDK.UI;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The menu class.
    /// </summary>
    internal partial class Tristana
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
                    MenuClass.Q.Add(new MenuBool("harass", "Harass"));
                    MenuClass.Q.Add(new MenuBool("buildings", "To buildings"));
                    MenuClass.Q.Add(new MenuBool("laneclear", "Laneclear"));
                    MenuClass.Q.Add(new MenuBool("jungleclear", "Jungleclear"));
                }
                MenuClass.Spells.Add(MenuClass.Q);

                /// <summary>
                ///     Sets the menu for the W.
                /// </summary>
                MenuClass.W = new Menu("w", "Use W to:");
                {
                    MenuClass.W.Add(new MenuBool("antigrab", "Anti-Grab"));
                    MenuClass.W.Add(new MenuSeperator("separator"));

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the Anti-Gapcloser W.
                        /// </summary>
                        MenuClass.Gapcloser = new Menu("gapcloser", "Anti-Gapcloser");
                        {
                            if (GameObjects.EnemyHeroes.Any(x => x.IsMelee && Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
                            {
                                MenuClass.Gapcloser.Add(new MenuBool("enabled", "Enable"));
                                MenuClass.Gapcloser.Add(new MenuSeperator(string.Empty));
                                MenuClass.W.Add(MenuClass.Gapcloser);

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
                            else
                            {
                                MenuClass.Gapcloser.Add(new MenuSeperator(string.Empty, "Anti-Gapcloser not needed"));
                            }
                        }
                    }
                    else
                    {
                        MenuClass.W.Add(new MenuSeperator(string.Empty, "Anti-Gapcloser not needed"));
                    }
                }
                MenuClass.Spells.Add(MenuClass.W);

                /// <summary>
                ///     Sets the menu for the E.
                /// </summary>
                MenuClass.E = new Menu("e", "Use E to:");
                {
                    MenuClass.E.Add(new MenuBool("combo", "Combo"));
                    MenuClass.E.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.E.Add(new MenuSliderBool("buildings", "To buildings / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.E.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.E.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

                    /// <summary>
                    ///     Sets the customization menu for the E spell.
                    /// </summary>
                    MenuClass.E2 = new Menu("customization", "Customization:");
                    {
                        //MenuClass.E2.Add(new MenuSeperator("separator1", "Laneclear settings:"));
                        MenuClass.E2.Add(new MenuSlider("laneclear", "Only Laneclear if Minions around target >= x%", 3, 1, 10));
                    }
                    MenuClass.E.Add(MenuClass.E2);

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the Q Whitelist.
                        /// </summary>
                        MenuClass.WhiteList = new Menu("whitelist", "E: Whitelist");
                        {
                            foreach (var target in GameObjects.EnemyHeroes)
                            {
                                MenuClass.WhiteList.Add(new MenuBool(target.CharName.ToLower(), "Use on: " + target.CharName));
                            }
                        }
                        MenuClass.E.Add(MenuClass.WhiteList);
                    }
                    else
                    {
                        MenuClass.E.Add(new MenuSeperator("exseparator", "Whitelist not needed"));
                    }
                }
                MenuClass.Spells.Add(MenuClass.E);

                /// <summary>
                ///     Sets the menu for the R.
                /// </summary>
                MenuClass.R = new Menu("r", "Use R to:");
                {
                    MenuClass.R.Add(new MenuBool("killsteal", "KillSteal"));
                    MenuClass.R.Add(new MenuSeperator("separator"));

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the Anti-Gapcloser R.
                        /// </summary>
                        MenuClass.Gapcloser2 = new Menu("gapcloser", "Anti-Gapcloser");
                        {
                            if (GameObjects.EnemyHeroes.Any(x => x.IsMelee && Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
                            {
                                MenuClass.Gapcloser2.Add(new MenuBool("enabled", "Enable"));
                                MenuClass.Gapcloser2.Add(new MenuSeperator(string.Empty));
                                MenuClass.R.Add(MenuClass.Gapcloser2);

                                foreach (var enemy in GameObjects.EnemyHeroes.Where(x => x.IsMelee && Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
                                {
                                    MenuClass.SubGapcloser2 = new Menu(enemy.CharName.ToLower(), enemy.CharName);
                                    {
                                        foreach (var spell in Gapcloser.Spells.Where(x => x.ChampionName == enemy.CharName))
                                        {
                                            MenuClass.SubGapcloser2.Add(new MenuBool(
                                                $"{enemy.CharName.ToLower()}.{spell.SpellName.ToLower()}",
                                                $"Slot: {spell.Slot} ({spell.SpellName})"));
                                        }
                                    }
                                    MenuClass.Gapcloser2.Add(MenuClass.SubGapcloser2);
                                }
                            }
                            else
                            {
                                MenuClass.Gapcloser2.Add(new MenuSeperator(string.Empty, "Anti-Gapcloser not needed"));
                            }
                        }
                    }
                    else
                    {
                        MenuClass.R.Add(new MenuSeperator(string.Empty, "Anti-Gapcloser not needed"));
                    }

                    MenuClass.R.Add(new MenuSeperator("separator2"));
                    MenuClass.R.Add(new MenuBool("interrupter", "Interrupt Enemy Channels"));
                    MenuClass.R.Add(new MenuSeperator("separator3"));
                    MenuClass.R.Add(new MenuBool("bool", "Semi-Automatic R"));
                    MenuClass.R.Add(new MenuKeyBind("key", "Key:", KeyCode.T, KeybindType.Press));

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the R Whitelist.
                        /// </summary>
                        MenuClass.WhiteList2 = new Menu("whitelist", "R: Whitelist");
                        {
                            foreach (var target in GameObjects.EnemyHeroes)
                            {
                                MenuClass.WhiteList2.Add(new MenuBool(target.CharName.ToLower(), "Use on: " + target.CharName));
                            }
                        }
                        MenuClass.R.Add(MenuClass.WhiteList2);
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
                MenuClass.Miscellaneous.Add(new MenuBool("focuse", "Focus E-charged enemies"));
            }
            MenuClass.Root.Add(MenuClass.Miscellaneous);

            /// <summary>
            ///     Sets the drawings menu.
            /// </summary>
            MenuClass.Drawings = new Menu("drawings", "Drawings");
            {
                MenuClass.Drawings.Add(new MenuBool("edmg", "E Damage"));
                MenuClass.Drawings.Add(new MenuBool("w", "W Range", false));
            }
            MenuClass.Root.Add(MenuClass.Drawings);
        }

        #endregion
    }
}