
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Anivia
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Laneclear()
        {
            var minions = Extensions.GetEnemyLaneMinionsTargets();

            /// <summary>
            ///     The Q Laneclear Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["laneclear"]) &&
                MenuClass.Spells["q"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                var qMinions = Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.Q.Range);
                //var farmLocation = SpellClass.Q.GetLinearFarmLocation(minions, MenuClass.Spells["q"]["customization"]["laneclear"].As<MenuSlider>().Value);
                switch (UtilityClass.Player.SpellBook.GetSpell(SpellSlot.Q).ToggleState)
                {
                    case 1:
                        /*
                        if (farmLocation != null)
                        {
                            SpellClass.Q.Cast(farmLocation);
                        }
                        */
                        break;
                    case 2:
                        if (FlashFrost != null &&
                            MenuClass.Spells["q"]["customization"]["laneclear"].As<MenuSlider>().Value <=
                                qMinions.Count(t => t.IsValidTarget(SpellClass.Q.Width, false, true, FlashFrost.Position)))
                        {
                            SpellClass.Q.Cast();
                        }
                        break;
                }
            }

            /// <summary>
            ///     The E Laneclear Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["laneclear"]) &&
                MenuClass.Spells["e"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                var target = ImplementationClass.IOrbwalker.GetOrbwalkingTarget() as Obj_AI_Minion;
                if (target != null &&
                    minions.Contains(target) &&
                    UtilityClass.Player.GetSpellDamage(target, SpellSlot.E, DamageStage.Empowered) >= target.Health)
                {
                    UtilityClass.CastOnUnit(SpellClass.E, target);
                }
            }

            /// <summary>
            ///     The R Laneclear Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.R.Slot, MenuClass.Spells["r"]["laneclear"]) &&
                MenuClass.Spells["r"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                var rMinions = Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.R.Range);
                //var farmLocation = SpellClass.R.GetCircularFarmLocation(minions, MenuClass.Spells["r"]["customization"]["laneclear"].As<MenuSlider>().Value);
                switch (UtilityClass.Player.SpellBook.GetSpell(SpellSlot.Q).ToggleState)
                {
                    case 1:
                        /*
                        if (farmLocation != null)
                        {
                            SpellClass.R.Cast(farmLocation);
                        }
                        */
                        break;
                    case 2:
                        if (UtilityClass.Player.InFountain())
                        {
                            return;
                        }

                        if (GlacialStorm != null &&
                            MenuClass.Spells["r"]["customization"]["laneclear"].As<MenuSlider>().Value >
                                rMinions.Count(t => t.IsValidTarget(SpellClass.R.Width, false, true, GlacialStorm.Position)))
                        {
                            SpellClass.R.Cast();
                        }
                        break;
                }
            }
        }

        #endregion
    }
}