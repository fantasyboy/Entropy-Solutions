
using System.Collections.Generic;
using System.Linq;
using Entropy;
using Entropy.SDK.Caching;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Events;
using Entropy.SDK.Extensions;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Spells;
using SharpDX;

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
        ///     true if an unit has a Sheen-Like buff; otherwise, false.
        /// </returns>
        public static bool HasSheenLikeBuff(this AIHeroClient unit)
        {
            var sheenLikeBuffNames = new[] { "sheen", "LichBane", "dianaarcready", "ItemFrozenFist", "sonapassiveattack", "AkaliTwinDisciplines" };
            return sheenLikeBuffNames.Any(b => UtilityClass.Player.HasBuff(b));
        }

        /// <summary>
        ///     Gets a value indicating whether a determined hero has a stackable item.
        /// </summary>
        public static bool HasTearLikeItem(this AIHeroClient unit)
        {
            return UtilityClass.TearLikeItems.Any(p => UtilityClass.Player.HasItem(p));
        }

        /// <summary>
        ///     Gets a value indicating whether a determined hero has a stackable item.
        /// </summary>
        public static bool IsTearLikeItemReady(this AIHeroClient unit)
        {
            if (!UtilityClass.Player.HasTearLikeItem())
            {
                return false;
            }

            var tearLikeItemSlot = UtilityClass.Player.InventorySlots.FirstOrDefault(s => UtilityClass.TearLikeItems.Contains((ItemID)s.ItemID));
            if (tearLikeItemSlot != null)
            {
                var tearLikeItemSpellSlot = tearLikeItemSlot.Slot;
                if (tearLikeItemSpellSlot != SpellSlot.Unknown &&
                    !UtilityClass.Player.Spellbook.GetSpellState(tearLikeItemSpellSlot).HasFlag(SpellState.Cooldown))
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
                var point = NavGrid.WorldToCell(startPos.Extend(endPos, i));
                if (point.IsWall() || point.IsBuilding())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Returns true if a determined hero is a zombie.
        /// </summary>
        public static bool IsZombie(this AIHeroClient hero)
        {
            switch (hero.CharName)
            {
                case "Sion":
                    return hero.HasBuff("sionpassivezombie");
            }

            return false;
        }

        /// <summary>
        ///     Returns true if a the player is being grabbed by an enemy unit.
        /// </summary>
        public static bool IsBeingGrabbed(this AIHeroClient hero)
        {
            var grabsBuffs = new[] {"ThreshQ", "rocketgrab2"};
            return hero.GetActiveBuffs().Any(b => grabsBuffs.Contains(b.Name));
        }

        /// <summary>
        ///     Returns true if a the player is being grabbed by an enemy unit.
        /// </summary>
        public static bool HasImmobileBuff(this AIHeroClient hero)
        {
            // Objects: Guardian Angel..
            var immobileObjectLinked = ObjectCache.AllGameObjects.FirstOrDefault(t => t.IsValid && t.Name == "LifeAura.troy");
            if (immobileObjectLinked != null &&
                ObjectCache.AllHeroes.MinBy(t => t.Distance(immobileObjectLinked)) == hero)
            {
                return true;
            }

            // Minions: Zac Passive
            if (hero.CharName == "Zac" &&
                ObjectCache.AllMinions.Any(m => m.Team == hero.Team && m.CharName == "ZacRebirthBloblet" && m.Distance(hero) < 500))
            {
                return true;
            }

            // Buffs: Zilean's Chronoshift, Zhonyas, Stopwatch, Anivia Egg,
            var immobileBuffs = new[] { "chronorevive", "zhonyasringshield", "rebirth" };
            if (hero.GetActiveBuffs().Any(b => immobileBuffs.Contains(b.Name)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Returns true if a determined buff is a Hard CC Buff.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static bool IsHardCC(this BuffInstance buff)
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
        public static bool IsImmobile(this AIBaseClient unit, double minTime)
        {
            var hero = unit as AIHeroClient;
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

            return unit.GetActiveBuffs().Any(b =>
                b.IsHardCC() &&
                b.GetRemainingBuffTime() >= minTime);
        }

        /// <returns>
        ///     true if the sender is a hero, a turret or an important jungle monster; otherwise, false.
        /// </returns>
        public static bool ShouldShieldAgainstSender(AIBaseClient sender)
        {
            return
                ObjectCache.EnemyHeroes.Contains(sender) ||
                ObjectCache.EnemyTurrets.Contains(sender) ||
                Extensions.GetGenericJungleMinionsTargets().Contains(sender);
        }

        /// <summary>
        ///     Returns whether the hero is in fountain range.
        /// </summary>
        /// <param name="hero">The Hero</param>
        /// <returns>Is Hero in fountain range</returns>
        public static bool InFountain(this AIHeroClient hero)
        {
            var heroTeam = hero.Team == GameObjectTeam.Order ? "Order" : "Chaos";
            var fountainTurret = ObjectCache.AllGameObjects.FirstOrDefault(o => o.IsValid && o.Name == "Turret_" + heroTeam + "TurretShrine");
            if (fountainTurret == null)
            {
                return false;
            }

            return hero.Distance(fountainTurret) < 1300f;
        }

        #endregion
    }
}