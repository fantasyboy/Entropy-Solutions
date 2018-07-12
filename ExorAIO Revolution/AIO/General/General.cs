
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Damage;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.UI.Components;
using Entropy.ToolKit;

#pragma warning disable 1587
namespace AIO
{
    /// <summary>
    ///     The general class.
    /// </summary>
    internal partial class General
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on postattack.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
        private static void OnPostAttack(OnPostAttackEventArgs args)
        {
            if (!UtilityClass.Player.IsMelee || args.Target.IsStructure())
            {
                return;
            }

            var hydraItems = new[] { ItemID.TitanicHydra, ItemID.RavenousHydra, ItemID.Tiamat };
            if (MenuClass.Hydra != null)
            {
                var hydraSlot = UtilityClass.Player.InventorySlots.FirstOrDefault(s => hydraItems.ToList().Contains((ItemID)s.ItemID));
                if (hydraSlot != null)
                {
                    switch (Orbwalker.Mode)
                    {
                        case OrbwalkingMode.Combo:
                            if (!MenuClass.Hydra["combo"].As<MenuBool>().Enabled)
                            {
                                return;
                            }
                            break;
                        case OrbwalkingMode.Harass:
                            if (!MenuClass.Hydra["mixed"].As<MenuBool>().Enabled)
                            {
                                return;
                            }
                            break;
                        case OrbwalkingMode.LaneClear:
                            if (!MenuClass.Hydra["laneclear"].As<MenuBool>().Enabled)
                            {
                                return;
                            }
                            break;
                        case OrbwalkingMode.LastHit:
                            if (!MenuClass.Hydra["lasthit"].As<MenuBool>().Enabled)
                            {
                                return;
                            }
                            break;
                        default:
                            if (!MenuClass.Hydra["manual"].As<MenuBool>().Enabled)
                            {
                                return;
                            }
                            break; 
                    }

                    var hydraSpellSlot = hydraSlot.Slot;
                    if (hydraSpellSlot != SpellSlot.Unknown &&
                        UtilityClass.Player.Spellbook.GetSpellState(hydraSpellSlot).HasFlag(SpellState.Ready))
                    {
						//Todo: WTF
	                    //UtilityClass.Player.Spellbook.CastSpell(hydraSpellSlot);
                    }
                }
            }
        }

        /// <summary>
        ///     Called on preattack.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
        public static void OnPreAttack(OnPreAttackEventArgs args)
        {
            switch (Orbwalker.Mode)
            {
                /// <summary>
                ///     The 'No AA in Combo' Logic.
                /// </summary>
                case OrbwalkingMode.Combo:
                    if (!UtilityClass.Player.HasSheenLikeBuff() &&
                        UtilityClass.Player.Level() >=
                                MenuClass.General["disableaa"].As<MenuSliderBool>().Value &&
                        MenuClass.General["disableaa"].As<MenuSliderBool>().Enabled)
                    {
                        args.Cancel = true;
                    }
                    break;

                /// <summary>
                ///     The 'Support Mode' Logic.
                /// </summary>
                case OrbwalkingMode.Harass:
                case OrbwalkingMode.LastHit:
                case OrbwalkingMode.LaneClear:
                    if (Extensions.GetEnemyLaneMinionsTargets().Contains(args.Target) &&
                        MenuClass.General["supportmode"].As<MenuBool>().Enabled)
                    {
                        args.Cancel = GameObjects.AllyHeroes.Any(a => !a.IsMe() && a.Distance(UtilityClass.Player) < 2500);
                    }
                    break;
            }

            if (args.Target.IsStructure())
            {
                return;
            }

            var stormrazorSlot = UtilityClass.Player.InventorySlots.FirstOrDefault(s => s.IsValid /*&& s.ItemID == ItemID.Stormrazor*/); //Todo: find stormrazor ItemID
            if (stormrazorSlot != null)
            {
                switch (Orbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                        if (!MenuClass.Stormrazor["combo"].As<MenuBool>().Enabled)
                        {
                            return;
                        }
                        break;
                    case OrbwalkingMode.Harass:
                        if (!MenuClass.Stormrazor["mixed"].As<MenuBool>().Enabled)
                        {
                            return;
                        }
                        break;
                    case OrbwalkingMode.LaneClear:
                        if (!MenuClass.Stormrazor["laneclear"].As<MenuBool>().Enabled)
                        {
                            return;
                        }
                        break;
                    case OrbwalkingMode.LastHit:
                        if (!MenuClass.Stormrazor["lasthit"].As<MenuBool>().Enabled)
                        {
                            return;
                        }
                        break;
                }

                if (!UtilityClass.Player.HasBuff("stormrazorbuff")) // TODO: find the real buffname.
                {
                    args.Cancel = true;
                }
            }
        }

