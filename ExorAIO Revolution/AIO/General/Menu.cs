
using System.Linq;
using Entropy.SDK.Menu;
using Entropy.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587
namespace AIO
{
    /// <summary>
    ///     The general class.
    /// </summary>
    internal partial class General
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Loads the menu.
        /// </summary>
        public static void Menu()
        {
            /// <summary>
            ///     Loads the root Menu.
            /// </summary>
            MenuClass.Root = new Menu(UtilityClass.Player.ChampionName.ToLower(), "ExorAIO: " + UtilityClass.Player.ChampionName, true);
            {
                /// <summary>
                ///     Loads the orbwalker menu.
                /// </summary>
                ImplementationClass.IOrbwalker.Attach(MenuClass.Root);

                /// <summary>
                ///     Loads the general menu.
                /// </summary>
                MenuClass.General = new Menu("generalmenu", "General Menu");
                {
                    MenuClass.General.Add(new MenuBool("supportmode", "Support Mode", false));
                    MenuClass.General.Add(new MenuSliderBool("disableaa", "Disable AutoAttacks in Combo / If Level >= x", false, 2, 2, 18));
                    MenuClass.General.Add(new MenuBool("junglesmall", "Cast to small minions too in JungleClear", false));

                    if (UtilityClass.Player.MaxMana >= 200)
                    {
                        MenuClass.General.Add(new MenuBool("nomanagerifblue", "Ignore ManaManagers if you have Blue Buff", false));
                    }

                    if (UtilityClass.Player.IsMelee)
                    {
                        MenuClass.Hydra = new Menu("hydramenu", "Hydras Menu");
                        {
                            MenuClass.Hydra.Add(new MenuSeparator("hydrasep", "Use Hydras in:"));
                            MenuClass.Hydra.Add(new MenuBool("combo", "Combo"));
                            MenuClass.Hydra.Add(new MenuBool("laneclear", "Laneclear"));
                            MenuClass.Hydra.Add(new MenuBool("mixed", "Harass"));
                            MenuClass.Hydra.Add(new MenuBool("lasthit", "Lasthit"));
                            MenuClass.Hydra.Add(new MenuBool("manual", "While playing manually"));
                        }
                        MenuClass.General.Add(MenuClass.Hydra);
                    }

                    MenuClass.Stormrazor = new Menu("stormrazormenu", "Stormrazor Menu");
                    {
                        MenuClass.Stormrazor.Add(new MenuSeparator("stormsep", "Stop AA'ing until it procs in:"));
                        MenuClass.Stormrazor.Add(new MenuBool("combo", "Combo"));
                        MenuClass.Stormrazor.Add(new MenuBool("laneclear", "Laneclear"));
                        MenuClass.Stormrazor.Add(new MenuBool("mixed", "Harass"));
                        MenuClass.Stormrazor.Add(new MenuBool("lasthit", "Lasthit"));
                    }

                    /// <summary>
                    ///     Loads the preserve mana menu.
                    /// </summary>
                    MenuClass.PreserveMana = new Menu("preservemanamenu", "Preserve Mana Menu");
                    {
                        var championSpellManaCosts = UtilityClass.ManaCostArray.FirstOrDefault(v => v.Key == UtilityClass.Player.ChampionName).Value;
                        if (championSpellManaCosts != null)
                        {
                            MenuClass.PreserveMana.Add(new MenuSeperator("separator", "Preserve Mana for:"));
                            foreach (var slot in UtilityClass.SpellSlots)
                            {
                                MenuClass.PreserveMana.Add(new MenuBool(slot.ToString().ToLower(), slot.ToString(), false));
                            }
                        }
                        else
                        {
                            MenuClass.PreserveMana.Add(new MenuSeperator("preserveseparator", "Preserve Mana not needed"));
                        }
                    }
                    MenuClass.General.Add(MenuClass.PreserveMana);

                    /// <summary>
                    ///     Loads the preserve spells menu.
                    /// </summary>
                    MenuClass.PreserveSpells = new Menu("preservespellsmenu", "Preserve Spells Menu");
                    {
                        MenuClass.PreserveSpells.Add(new MenuSeperator("separator", "Only works for inside-AA-range targets"));
                        MenuClass.PreserveSpells.Add(new MenuSeperator("separator2", "0 = Don't limit."));
                        foreach (var slot in UtilityClass.SpellSlots)
                        {
                            MenuClass.PreserveSpells.Add(new MenuSlider(slot.ToString().ToLower(), $"Don't cast {slot} if target killable in X AAs", 0, 0, 10));
                        }
                    }
                    MenuClass.General.Add(MenuClass.PreserveSpells);
                }
                MenuClass.Root.Add(MenuClass.General);
            }
            MenuClass.Root.Attach();
        }

        #endregion
    }
}