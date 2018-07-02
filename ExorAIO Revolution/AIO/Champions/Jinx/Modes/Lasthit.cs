
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587
namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Jinx
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Lasthit()
        {
            /// <summary>
            ///     The Force Lasthit Logic. 
            /// </summary>
            if (SpellClass.Q.Ready &&
                IsUsingFishBones() &&
                MenuClass.Miscellaneous["forcelasthit"].As<MenuBool>().Enabled)
            {
                SpellClass.Q.Cast();
            }
        }

        #endregion
    }
}