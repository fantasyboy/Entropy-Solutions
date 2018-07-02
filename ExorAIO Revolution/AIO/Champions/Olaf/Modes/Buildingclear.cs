
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Olaf
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PreAttackEventArgs" /> instance containing the event data.</param>
        public void Buildingclear(object sender, PreAttackEventArgs args)
        {
            var target = args.Target;
            if (!target.IsBuilding())
            {
                return;
            }

            /// <summary>
            ///     The W BuildingClear Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["buildings"]) &&
                MenuClass.Spells["w"]["buildings"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.W.Cast();
            }
        }

        #endregion
    }
}