		/// <summary>
		///     Fired on spell cast.
		/// </summary>

		/// <param name="args">The <see cref="SpellbookLocalCastSpellEventArgs" /> instance containing the event data.</param>
		public static void OnLocalCastSpell(SpellbookLocalCastSpellEventArgs args)
        {
            var slot = args.Slot;
            if (UtilityClass.SpellSlots.Contains(slot))
            {
                /// <summary>
                ///     The 'Preserve Mana' Logic.
                /// </summary>
                var championSpellManaCosts = UtilityClass.ManaCostArray.FirstOrDefault(v => v.Key == UtilityClass.Player.CharName).Value;
                if (championSpellManaCosts != null)
                {
                    var Spellbook = UtilityClass.Player.Spellbook;
                    var data = UtilityClass.PreserveManaData;

                    var spell = Spellbook.GetSpell(slot);
                    var menuOption = MenuClass.PreserveMana[slot.ToString().ToLower()];
                    if (menuOption != null && menuOption.As<MenuBool>().Enabled)
                    {
                        var registeredSpellData = data.FirstOrDefault(d => d.Key == slot).Value;
                        var actualSpellData = championSpellManaCosts[slot][spell.Level - 1];

                        if (data.ContainsKey(slot) &&
                            registeredSpellData != actualSpellData)
                        {
                            data.Remove(slot);
                            Logging.Log($"Preserve Mana List: Removed {slot} (Updated ManaCost).");
                        }

                        if (!data.ContainsKey(slot) && spell.Level > 0)
                        {
                            data.Add(slot, actualSpellData);
                            Logging.Log($"Preserve Mana List: Added {slot}, Cost: {actualSpellData}.");
                        }
                    }
                    else
                    {
                        if (data.ContainsKey(slot))
                        {
                            data.Remove(slot);
                            Logging.Log($"Preserve Mana List: Removed {slot} (Disabled).");
                        }
                    }

                    var sum = data
                        .Where(s => MenuClass.PreserveMana[s.Key.ToString().ToLower()].As<MenuBool>().Enabled)
                        .Sum(s => s.Value);
                    if (sum <= 0)
                    {
                        return;
                    }

                    var spellCost =
                        championSpellManaCosts[slot][UtilityClass.Player.Spellbook.GetSpell(slot).Level - 1];
                    var mana = UtilityClass.Player.MP;
                    if (!data.Keys.Contains(slot) && mana - spellCost < sum)
                    {
                        Logging.Log($"Preserve Mana List: Denied Spell {slot} Usage (Mana: {mana}, Cost: {spellCost}), Preserve Mana Quantity: {sum}");
                        args.Execute = false;
                    }
                }

                /// <summary>
                ///     The 'Preserve Spells' Logic.
                /// </summary>
                switch (Orbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                    case OrbwalkingMode.Harass:
                        var target = Orbwalker.GetOrbwalkingTarget() as AIHeroClient;
                        if (target != null)
                        {
                            if (target.GetRealHealth() <=
                                    UtilityClass.Player.GetAutoAttackDamage(target) *
                                    MenuClass.PreserveSpells[args.Slot.ToString().ToLower()].As<MenuSlider>().Value)
                            {
                                args.Execute = false;
                            }
                        }
                        break;
                }
            }
        }

        #endregion
    }
}