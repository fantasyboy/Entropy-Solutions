
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
    internal partial class Varus
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the menus.
        /// </summary>
        public void Menus()
        {
            /// <summary>
            ///     Sets the menu for the spells.
            /// </summary>
            MenuClass.Spells = new Menu("spells", "Spells");
            {
                /// <summary>
                ///     Sets the menu for the Q.
                /// </summary>
                MenuClass.Q = new Menu("q", "Use Q to:");
                {
                    MenuClass.Q.Add(new MenuBool("combo", "Combo"));
                    MenuClass.Q.Add(new MenuBool("combolong", "Long Range Combo", false));
                    MenuClass.Q.Add(new MenuBool("killsteal", "KillSteal"));
                    MenuClass.Q.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.Q.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.Q.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));
                }
                MenuClass.Spells.Add(MenuClass.Q);

                /// <summary>
                ///     Sets the customization menu for the Q spell.
                /// </summary>
                MenuClass.Q2 = new Menu("customization", "Customization:");
                {
                    MenuClass.Q2.Add(new MenuSlider("combostacks", "Combo / if Blight Stacks >= x", 3, 0, 3));
                    MenuClass.Q2.Add(new MenuSlider("harassstacks", "Harass / if Blight Stacks >= x", 3, 0, 3));
                    MenuClass.Q2.Add(new MenuSlider("laneclear", "Laneclear / if hittable minions >= x%", 4, 1, 10));
                }
                MenuClass.Q.Add(MenuClass.Q2);

                if (GameObjects.EnemyHeroes.Any())
                {
                    /// <summary>
                    ///     Sets the menu for the Q Harass Whitelist.
                    /// </summary>
                    MenuClass.WhiteList = new Menu("whitelist", "Harass: Whitelist");
                    {
                        foreach (var target in GameObjects.EnemyHeroes)
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

                /// <summary>
                ///     Sets the menu for the W.
                /// </summary>
                MenuClass.W = new Menu("w", "Use W to:");
                {
                    MenuClass.W.Add(new MenuBool("combo", "Combo"));
                    MenuClass.W.Add(new MenuBool("killsteal", "KillSteal"));
                    MenuClass.W.Add(new MenuBool("harass", "Harass"));
                    MenuClass.W.Add(new MenuBool("laneclear", "Laneclear"));
                    MenuClass.W.Add(new MenuBool("jungleclear", "Jungleclear"));
                }
                MenuClass.Spells.Add(MenuClass.W);

                /// <summary>
                ///     Sets the menu for the E.
                /// </summary>
                MenuClass.E = new Menu("e", "Use E to:");
                {
                    MenuClass.E.Add(new MenuBool("combo", "Combo"));
                    MenuClass.E.Add(new MenuBool("killsteal", "KillSteal"));
                    MenuClass.E.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.E.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.E.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));
                }
                MenuClass.Spells.Add(MenuClass.E);

                /// <summary>
                ///     Sets the customization menu for the E spell.
                /// </summary>
                MenuClass.E2 = new Menu("customization", "Customization:");
                {
                    MenuClass.E2.Add(new MenuSlider("combostacks", "Combo / if Blight Stacks >= x", 3, 0, 3));
                    MenuClass.E2.Add(new MenuSlider("harassstacks", "Harass / if Blight Stacks >= x", 3, 0, 3));
                    MenuClass.E2.Add(new MenuSlider("laneclear", "Laneclear / if hittable minions >= x%", 4, 1, 10));
                }
                MenuClass.E.Add(MenuClass.E2);

                if (GameObjects.EnemyHeroes.Any())
                {
                    /// <summary>
                    ///     Sets the menu for the E Harass Whitelist.
                    /// </summary>
                    MenuClass.WhiteList2 = new Menu("whitelist", "Harass: Whitelist");
                    {
                        foreach (var target in GameObjects.EnemyHeroes)
                        {
                            MenuClass.WhiteList2.Add(new MenuBool(target.CharName.ToLower(), "Harass: " + target.CharName));
                        }
                    }
                    MenuClass.E.Add(MenuClass.WhiteList2);
                }
                else
                {
                    MenuClass.E.Add(new MenuSeperator("exseparator", "Whitelist not needed"));
                }

                /// <summary>
                ///     Sets the menu for the R.
                /// </summary>
                MenuClass.R = new Menu("r", "Use R to:");
                {
                    if (GameObjects.EnemyHeroes.Any(x => Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
                    {
                        /// <summary>
                        ///     Sets the menu for the Anti-Gapcloser R.
                        /// </summary>
                        MenuClass.Gapcloser = new Menu("gapcloser", "Anti-Gapcloser");
                        {
                            MenuClass.Gapcloser.Add(new MenuBool("enabled", "Enable"));
                            MenuClass.Gapcloser.Add(new MenuSeperator(string.Empty));
                            MenuClass.R.Add(MenuClass.Gapcloser);

                            foreach (var enemy in GameObjects.EnemyHeroes.Where(x => Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
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
                        MenuClass.R.Add(new MenuSeperator(string.Empty, "Anti-Gapcloser not needed"));
                    }

                    MenuClass.R.Add(new MenuSeperator("separator"));
                    if (GameObjects.EnemyHeroes.Count() >= 2)
                    {
                        MenuClass.R.Add(new MenuSliderBool("aoe", "AoE / If can hit >= x enemies", true, 2, 2, GameObjects.EnemyHeroes.Count()));
                    }
                    else
                    {
                        MenuClass.R.Add(new MenuSeperator("separator2", "AoE / Not enough enemies found"));
                    }
                    MenuClass.R.Add(new MenuBool("killsteal", "KillSteal", false));
                    MenuClass.R.Add(new MenuBool("logical", "On Hard-CC'ed Enemies"));
                    MenuClass.R.Add(new MenuBool("interrupter", "Interrupt Enemy Channels"));
                    //MenuClass.R.Add(new MenuSeperator("separator"));
                    //MenuClass.R.Add(new MenuSeperator("separator1", "It will ult the lowest on health,"));
                    //MenuClass.R.Add(new MenuSeperator("separator2", "whitelisted and non-invulnerable enemy in range."));
                    MenuClass.R.Add(new MenuBool("bool", "Semi-Automatic R"));
                    MenuClass.R.Add(new MenuKeyBind("key", "Key:", KeyCode.T, KeybindType.Press));

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the R Whitelist.
                        /// </summary>
                        MenuClass.WhiteList = new Menu("whitelist", "Ultimate: Whitelist");
                        {
                            //MenuClass.WhiteList.Add(new MenuSeperator("separator1", "WhiteList only works for Combo"));
                            //MenuClass.WhiteList.Add(new MenuSeperator("separator2", "not Killsteal (Automatic)"));
                            foreach (var target in GameObjects.EnemyHeroes)
                            {
                                MenuClass.WhiteList.Add(new MenuBool(target.CharName.ToLower(), "Use against: " + target.CharName));
                            }
                        }
                        MenuClass.R.Add(MenuClass.WhiteList);
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
            ///     Sets the menu for the drawings.
            /// </summary>
            MenuClass.Drawings = new Menu("drawings", "Drawings");
            {
                MenuClass.Drawings.Add(new MenuBool("qmin", "Q Minimum Range", false));
                MenuClass.Drawings.Add(new MenuBool("qmax", "Q Maximum Range", false));
                MenuClass.Drawings.Add(new MenuBool("e", "E Range", false));
                MenuClass.Drawings.Add(new MenuBool("r", "R Range", false));
            }
            MenuClass.Root.Add(MenuClass.Drawings);
        }

        #endregion
    }
}