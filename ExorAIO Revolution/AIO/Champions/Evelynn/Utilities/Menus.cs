
using System.Linq;
using Entropy.SDK.Menu;
using Entropy.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The menu class.
    /// </summary>
    internal partial class Evelynn
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
                    MenuClass.Q.Add(new MenuBool("onlyiffullyallured", "^ Only if enemy fully Allured")).SetToolTip("If the enemy is marked by Allure (W), it will wait until the Mark completes itself before casting Q against him.");
                    MenuClass.Q.Add(new MenuSeperator(string.Empty, string.Empty));
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

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the Q Harass Whitelist.
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
                    /// <summary>
                    ///     Sets the menu for the E Whitelist.
                    /// </summary>
                    MenuClass.WhiteList2 = new Menu("whitelist", "Junglesteal Allure: Whitelist");
                    {
                        foreach (var target in UtilityClass.JungleList)
                        {
                            MenuClass.WhiteList2.Add(new MenuBool(target, "Allure: " + target));
                        }
                    }
                    MenuClass.W.Add(MenuClass.WhiteList2);

                    MenuClass.W.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));
                }
                MenuClass.Spells.Add(MenuClass.W);

                /// <summary>
                ///     Sets the menu for the E.
                /// </summary>
                MenuClass.E = new Menu("e", "Use E to:");
                {
                    MenuClass.E.Add(new MenuBool("combo", "Combo"));
                    MenuClass.E.Add(new MenuBool("onlyiffullyallured", "^ Only if enemy fully Allured")).SetToolTip("If the enemy is marked by Allure (W), it will wait until the Mark completes itself before casting E against him.");
                    MenuClass.E.Add(new MenuSeperator(string.Empty, string.Empty));
                    MenuClass.E.Add(new MenuBool("killsteal", "KillSteal"));
                    MenuClass.E.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.E.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.E.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the Q Harass Whitelist.
                        /// </summary>
                        MenuClass.WhiteList2 = new Menu("whitelist", "Harass: Whitelist");
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
                }
                MenuClass.Spells.Add(MenuClass.E);

                /// <summary>
                ///     Sets the menu for the R.
                /// </summary>
                MenuClass.R = new Menu("r", "Use R to:");
                {
                    if (GameObjects.EnemyHeroes.Any(x => x.IsMelee && Gapcloser.Spells.Any(spell => x.ChampionName == spell.ChampionName)))
                    {
                        /// <summary>
                        ///     Sets the menu for the Anti-Gapcloser R.
                        /// </summary>
                        MenuClass.Gapcloser = new Menu("gapcloser", "Anti-Gapcloser");
                        {
                            MenuClass.Gapcloser.Add(new MenuBool("enabled", "Enable"));
                            MenuClass.Gapcloser.Add(new MenuSeperator(string.Empty));
                            MenuClass.R.Add(MenuClass.Gapcloser);

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
                        MenuClass.R.Add(new MenuSeperator(string.Empty, "Anti-Gapcloser not needed"));
                    }

                    MenuClass.R.Add(new MenuSeperator("separator"));
                    MenuClass.R.Add(new MenuBool("killsteal", "KillSteal", false));
                    if (GameObjects.EnemyHeroes.Count() >= 2)
                    {
                        MenuClass.R.Add(new MenuSliderBool("aoe", "AoE / If can hit >= x enemies", true, 2, 2, GameObjects.EnemyHeroes.Count()));
                    }
                    else
                    {
                        MenuClass.R.Add(new MenuSeperator("separator2", "AoE / Not enough enemies found"));
                    }

                    /// <summary>
                    ///     Sets the customization menu for the R spell.
                    /// </summary>
                    MenuClass.R2 = new Menu("customization", "Customization:");
                    {
                        MenuClass.R2.Add(new MenuSeperator(string.Empty, "Keep in mind that Killsteal R ignores those safety checks"));
                        MenuClass.R2.Add(new MenuSlider("safetyrange", "Blink position safety range:", 450, 200, 750));

                        if (GameObjects.EnemyHeroes.Count() >= 2)
                        {
                            MenuClass.R2.Add(new MenuSlider("aoecheck", "Only if < x enemies in Blink position", 3, 2, GameObjects.EnemyHeroes.Count()));
                        }
                        else
                        {
                            MenuClass.R2.Add(new MenuSeperator("exseparator", "No enemies found, no need for AoE blink position check."));
                        }

                        MenuClass.R2.Add(new MenuBool("turretcheck", "Only if no enemy turrets in Blink position"));
                    }
                    MenuClass.R.Add(MenuClass.R2);
                }
                MenuClass.Spells.Add(MenuClass.R);

                /// <summary>
                ///     Sets the miscellaneous menu.
                /// </summary>
                MenuClass.Miscellaneous = new Menu("miscellaneous", "Miscellaneous");
                {
                    MenuClass.Miscellaneous.Add(new MenuBool("dontaasemiallured", "Don't AA Semi-Allured Targets")).SetToolTip("If the target is Allured (W) it will wait until the mark completes itself before attacking the target");
                }
                MenuClass.Root.Add(MenuClass.Miscellaneous);
            }
            MenuClass.Root.Add(MenuClass.Spells);

            /// <summary>
            ///     Sets the drawings menu.
            /// </summary>
            MenuClass.Drawings = new Menu("drawings", "Drawings");
            {
                MenuClass.Drawings.Add(new MenuBool("q", "Q Range", false));
                MenuClass.Drawings.Add(new MenuBool("w", "W Range", false));
                MenuClass.Drawings.Add(new MenuBool("e", "E Range", false));
                MenuClass.Drawings.Add(new MenuBool("r", "R Range", false));
                MenuClass.Drawings.Add(new MenuBool("rsafepos", "Draw R Safe Position"));
            }
            MenuClass.Root.Add(MenuClass.Drawings);
        }

        #endregion
    }
}