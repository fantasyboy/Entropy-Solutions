using System.Collections.Generic;
using System.Drawing;
using Entropy;

namespace NabbTracker
{
    /// <summary>
    ///     The Utility class.
    /// </summary>
    internal class UtilityClass
    {
        #region Static Fields

        /// <summary>
        ///     A list of the names of the champions who have a different healthbar type.
        /// </summary>
        public static readonly List<string> SpecialChampions = new List<string> { "Annie", "Jhin", "Corki", "Xayah" };

        /// <summary>
        ///     Gets the Player.
        /// </summary>
        public static Obj_AI_Hero Player = ObjectManager.GetLocalPlayer();

        /// <summary>
        ///     Gets the spellslots.
        /// </summary>
        public static SpellSlot[] SpellSlots =
            {
                SpellSlot.Q,
                SpellSlot.W,
                SpellSlot.E,
                SpellSlot.R
            };

        /// <summary>
        ///     Gets the summoner spellslots.
        /// </summary>
        public static SpellSlot[] SummonerSpellSlots =
            {
                SpellSlot.Summoner1,
                SpellSlot.Summoner2
            };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The cooldown of a determined spell of a determined unit.
        /// </summary>
        public static string GetUnitSpellCooldown(Obj_AI_Hero unit, int spell)
        {
            var unitSpell = unit.SpellBook.GetSpell(SpellSlots[spell]);
            var cooldownRemaining = unitSpell.CooldownEnd - Game.ClockTime;
            if (cooldownRemaining > 0)
            {
                return ((int)cooldownRemaining).ToString();
            }

            if (unitSpell.State.HasFlag(SpellState.Disabled) ||
                unit.IsMe && unitSpell.State.HasFlag(SpellState.Surpressed))
            {
                return "X";
            }
            if (unitSpell.State.HasFlag(SpellState.Unknown))
            {
                return "?";
            }

            return SpellSlots[spell].ToString();
        }

        /// <summary>
        ///     The color of the string, based on determined events.
        /// </summary>
        public static Color GetUnitSpellStateColor(Obj_AI_Hero unit, int spell)
        {
            var unitSpell = unit.SpellBook.GetSpell(SpellSlots[spell]);
            var unitSpellState = unitSpell.State;
            if (unitSpellState.HasFlag(SpellState.NotLearned))
            {
                return Color.Gray;
            }

            if (unitSpellState.HasFlag(SpellState.Disabled) ||
                unit.IsMe && unitSpellState.HasFlag(SpellState.Surpressed))
            {
                return Color.Purple;
            }

            if (unitSpellState.HasFlag(SpellState.NoMana))
            {
                return Color.Cyan;
            }

            if (unitSpellState.HasFlag(SpellState.Cooldown))
            {
                var unitSpellCooldown = unitSpell.CooldownEnd - Game.ClockTime;
                return unitSpellCooldown <= 5 ? Color.Red : Color.Yellow;
            }

            if (unitSpellState.HasFlag(SpellState.Ready))
            {
                return Color.LightGreen;
            }
            
            return Color.Black;
        }

        /// <summary>
        ///     The cooldown of a determined summoner spell of a determined unit.
        /// </summary>
        public static string GetUnitSummonerSpellCooldown(Obj_AI_Hero unit, int summonerSpell)
        {
            var cooldownRemaining = unit.SpellBook.GetSpell(SummonerSpellSlots[summonerSpell]).CooldownEnd - Game.ClockTime;
            return cooldownRemaining > 0 ? ((int)cooldownRemaining).ToString() : "READY";
        }

        /// <summary>
        ///     Gets the fixed name for reach summonerspell in the game.
        /// </summary>
        public static string GetUnitSummonerSpellFixedName(Obj_AI_Hero unit, int summonerSpell)
        {
            switch (unit.SpellBook.GetSpell(SummonerSpellSlots[summonerSpell]).Name.ToLower())
            {
                case "summonerflash":        return "Flash";
                case "summonerdot":          return "Ignite";
                case "summonerheal":         return "Heal";
                case "summonerteleport":     return "Teleport";
                case "summonerexhaust":      return "Exhaust";
                case "summonerhaste":        return "Ghost";
                case "summonerbarrier":      return "Barrier";
                case "summonerboost":        return "Cleanse";
                case "summonermana":         return "Clarity";
                case "summonersnowball":     return "Mark";
                default:
                    return "Smite";
            }
        }

        /// <summary>
        ///     The color of the string, based on determined events.
        /// </summary>
        public static Color GetUnitSummonerSpellStateColor(Obj_AI_Hero unit, int summonerSpell)
        {
            var unitSummonerSpell = unit.SpellBook.GetSpell(SummonerSpellSlots[summonerSpell]);
            var unitSummonerSpellState = unitSummonerSpell.State;
            if (unitSummonerSpellState.HasFlag(SpellState.Disabled) ||
                unit.IsMe && unitSummonerSpellState.HasFlag(SpellState.Surpressed))
            {
                return Color.Purple;
            }

            if (unitSummonerSpellState.HasFlag(SpellState.Cooldown))
            {
                var unitSpellCooldown = unitSummonerSpell.CooldownEnd - Game.ClockTime;
                return unitSpellCooldown <= 5 ? Color.Red : Color.Yellow;
            }

            if (unitSummonerSpellState.HasFlag(SpellState.Ready))
            {
                return Color.LightGreen;
            }

            return Color.Black;
        }

        /// <summary>
        ///     The Exp Healthbars X coordinate adjustment.
        /// </summary>
        public static int ExpXAdjustment(Obj_AI_Hero target)
        {
            return 85;
        }

        /// <summary>
        ///     The Spells Healthbars Y coordinate adjustment.
        /// </summary>
        public static int ExpYAdjustment(Obj_AI_Hero target)
        {
            if (SpecialChampions.Contains(target.ChampionName))
            {
                return MenuClass.Miscellaneous["name"].Enabled ? -65 : -45;
            }

            return MenuClass.Miscellaneous["name"].Enabled ? -60 : -45;
        }

        /// <summary>
        ///     The Healthbars X coordinate adjustment.
        /// </summary>
        public static int SummonerSpellXAdjustment(Obj_AI_Hero target)
        {
            return 28;
        }

        /// <summary>
        ///     The Healthbars Y coordinate adjustment.
        /// </summary>
        public static int SummonerSpellYAdjustment(Obj_AI_Hero target)
        {
            if (SpecialChampions.Contains(target.ChampionName))
            {
                return MenuClass.Miscellaneous["name"].Enabled ? -35 : -15;
            }

            return MenuClass.Miscellaneous["name"].Enabled ? -30 : -15;
        }

        /// <summary>
        ///     The Spells Healthbars X coordinate adjustment.
        /// </summary>
        public static int SpellXAdjustment(Obj_AI_Hero target)
        {
            return 40;
        }

        /// <summary>
        ///     The Spells Healthbars Y coordinate adjustment.
        /// </summary>
        public static int SpellYAdjustment(Obj_AI_Hero target)
        {
            if (SpecialChampions.Contains(target.ChampionName))
            {
                return 40;
            }

            return 25;
        }

        #endregion
    }
}