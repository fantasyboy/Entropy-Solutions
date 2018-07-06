
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
    internal partial class Sivir
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
                    MenuClass.Q.Add(new MenuBool("killsteal", "KillSteal"));
                    MenuClass.Q.Add(new MenuSliderBool("harass", "Harass / If Mana >= x%", true, 50, 0, 99));
                    MenuClass.Q.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.Q.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

                    /// <summary>
                    ///     Sets the customization menu for the Q spell.
                    /// </summary>
                    MenuClass.Q2 = new Menu("customization", "Customization:");
                    {
                        //MenuClass.Q2.Add(new MenuSeperator("separator3", "Laneclear settings:"));
                        MenuClass.Q2.Add(new MenuSlider("laneclear", "Laneclear / if hittable minions >= x%", 4, 1, 10));
                    }
                    MenuClass.Q.Add(MenuClass.Q2);

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the W Harass Whitelist.
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
                    MenuClass.W.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.W.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.W.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));
                    MenuClass.W.Add(new MenuSliderBool("buildings", "To buildings / if Mana >= x%", true, 50, 0, 99));

                    /// <summary>
                    ///     Sets the customization menu for the W spell.
                    /// </summary>
                    MenuClass.W2 = new Menu("customization", "Customization:");
                    {
                        //MenuClass.W2.Add(new MenuSeperator("separator3", "Laneclear settings:"));
                        MenuClass.W2.Add(new MenuSlider("laneclear", "Laneclear / if hittable minions >= x%", 4, 1, 10));
                    }
                    MenuClass.W.Add(MenuClass.W2);

                    if (GameObjects.EnemyHeroes.Any())
                    {
                        /// <summary>
                        ///     Sets the menu for the W Harass Whitelist.
                        /// </summary>
                        MenuClass.WhiteList2 = new Menu("whitelist", "Harass: Whitelist");
                        {
                            foreach (var target in GameObjects.EnemyHeroes)
                            {
                                MenuClass.WhiteList2.Add(new MenuBool(target.CharName.ToLower(), "Harass: " + target.CharName));
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
                    MenuClass.E.Add(new MenuSliderBool("logical", "Use Shield / with X ms Delay", true, 0, 0, 250));
                    {
                        /// <summary>
                        ///     Sets the menu for the E Whitelist.
                        /// </summary>
                        MenuClass.WhiteList = new Menu("whitelist", "Shield: Whitelist Menu");
                        {
                            MenuClass.WhiteList.Add(new MenuBool("minions", "Shield: Dragon's Attacks"));
                            foreach (var enemy in GameObjects.EnemyHeroes)
                            {
                                if (enemy.CharName.Equals("Alistar"))
                                {
                                    MenuClass.WhiteList.Add(
                                        new MenuBool(
                                            $"{enemy.CharName.ToLower()}.pulverize",
                                            $"Shield: {enemy.CharName}'s Pulverize (Q)"));
                                }
                                if (enemy.CharName.Equals("Braum"))
                                {
                                    MenuClass.WhiteList.Add(
                                        new MenuBool(
                                            $"{enemy.CharName.ToLower()}.passive",
                                            $"Shield: {enemy.CharName}'s Passive"));
                                }
                                if (enemy.CharName.Equals("Jax"))
                                {
                                    MenuClass.WhiteList.Add(
                                        new MenuBool(
                                            $"{enemy.CharName.ToLower()}.jaxcounterstrike",
                                            $"Shield: {enemy.CharName}'s JaxCounterStrike (E)"));
                                }
                                if (enemy.CharName.Equals("KogMaw"))
                                {
                                    MenuClass.WhiteList.Add(
                                        new MenuBool(
                                            $"{enemy.CharName.ToLower()}.kogmawicathiansurprise",
                                            $"Shield: {enemy.CharName}'s KogMawIcathianSurprise (Passive)"));
                                }
                                if (enemy.CharName.Equals("Nautilus"))
                                {
                                    MenuClass.WhiteList.Add(
                                        new MenuBool(
                                            $"{enemy.CharName.ToLower()}.nautilusravagestrikeattack",
                                            $"Shield: {enemy.CharName}'s NautilusRavageStrikeAttack (Passive)"));
                                }
                                if (enemy.CharName.Equals("Udyr"))
                                {
                                    MenuClass.WhiteList.Add(
                                        new MenuBool(
                                            $"{enemy.CharName.ToLower()}.udyrbearattack",
                                            $"Shield: {enemy.CharName}'s UdyrBearAttack (E)"));
                                }
                            }
                        }

                        if (GameObjects.EnemyHeroes.Any(x => Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
                        {
                            /// <summary>
                            ///     Sets the menu for the Anti-Gapcloser E.
                            /// </summary>
                            MenuClass.Gapcloser = new Menu("gapcloser", "Anti-Gapcloser");
                            {
                                MenuClass.Gapcloser.Add(new MenuBool("enabled", "Enable"));
                                MenuClass.Gapcloser.Add(new MenuSeperator(string.Empty));
                                MenuClass.E.Add(MenuClass.Gapcloser);

                                foreach (var enemy in GameObjects.EnemyHeroes.Where(x => Gapcloser.Spells.Any(spell =>
                                    x.CharName == spell.ChampionName &&
                                    spell.SpellType == Gapcloser.Type.Targeted)))
                                {
                                    MenuClass.SubGapcloser = new Menu(enemy.CharName.ToLower(), enemy.CharName);
                                    {
                                        foreach (var spell in Gapcloser.Spells.Where(x =>
                                            x.CharName == enemy.CharName &&
                                            x.SpellType == Gapcloser.Type.Targeted))
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
                            MenuClass.E.Add(new MenuSeperator(string.Empty, "Anti-Gapcloser not needed"));
                        }

                        MenuClass.E.Add(MenuClass.WhiteList);
                    }
                }
                MenuClass.Spells.Add(MenuClass.E);
            }
            MenuClass.Root.Add(MenuClass.Spells);

            /// <summary>
            ///     Sets the menu for the drawings.
            /// </summary>
            MenuClass.Drawings = new Menu("drawings", "Drawings");
            {
                MenuClass.Drawings.Add(new MenuBool("q", "Q Range", false));
            }
            MenuClass.Root.Add(MenuClass.Drawings);
        }

        #endregion
    }
}