
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class KogMaw
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Laneclear()
        {
            /// <summary>
            ///     The Laneclear E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["laneclear"]) &&
                MenuClass.Spells["e"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                /*
                var farmLocation = SpellClass.E.GetLineFarmLocation(minions, SpellClass.E.Width);
                if (farmLocation.MinionsHit >= MenuClass.Spells["e"]["customization"]["laneclear"].As<MenuSlider>().Value)
                {
                    SpellClass.E.Cast(farmLocation.Position);
                }
                */
            }

            /// <summary>
            ///     The Laneclear R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                UtilityClass.Player.Mana >
                    SpellClass.R.Cost + 50 * (UtilityClass.Player.GetRealBuffCount("kogmawlivingartillerycost") + 1) &&
                MenuClass.Spells["r"]["laneclear"].As<MenuSliderBool>().Value >
                    UtilityClass.Player.GetRealBuffCount("kogmawlivingartillerycost") &&
                MenuClass.Spells["r"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                /*
                var farmLocation = SpellClass.R.GetCircularFarmLocation(minions, SpellClass.R.Width);
                if (farmLocation.MinionsHit >= MenuClass.Spells["r"]["customization"]["laneclear"].As<MenuSlider>().Value)
                {
                    SpellClass.R.Cast(farmLocation.Position);
                }
                */
            }

            /// <summary>
            ///     The Laneclear W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["laneclear"]) &&
                MenuClass.Spells["w"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                if (UtilityClass.Player.HasItem(ItemId.RunaansHurricane))
                {
                    UtilityClass.Player.SpellBook.CastSpell(SpellSlot.W);
                }
            }
        }

        #endregion
    }
}