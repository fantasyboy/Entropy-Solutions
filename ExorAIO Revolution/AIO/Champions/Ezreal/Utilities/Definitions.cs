﻿// ReSharper disable ArrangeMethodOrOperatorBody

using AIO.Utilities;
using Entropy.SDK.UI.Components;

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
            var menuOption = MenuClass.W["apmode"].As<MenuSliderBool>();
            return menuOption.Enabled
                ? menuOption.Value
                : menuOption.MaxValue;
        }

        #endregion
    }
}