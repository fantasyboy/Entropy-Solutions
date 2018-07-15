using System;
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Caching;
using Entropy.SDK.Events;
using Entropy.SDK.UI;
using Entropy.SDK.UI.Components;
using Gapcloser = AIO.Utilities.Gapcloser;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The menu class.
    /// </summary>
    internal partial class Vayne
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
                    MenuClass.Q.Add(new MenuBool("engage", "Engager", false)).SetToolTip("Casts if enemy gets out of AA range");
					MenuClass.Q.Add(new MenuSeperator("separator"));

                    if (GameObjects.EnemyHeroes.Any(x => x.IsMelee && Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
                    {
                        /// <summary>
                        ///     Sets the menu for the Anti-Gapcloser Q.
                        /// </summary>
                        MenuClass.Gapcloser = new Menu("gapcloser", "Anti-Gapcloser");
                        {
                            MenuClass.Gapcloser.Add(new MenuBool("enabled", "Enable"));
                            MenuClass.Gapcloser.Add(new MenuSeperator(string.Empty));
                            MenuClass.Q.Add(MenuClass.Gapcloser);

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
                        MenuClass.Q.Add(new MenuSeperator("antigapclosernotneeded", "Anti-Gapcloser not needed"));
                    }

                    MenuClass.Q.Add(new MenuSeperator("separator2"));
                    MenuClass.Q.Add(new MenuSliderBool("farmhelper", "Farmhelper / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.Q.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.Q.Add(new MenuSliderBool("buildings", "To buildings / if Mana >= x%", true, 50, 0, 99));

                    /// <summary>
                    ///     Sets the customization menu for the Q spell.
                    /// </summary>
                    MenuClass.Q2 = new Menu("customization", "Customization:");
                    {
                        MenuClass.Q2.Add(new MenuBool("noqoutaarange", "Don't Q out of AA range from target", false));
                        MenuClass.Q2.Add(new MenuBool("onlyqifmouseoutaarange", "Only Q if mouse out of AA Range", false));
                        if (GameObjects.EnemyHeroes.Any())
                        {
                            var count = GameObjects.EnemyHeroes.Count();
                            MenuClass.Q2.Add(new MenuSliderBool("qrangecheck", "Don't Q if pos has >= X enemies in range", false, count >= 3 ? 3 : count, 1, count));
                        }
                        else
                        {
                            MenuClass.Q2.Add(new MenuSeperator("exseparator", "Don't Q if pos has >= / No enemies found, no need for a position range check."));
                        }
                        MenuClass.Q2.Add(new MenuBool("wstacks", "Use Q only to proc 3rd W Ring", false));
                        MenuClass.Q2.Add(new MenuBool("noqturret", "Don't use Q under Enemy Turret", false));
                    }
                    MenuClass.Q.Add(MenuClass.Q2);
                }
                MenuClass.Spells.Add(MenuClass.Q);

                /// <summary>
                ///     Sets the menu for the E.
                /// </summary>
                MenuClass.E = new Menu("e", "Use E to:");
                {
                    MenuClass.E.Add(new MenuList("emode", "Condemn Mode", new []{ "Exory Perfect", "Exory Fast", "Don't Condemn to stun"}).SetToolTip("Fast: Fastest condemn possible, Perfect: Slower but almost 100% accurate."));
                    MenuClass.E.Add(new MenuBool("killsteal", "KillSteal"));
                    MenuClass.E.Add(new MenuSeperator("separator"));

                    if (GameObjects.EnemyHeroes.Any(x => x.IsMelee && Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
                    {
                        /// <summary>
                        ///     Sets the menu for the Anti-Gapcloser E.
                        /// </summary>
                        MenuClass.Gapcloser2 = new Menu("gapcloser", "Anti-Gapcloser");
                        {
                            MenuClass.Gapcloser2.Add(new MenuBool("enabled", "Enable"));
                            MenuClass.Gapcloser2.Add(new MenuSeperator(string.Empty));
	                        MenuClass.E.Add(MenuClass.Gapcloser2);

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
					}
                    else
                    {
                        MenuClass.E.Add(new MenuSeperator("antigapclosernotneeded", "Anti-Gapcloser not needed"));
                    }

                    MenuClass.E.Add(new MenuSeperator("separator2"));
	                if (ObjectCache.EnemyHeroes.Any(t => Interrupter.SpellDatabase.Keys.Contains(t.CharName)))
	                {
		                /// <summary>
		                ///     Sets the menu for the Interrupter E.
		                /// </summary>
		                MenuClass.Interrupter = new Menu("interrupter", "Interrupter");
		                {
			                MenuClass.Interrupter.Add(new MenuBool("enabled", "Enable"));
			                MenuClass.Interrupter.Add(new MenuSeperator(string.Empty));
			                MenuClass.E.Add(MenuClass.Interrupter);

							foreach (var enemy in ObjectCache.EnemyHeroes.Where(t => Interrupter.SpellDatabase.Keys.Contains(t.CharName)))
			                {
				                MenuClass.SubInterrupter = new Menu(enemy.CharName.ToLower(), enemy.CharName);
				                {
					                foreach (var list in Interrupter.SpellDatabase.Where(x => x.Key == enemy.CharName))
					                {
						                foreach (var spell in list.Value)
						                {
							                MenuClass.SubInterrupter.Add(new MenuBool(
								                $"{enemy.CharName.ToLower()}.{spell.SpellSlot.ToString().ToLower()}",
								                $"Interrupt: {spell.SpellSlot}"));
										}
					                }
				                }
				                MenuClass.Interrupter.Add(MenuClass.SubInterrupter);
			                }
						}
					}
	                else
	                {
		                MenuClass.E.Add(new MenuSeperator("interrupternotneeded", "Interrupter not needed"));
	                }
	                MenuClass.E.Add(new MenuSeperator("separator3"));

					MenuClass.E.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.E.Add(new MenuSeperator("separator4"));
                    MenuClass.E.Add(new MenuBool("bool", "Semi-Automatic E"));
                    MenuClass.E.Add(new MenuKeyBind("key", "Key:", WindowMessageWParam.U, KeybindType.Hold));

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the E Whitelist.
                        /// </summary>
                        MenuClass.WhiteList = new Menu("whitelist", "Condemn: Whitelist");
                        {
                            //MenuClass.WhiteList.Add(new MenuSeperator("separator1", "WhiteList only works for Combo"));
                            //MenuClass.WhiteList.Add(new MenuSeperator("separator2", "not Killsteal (Automatic)"));
                            foreach (var target in GameObjects.EnemyHeroes)
                            {
                                MenuClass.WhiteList.Add(new MenuBool(target.CharName.ToLower(), "Stun: " + target.CharName));
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
            }
            MenuClass.Root.Add(MenuClass.Spells);

            /// <summary>
            ///     Sets the miscellaneous menu.
            /// </summary>
            MenuClass.Miscellaneous = new Menu("miscellaneous", "Miscellaneous");
            {
                MenuClass.Miscellaneous.Add(new MenuBool("focusw", "Focus enemies with 2 W stacks"));
                MenuClass.Miscellaneous.Add(new MenuSliderBool("stealthtime", "Stay Invisible: For at least x ms [1000 ms = 1 second]", false, 0, 0, 1000));

                if (GameObjects.EnemyHeroes.Any())
                {
                    var count = GameObjects.EnemyHeroes.Count();
                    MenuClass.Miscellaneous.Add(new MenuSliderBool("stealthcheck", "Stay Invisible: if >= x enemies in AA Range", false, Math.Min(count, 3), 1, count));
                }
                else
                {
                    MenuClass.Miscellaneous.Add(new MenuSeperator("exseparator", "No enemies found, no need for stealth slider check."));
                }
            }
            MenuClass.Root.Add(MenuClass.Miscellaneous);

            /// <summary>
            ///     Sets the drawings menu.
            /// </summary>
            MenuClass.Drawings = new Menu("drawings", "Drawings");
            {
                MenuClass.Drawings.Add(new MenuBool("q", "Q Range"));
	            MenuClass.Drawings.Add(new MenuBool("wdmg", "W Damage"));
				MenuClass.Drawings.Add(new MenuBool("e", "E Range", false));
            }
            MenuClass.Root.Add(MenuClass.Drawings);
        }

        #endregion
    }
}