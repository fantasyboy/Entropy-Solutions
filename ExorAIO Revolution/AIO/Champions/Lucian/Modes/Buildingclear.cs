
using Aimtec;
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
    internal partial class Lucian
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PostAttackEventArgs" /> instance containing the event data.</param>
        public void Buildingclear(object sender, PostAttackEventArgs args)
        {
            var target = args.Target;
            if (!target.IsBuilding())
            {
                return;
            }

            /// <summary>
            ///     The E BuildingClear Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["buildings"]) &&
                MenuClass.Spells["e"]["buildings"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.E.Cast(UtilityClass.Player.Position.Extend(Game.CursorPos, 25f));
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
                SpellClass.W.Cast(Game.CursorPos);
            }
        }

        #endregion
    }
}