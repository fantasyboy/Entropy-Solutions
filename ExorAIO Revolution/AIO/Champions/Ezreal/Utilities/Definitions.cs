// ReSharper disable ArrangeMethodOrOperatorBody


using Entropy.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Ezreal
    {
        #region Fields

        /// <summary>
        ///     Gets or sets the Minimum AP for the AP mode to trigger.
        /// </summary>
        public int GetMinimumApForApMode()
        {
            var menuOption = MenuClass.Spells["w"]["apmode"].As<MenuSliderBool>();
            return menuOption.Enabled
                ? menuOption.Value
                : menuOption.MaxValue;
        }

        #endregion
    }
}