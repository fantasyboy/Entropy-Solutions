using System.Linq;
using Entropy;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI;

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
			    ignoreManaManagerMenu.Enabled)
			{
				return 0;
			}

			var spellData = UtilityClass.ManaCostArray.FirstOrDefault(v => v.Key == UtilityClass.Player.CharName);
			var cost = spellData.Value[slot][UtilityClass.Player.Spellbook.GetSpell(slot).Level - 1];
			return (int) (value.Value + cost / UtilityClass.Player.MaxMP * 100);
		}

		/// <summary>
		///     The minimum health needed to cast the Spell from the 'slot' SpellSlot.
		/// </summary>
		public int GetNeededHealth(SpellSlot slot, MenuComponent value)
		{
			var spellData = UtilityClass.ManaCostArray.FirstOrDefault(v => v.Key == UtilityClass.Player.CharName);
			var cost = spellData.Value[slot][UtilityClass.Player.Spellbook.GetSpell(slot).Level - 1];
			return (int) (value.Value + cost / UtilityClass.Player.MaxHP * 100);
		}

		#endregion
	}
}