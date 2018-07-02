
using System.Collections.Generic;
using System.Linq;
using Entropy;
using Entropy.SDK.Events;
using Entropy.SDK.Extensions;

namespace AIO.Utilities
{
    /// <summary>
    ///     The Bools class.
    /// </summary>
    internal static class Bools
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Returns if a Vector2 position is On Screen.
        /// </summary>
        /// <param name="position">The position.</param>
        public static bool OnScreen(this Vector2 position)
        {
            return !position.IsZero;
        }

        /// <summary>
        ///     Returns if a Spell is usable.
        /// </summary>
        /// <param name="spell">The spell.</param>
        public static bool CanUseSpell(Spell spell)
        {
            return UtilityClass.SpellStates.All(state => !spell.State.HasFlag(state));
        }

        /// <summary>
        ///     Returns if a Spell from a determined SpellSlot is usable.
        /// </summary>
        /// <param name="slot">The spellslot.</param>
        public static bool CanUseSpell(SpellSlot slot)
        {
            return UtilityClass.SpellStates.All(state => !UtilityClass.Player.SpellBook.GetSpell(slot).State.HasFlag(state));
        }

        /// <summary>
        ///     Returns if the name is an auto attack
        /// </summary>
        /// <param name="name">Name of spell</param>
        /// <returns>The <see cref="bool" /></returns>
        public static bool IsAutoAttack(string name)
        {
            name = name.ToLower();
            return name.Contains("attack") && !UtilityClass.NoAttacks.Contains(name) || UtilityClass.Attacks.Contains(name);
        }

        /// <returns>
        ///     true if a determined cell has a Wall flag, else, false.
        /// </returns>
        public static bool IsWall(this Vector3 pos, bool includeBuildings = false)
        {
            var point = NavMesh.WorldToCell(pos).Flags;
            return
                point.HasFlag(NavCellFlags.Wall) ||
                includeBuildings && point.HasFlag(NavCellFlags.Building);
        }

        /// <returns>
        ///     true if an unit has a Sheen-Like buff; otherwise, false.
        /// </returns>
        public static bool HasSheenLikeBuff(this Obj_AI_Hero unit)
        {
            var sheenLikeBuffNames = new[] { "sheen", "LichBane", "dianaarcready", "ItemFrozenFist", "sonapassiveattack", "AkaliTwinDisciplines" };
            return sheenLikeBuffNames.Any(b => UtilityClass.Player.HasBuff(b));
        }

        /// <summary>
        ///     Gets a value indicating whether a determined hero has a stackable item.
        /// </summary>
        public static bool HasTearLikeItem(this Obj_AI_Hero unit)
        {
            return UtilityClass.TearLikeItems.Any(p => UtilityClass.Player.HasItem(p));
        }

