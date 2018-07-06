using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;

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
        
        /// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
        public void Buildingclear(OnPreAttackEventArgs args)
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
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["buildings"]) &&
                MenuClass.Spells["e"]["buildings"].As<MenuSliderBool>().Enabled)
            {
                var turretTarget = target as Obj_AI_Turret;
                if (turretTarget != null)
                {
                    SpellClass.E.CastOnUnit(turretTarget);
                }
            }
        }

        #endregion
    }
}