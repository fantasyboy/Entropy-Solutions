
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
    internal partial class Xayah
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
                    MenuClass.Q.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.Q.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.Q.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

                    /// <summary>
                    ///     Sets the customization menu for the Q spell.
                    /// </summary>
                    MenuClass.Q2 = new Menu("customization", "Customization:");
                    {
                        //MenuClass.Q2.Add(new MenuSeperator("separator1", "Laneclear settings:"));
                        MenuClass.Q2.Add(new MenuSlider("laneclear", "Laneclear / if hittable minions >= x%", 3, 1, 10));
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

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the Q Whitelist.
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
                }
                MenuClass.Spells.Add(MenuClass.Q);

                /// <summary>
                ///     Sets the menu for the W.
                /// </summary>
                MenuClass.W = new Menu("w", "Use W to:");
                {
                    MenuClass.W.Add(new MenuBool("combo", "Combo"));
                    MenuClass.W.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.W.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.W.Add(new MenuSliderBool("buildings", "To buildings / if Mana >= x%", true, 50, 0, 99));
                }
                MenuClass.Spells.Add(MenuClass.W);

                /// <summary>
                ///     Sets the menu for the E.
                /// </summary>
                MenuClass.E = new Menu("e", "Use E to:");
                {
                    //MenuClass.E.Add(new MenuSeperator("separator1", "If the targets gets hit by 3 or more Feathers"));
                    //MenuClass.E.Add(new MenuSeperator("separator2", "it gets rooted for a brief period."));
                    MenuClass.E.Add(new MenuSliderBool("combo", "Combo / If can Hit >= x Feathers", true, 3, 3, 8));
                    MenuClass.E.Add(new MenuBool("ondeath", "Before death"));
                    //MenuClass.E.Add(new MenuSeperator("separator3"));
                    MenuClass.E.Add(new MenuBool("killsteal", "Killsteal"));
                    MenuClass.E.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.E.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

                    /// <summary>
                    ///     Sets the customization menu for the E spell.
                    /// </summary>
                    MenuClass.E2 = new Menu("customization", "Customization:");
                    {
                        //MenuClass.E2.Add(new MenuSeperator("separator1", "Laneclear settings:"));
                        MenuClass.E2.Add(new MenuSlider("laneclear", "Only Laneclear if killable minions >= x%", 3, 1, 10));
                    }
                    MenuClass.E.Add(MenuClass.E2);
                }
                MenuClass.Spells.Add(MenuClass.E);

                /// <summary>
                ///     Sets the menu for the R.
                /// </summary>
                MenuClass.R = new Menu("r", "Use R to:");
                {
                    if (GameObjects.EnemyHeroes.Any(x => x.IsMelee && Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
                    {
                        /// <summary>
                        ///     Sets the menu for the Anti-Gapcloser R.
                        /// </summary>
                        MenuClass.Gapcloser = new Menu("gapcloser", "Anti-Gapcloser");
                        {
                            MenuClass.Gapcloser.Add(new MenuBool("enabled", "Enable"));
                            MenuClass.Gapcloser.Add(new MenuSeperator(string.Empty));
                            MenuClass.R.Add(MenuClass.Gapcloser);

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
                        MenuClass.R.Add(new MenuSeperator(string.Empty, "Anti-Gapcloser not needed"));
                    }

                    MenuClass.R.Add(new MenuSeperator("separator"));
                    //MenuClass.R.Add(new MenuSeperator("separator1"));
                    //MenuClass.R.Add(new MenuSeperator("separator2", "It will ult the lowest on health,"));
                    //MenuClass.R.Add(new MenuSeperator("separator3", "whitelisted and non-invulnerable enemy in range."));
                    MenuClass.R.Add(new MenuBool("bool", "Semi-Automatic R"));
                    MenuClass.R.Add(new MenuList("mode", "Semi-Automatic R mode", new[] { "R", "Q->R" }, 0));
                    MenuClass.R.Add(new MenuKeyBind("key", "Key:", KeyCode.T, KeybindType.Press));

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the R Whitelist.
                        /// </summary>
                        MenuClass.WhiteList = new Menu("whitelist", "Ultimate: Whitelist");
                        {
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
            ///     Sets the miscellaneous menu.
            /// </summary>
            MenuClass.Miscellaneous = new Menu("miscellaneous", "Miscellaneous");
            {
                //MenuClass.Miscellaneous.Add(new MenuSeperator("separator1", "If enabled, Xayah will not cast Q or W if"));
                //MenuClass.Miscellaneous.Add(new MenuSeperator("separator2", "she would gain more than the maximum number"));
                //MenuClass.Miscellaneous.Add(new MenuSeperator("separator3", "of Feathers possible (5)"));
                //MenuClass.Miscellaneous.Add(new MenuSeperator("separator4", "Example: Xayah has 4 Feather stacks and"));
                //MenuClass.Miscellaneous.Add(new MenuSeperator("separator5", "gains 3 Feathers with a cast, this means"));
                //MenuClass.Miscellaneous.Add(new MenuSeperator("separator6", "She will not cast Q or W until her stacks"));
                //MenuClass.Miscellaneous.Add(new MenuSeperator("separator7", "get equal or lower than 2, cause 2 + 3 = 5"));
                MenuClass.Miscellaneous.Add(new MenuBool("feathersweaving", "Feathers Weaving Logic", false));
            }
            MenuClass.Root.Add(MenuClass.Miscellaneous);

            /// <summary>
            ///     Sets the menu for the drawings.
            /// </summary>
            MenuClass.Drawings = new Menu("drawings", "Drawings");
            {
                MenuClass.Drawings.Add(new MenuBool("q", "Q Range", false));
                MenuClass.Drawings.Add(new MenuBool("r", "R Range", false));
                MenuClass.Drawings.Add(new MenuBool("feathers", "Feathers Linking", false));
            }
            MenuClass.Root.Add(MenuClass.Drawings);
        }

        #endregion
    }
}