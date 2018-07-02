
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
    internal partial class Twitch
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
                    MenuClass.Q.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.Q.Add(new MenuSliderBool("buildings", "To buildings / If Mana >= x%", true, 50, 0, 99));
                }
                MenuClass.Spells.Add(MenuClass.Q);

                /// <summary>
                ///     Sets the menu for the W.
                /// </summary>
                MenuClass.W = new Menu("w", "Use W to:");
                {
                    MenuClass.W.Add(new MenuSliderBool("combo", "Combo / If Venom stacks <= X", true, 2, 0, 6));
                    if (GameObjects.EnemyHeroes.Count() >= 2)
                    {
                        MenuClass.W.Add(new MenuSliderBool("aoe", "AoE / If can hit >= x enemies", false, 3, 2, GameObjects.EnemyHeroes.Count()));
                    }
                    else
                    {
                        MenuClass.W.Add(new MenuSeperator("separator", "AoE / Not enough enemies found."));
                    }
                    MenuClass.W.Add(new MenuSeperator("separator2"));

                    if (GameObjects.EnemyHeroes.Any(x => Gapcloser.Spells.Any(spell => x.ChampionName == spell.ChampionName)))
                    {
                        /// <summary>
                        ///     Sets the menu for the Anti-Gapcloser W.
                        /// </summary>
                        MenuClass.Gapcloser = new Menu("gapcloser", "Anti-Gapcloser");
                        {
                            MenuClass.Gapcloser.Add(new MenuBool("enabled", "Enable"));
                            MenuClass.Gapcloser.Add(new MenuSeperator(string.Empty));
                            MenuClass.W.Add(MenuClass.Gapcloser);

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
                        MenuClass.W.Add(new MenuSeperator(string.Empty, "Anti-Gapcloser not needed"));
                    }

                    MenuClass.W.Add(new MenuSeperator("separator3"));
                    MenuClass.W.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.W.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.W.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

                    /// <summary>
                    ///     Sets the customization menu for the W spell.
                    /// </summary>
                    MenuClass.W2 = new Menu("customization", "Customization:");
                    {
                        //MenuClass.W2.Add(new MenuSeperator("separator1", "General settings:"));
                        MenuClass.W2.Add(new MenuBool("dontwinr", "Don't use W while using R"));
                        //MenuClass.W2.Add(new MenuSeperator("separator2"));
                        //MenuClass.W2.Add(new MenuSeperator("separator3", "Laneclear settings:"));
                        //MenuClass.W2.Add(new MenuSeperator("separator4", "v (Will only take into account minions) v:"));
                        //MenuClass.W2.Add(new MenuSeperator("separator5", "v (with less than or equal 4 venom stacks) v:"));
                        MenuClass.W2.Add(new MenuSlider("laneclear", "Laneclear / if hittable minions >= x%", 3, 1, 10));
                    }
                    MenuClass.W.Add(MenuClass.W2);

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the W Whitelist.
                        /// </summary>
                        MenuClass.WhiteList = new Menu("whitelist", "Harass: Whitelist");
                        {
                            //MenuClass.WhiteList.Add(new MenuSeperator("extendedsep", "Note: The Whitelist only works for Harass."));
                            foreach (var target in GameObjects.EnemyHeroes)
                            {
                                MenuClass.WhiteList.Add(new MenuBool(target.ChampionName.ToLower(), "Harass: " + target.ChampionName));
                            }
                        }
                        MenuClass.W.Add(MenuClass.WhiteList);
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
                    /// <summary>
                    ///     Sets the menu for the E Whitelist.
                    /// </summary>
                    MenuClass.WhiteList2 = new Menu("whitelist", "Junglesteal Expunge: Whitelist");
                    {
                        foreach (var target in UtilityClass.JungleList)
                        {
                            MenuClass.WhiteList2.Add(new MenuBool(target, "Expunge: " + target));
                        }
                    }
                    MenuClass.E.Add(MenuClass.WhiteList2);

                    MenuClass.E.Add(new MenuBool("ondeath", "Before death"));
                    MenuClass.E.Add(new MenuBool("killsteal", "KillSteal"));
                    MenuClass.E.Add(new MenuSliderBool("junglesteal", "Junglesteal / If Level >= x", true, 2, 1, 18));
                    MenuClass.E.Add(new MenuSliderBool("logical", "Automatic / If stacks >= x", false, 6, 1, 6));
                    MenuClass.E.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));

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
            }
            MenuClass.Root.Add(MenuClass.Spells);

            /// <summary>
            ///     Sets the miscellaneous menu.
            /// </summary>
            MenuClass.Miscellaneous = new Menu("miscellaneous", "Miscellaneous");
            {
                MenuClass.Miscellaneous.Add(new MenuBool("stealthrecall", "Use Stealth Recall"));
            }
            MenuClass.Root.Add(MenuClass.Miscellaneous);

            /// <summary>
            ///     Sets the drawings menu.
            /// </summary>
            MenuClass.Drawings = new Menu("drawings", "Drawings");
            {
                MenuClass.Drawings.Add(new MenuBool("qduration", "Q Duration Circle"));
                MenuClass.Drawings.Add(new MenuBool("w", "W Range", false));
                MenuClass.Drawings.Add(new MenuBool("e", "E Range"));
                MenuClass.Drawings.Add(new MenuBool("edmg", "E Damage"));
                MenuClass.Drawings.Add(new MenuBool("r", "R Range"));
            }
            MenuClass.Root.Add(MenuClass.Drawings);
        }

        #endregion
    }
}