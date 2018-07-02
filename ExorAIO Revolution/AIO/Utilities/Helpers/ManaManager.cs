using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu;
using Entropy.SDK.Menu.Components;

namespace AIO.Utilities
{
    /// <summary>
    ///     The Mana manager class.
    /// </summary>
    internal class ManaManager
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The minimum mana needed to cast the Spell from the 'slot' SpellSlot.
        /// </summary>
        public static int GetNeededMana(SpellSlot slot, IMenuComponent value)
        {
            var ignoreManaManagerMenu = MenuClass.General["nomanagerifblue"];
            if (ignoreManaManagerMenu != null &&
                UtilityClass.Player.HasBuff("crestoftheancientgolem") &&
                ignoreManaManagerMenu.As<MenuBool>().Enabled)
            {
                return 0;
            }

            if (ObjectManager.Get<GameObject>().Any(o =>
                    o.Type == GameObjectType.obj_GeneralParticleEmitter &&
                    o.Name == "Perks_ManaflowBand_Buff" &&
                    o.Distance(UtilityClass.Player) <= 75))
            {
                return 0;
            }

            if (ObjectManager.Get<GameObject>().Any(o =>
                    o.Type == GameObjectType.obj_GeneralParticleEmitter &&
                    o.Name == "Perks_LastResort_Buf" &&
                    o.Distance(UtilityClass.Player) <= 75))
            {
                return 0;
            }

            var spellData = UtilityClass.ManaCostArray.FirstOrDefault(v => v.Key == UtilityClass.Player.ChampionName);
            var cost = spellData.Value[slot][UtilityClass.Player.GetSpell(slot).Level - 1];
            return
                value.As<MenuSliderBool>().Value +
                (int)(cost / UtilityClass.Player.MaxMana * 100);
        }

        /*
        /// <summary>
        ///     The minimum mana needed to cast the Spell from the 'slot' SpellSlot.
        /// </summary>
        public int GetNeededHealth(SpellSlot slot, MenuComponent value)
        {
            var cost = UtilityClass.Player.SpellBook.GetSpell(slot).Cost;
            return
                value.As<MenuSliderBool>().Value +
                (int)(cost / UtilityClass.Player.MaxHealth * 100);
        }
        */

        #endregion
    }
}