        /// <summary>
        ///     Gets a value indicating whether a determined hero has a stackable item.
        /// </summary>
        public static bool IsTearLikeItemReady(this Obj_AI_Hero unit)
        {
            if (!UtilityClass.Player.HasTearLikeItem())
            {
                return false;
            }

            var tearLikeItemSlot = UtilityClass.Player.Inventory.Slots.FirstOrDefault(s => s.SlotTaken && UtilityClass.TearLikeItems.Contains(s.ItemId));
            if (tearLikeItemSlot != null)
            {
                var tearLikeItemSpellSlot = tearLikeItemSlot.SpellSlot;
                if (tearLikeItemSpellSlot != SpellSlot.Unknown &&
                    !UtilityClass.Player.SpellBook.GetSpell(tearLikeItemSpellSlot).State.HasFlag(SpellState.Cooldown))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Returns true if there is a Wall between X pos and Y pos.
        /// </summary>
        public static bool AnyWallInBetween(Vector3 startPos, Vector3 endPos)
        {
            for (var i = 0; i < startPos.Distance(endPos); i+=5)
            {
                var point = NavMesh.WorldToCell(startPos.Extend(endPos, i));
                if (point.Flags.HasFlag(NavCellFlags.Wall) ||
                    point.Flags.HasFlag(NavCellFlags.Building))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Returns true if a determined hero is a zombie.
        /// </summary>
        public static bool IsZombie(this Obj_AI_Hero hero)
        {
            switch (hero.ChampionName)
            {
                case "Sion":
                    return hero.HasBuff("sionpassivezombie");
            }

            return false;
        }

        /// <summary>
        ///     Returns true if a the player is being grabbed by an enemy unit.
        /// </summary>
        public static bool IsBeingGrabbed(this Obj_AI_Hero hero)
        {
            var grabsBuffs = new[] {"ThreshQ", "rocketgrab2"};
            return hero.ValidActiveBuffs().Any(b => grabsBuffs.Contains(b.Name));
        }

        /// <summary>
        ///     Returns true if a the player is being grabbed by an enemy unit.
        /// </summary>
        public static bool HasImmobileBuff(this Obj_AI_Hero hero)
        {
            // Objects: Guardian Angel..
            var immobileObjectLinked = ObjectManager.Get<GameObject>().FirstOrDefault(t => t.IsValid && t.Name == "LifeAura.troy");
            if (immobileObjectLinked != null &&
                ObjectManager.Get<Obj_AI_Hero>().MinBy(t => t.Distance(immobileObjectLinked)) == hero)
            {
                return true;
            }

            // Minions: Zac Passive
            if (hero.ChampionName == "Zac" &&
                !hero.ActionState.HasFlag(ActionState.CanMove) &&
                ObjectManager.Get<Obj_AI_Minion>().Any(m => m.Team == hero.Team && m.UnitSkinName == "ZacRebirthBloblet" && m.Distance(hero) < 500))
            {
                return true;
            }

            // Buffs: Zilean's Chronoshift, Zhonyas, Aatrox's Blood Well, Anivia Egg,
            var immobileBuffs = new[] { "chronorevive", "zhonyasringshield", "AatroxPassiveDeath", "rebirth" };
            if (hero.ValidActiveBuffs().Any(b => immobileBuffs.Contains(b.Name)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Returns true if a determined buff is a Hard CC Buff.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static bool IsHardCC(this Buff buff)
        {
            // ReSharper disable once InconsistentNaming
            var hardCCList = new List<BuffType>
            {
                BuffType.Stun,
                BuffType.Fear,
                BuffType.Flee,
                BuffType.Snare,
                BuffType.Taunt,
                BuffType.Charm,
                BuffType.Knockup,
                BuffType.Suppression
            };

            return hardCCList.Contains(buff.Type);
        }

        /// <summary>
        ///     Gets a value indicating whether a determined champion can move or not.
        /// </summary>
        /// <param name="unit">The hero.</param>
        /// <param name="minTime">The minimum time remaining for the CC to trigger this function.</param>
        public static bool IsImmobile(this Obj_AI_Base unit, double minTime)
        {
            var hero = unit as Obj_AI_Hero;
            if (hero != null &&
                hero.HasImmobileBuff())
            {
                return true;
            }

            if (unit.IsDead ||
                unit.IsDashing() ||
                //unit.Name.Equals("Target Dummy") ||
                unit.HasBuffOfType(BuffType.Knockback))
            {
                return false;
            }

            return unit.ValidActiveBuffs().Any(b =>
                b.IsHardCC() &&
                b.GetRemainingBuffTime() >= minTime);
        }

        /// <returns>
        ///     true if the sender is a hero, a turret or an important jungle monster; otherwise, false.
        /// </returns>
        public static bool ShouldShieldAgainstSender(Obj_AI_Base sender)
        {
            return
                GameObjects.EnemyHeroes.Contains(sender) ||
                GameObjects.EnemyTurrets.Contains(sender) ||
                Extensions.GetGenericJungleMinionsTargets().Contains(sender);
        }

        /// <summary>
        ///     Returns whether the hero is in fountain range.
        /// </summary>
        /// <param name="hero">The Hero</param>
        /// <returns>Is Hero in fountain range</returns>
        public static bool InFountain(this Obj_AI_Hero hero)
        {
            var heroTeam = hero.Team == GameObjectTeam.Order ? "Order" : "Chaos";
            var fountainTurret = ObjectManager.Get<GameObject>().FirstOrDefault(o => o.IsValid && o.Name == "Turret_" + heroTeam + "TurretShrine");
            if (fountainTurret == null)
            {
                return false;
            }

            return hero.Distance(fountainTurret) < 1300f;
        }

        #endregion
    }
}