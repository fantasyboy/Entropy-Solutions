using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

#pragma warning disable 1587

namespace NabbTracker
{
    /// <summary>
    ///     The menu class.
    /// </summary>
    internal class Menus
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Builds the general Menu.
        /// </summary>
        public static void Initialize()
        {
            /// <summary>
            ///     The general Menu.
            /// </summary>
            MenuClass.Menu = new Menu("nabbtracker", "NabbTracker", true);
            {
                /// <summary>
                ///     Sets the menu for the SpellTracker.
                /// </summary>
                MenuClass.SpellTracker = new Menu("spelltracker", "Spell Tracker");
                {
                    MenuClass.SpellTracker.Add(new MenuBool("me", "Me", false));
                    MenuClass.SpellTracker.Add(new MenuBool("allies", "Allies"));
                    MenuClass.SpellTracker.Add(new MenuBool("enemies", "Enemies"));
                }
                MenuClass.Menu.Add(MenuClass.SpellTracker);

                /// <summary>
                ///     Sets the menu for the ExpTracker.
                /// </summary>
                MenuClass.ExpTracker = new Menu("exptracker", "Experience Tracker");
                {
                    MenuClass.ExpTracker.Add(new MenuBool("me", "Me", false));
                    MenuClass.ExpTracker.Add(new MenuBool("allies", "Allies"));
                    MenuClass.ExpTracker.Add(new MenuBool("enemies", "Enemies"));
                }
                MenuClass.Menu.Add(MenuClass.ExpTracker);

                /// <summary>
                ///     Sets the menu for the TowerRangeTracker.
                /// </summary>
                MenuClass.TowerRangeTracker = new Menu("towerrangetracker", "TowerRange Tracker");
                {
                    MenuClass.TowerRangeTracker.Add(new MenuBool("allies", "Allies", false));
                    MenuClass.TowerRangeTracker.Add(new MenuBool("enemies", "Enemies"));
                }
                MenuClass.Menu.Add(MenuClass.TowerRangeTracker);

                /// <summary>
                ///     Sets the menu for the AttackRangeTracker.
                /// </summary>
                MenuClass.AttackRangeTracker = new Menu("attackrangetracker", "AttackRange Tracker");
                {
                    MenuClass.AttackRangeTracker.Add(new MenuBool("allies", "Allies", false));
                    MenuClass.AttackRangeTracker.Add(new MenuBool("enemies", "Enemies"));
                }
                MenuClass.Menu.Add(MenuClass.AttackRangeTracker);

                /// <summary>
                ///     The miscellaneous Menu.
                /// </summary>
                MenuClass.Miscellaneous = new Menu("miscellaneous", "Miscellaneous");
                {
                    MenuClass.Miscellaneous.Add(new MenuBool("name", "Adjust the Bars for the Summoner Names", false));

                    /// <summary>
                    ///     The Colorblind Menu.
                    /// </summary>
                    MenuClass.ColorblindMenu = new Menu("colorblind", "Colorblind Menu");
                    {
                        MenuClass.ColorblindMenu.Add(new MenuSeperator("separator", "Select your colorblind mode:"));
                        MenuClass.ColorblindMenu.Add(new MenuList("mode", "Mode:", new[] { "Normal", "Deuteranopia", "Protanopia", "Tritanopia", "Achromatopsia" }, 0));
                    }
                    MenuClass.Miscellaneous.Add(MenuClass.ColorblindMenu);
                }
                MenuClass.Menu.Add(MenuClass.Miscellaneous);
            }
            MenuClass.Menu.Attach();
        }

        #endregion
    }
}