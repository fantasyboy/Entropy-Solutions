
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Tristana
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
            ///     The Q BuildingClear Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["buildings"].As<MenuBool>().Enabled)
            {
                SpellClass.Q.Cast();
            }

            /// <summary>
            ///     The E BuildingClear Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["buildings"]) &&
                MenuClass.Spells["e"]["buildings"].As<MenuSliderBool>().Enabled)
            {
                var turretTarget = target as Obj_AI_Turret;
                if (turretTarget != null)
                {
                    UtilityClass.CastOnUnit(SpellClass.E, turretTarget);
                }
            }
        }

        #endregion
    }
}