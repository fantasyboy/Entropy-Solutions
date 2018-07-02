using System.Collections.Generic;
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using Entropy.SDK.TargetSelector;

namespace AIO.Utilities
{
    /// <summary>
    ///     The UtilityData class.
    /// </summary>
    internal static class Extensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the valid generic (lane or jungle) minions targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetAllGenericMinionsTargets()
        {
            return GetAllGenericMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid generic (lane or jungle) minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetAllGenericMinionsTargetsInRange(float range)
        {
            return GetEnemyLaneMinionsTargetsInRange(range).Concat(GetGenericJungleMinionsTargetsInRange(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid generic unit targets in the game.
        /// </summary>
        public static List<Obj_AI_Base> GetAllGenericUnitTargets()
        {
            return GetAllGenericUnitTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid generic unit targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Base> GetAllGenericUnitTargetsInRange(float range)
        {
            return GameObjects.EnemyHeroes.Where(h => h.IsValidTarget(range)).Concat<Obj_AI_Base>(GetAllGenericMinionsTargetsInRange(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid enemy pet targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetEnemyPets()
        {
            return GetEnemyPetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid enemy pets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetEnemyPetsInRange(float range)
        {
            return ObjectManager.Get<Obj_AI_Minion>().Where(h => h.IsValidTarget(range) && UtilityClass.PetList.Contains(h.Name)).ToList();
        }

        /// <summary>
        ///     Gets the valid ally pet targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetAllyPets()
        {
            return GetAllyPetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid ally pets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetAllyPetsInRange(float range)
        {
            return ObjectManager.Get<Obj_AI_Minion>().Where(h => h.IsValidTarget(range, true) && UtilityClass.PetList.Contains(h.Name)).ToList();
        }

        /// <summary>
        ///     Gets the valid ally heroes targets in the game.
        /// </summary>
        public static List<Obj_AI_Hero> GetAllyHeroesTargets()
        {
            return GetAllyHeroesTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid ally heroes targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Hero> GetAllyHeroesTargetsInRange(float range)
        {
            return GameObjects.AllyHeroes.Where(h => h.IsValidTarget(range, true)).ToList();
        }

        /// <summary>
        ///     Gets the valid ally lane minions targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetAllyLaneMinionsTargets()
        {
            return GetAllyLaneMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid ally lane minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetAllyLaneMinionsTargetsInRange(float range)
        {
            return GameObjects.AllyMinions.Where(m => m.IsValidTarget(range, true)).ToList();
        }

        /// <summary>
        ///     Gets the best valid enemy heroes targets in the game.
        /// </summary>
        public static List<Obj_AI_Hero> GetBestEnemyHeroesTargets()
        {
            return GetBestEnemyHeroesTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the best valid enemy heroes targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Hero> GetBestEnemyHeroesTargetsInRange(float range)
        {
            return ImplementationClass.ITargetSelector.GetOrderedTargets(range);
        }

        /// <summary>
        ///     Gets the best valid enemy hero target in the game.
        /// </summary>
        public static Obj_AI_Hero GetBestEnemyHeroTarget()
        {
            return GetBestEnemyHeroTargetInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the best valid enemy hero target in the game inside a determined range.
        /// </summary>
        public static Obj_AI_Hero GetBestEnemyHeroTargetInRange(float range)
        {
            var selectedTarget = TargetSelector.GetSelectedTarget();
            if (selectedTarget != null &&
                selectedTarget.IsValidTarget(range))
            {
                return selectedTarget;
            }

            var orbTarget = ImplementationClass.IOrbwalker.GetOrbwalkingTarget();
            if (orbTarget is Obj_AI_Hero hero &&
                orbTarget.IsValidTarget(range))
            {
                return hero;
            }

            var tsTarget = ImplementationClass.ITargetSelector.GetTarget(range);
            if (tsTarget != null)
            {
                return tsTarget;
            }

            var lastTarget = GameObjects.EnemyHeroes.FirstOrDefault(t => t.IsValidTarget(range) && !t.IsZombie() && !Invulnerable.Check(t));
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (lastTarget != null)
            {
                return lastTarget;
            }

            return null;
        }

        /// <summary>
        ///     Gets the best valid killable enemy hero target in the game inside a determined range.
        /// </summary>
        public static Obj_AI_Hero GetBestSortedTarget(
            DamageType damageType = DamageType.True,
            bool ignoreShields = false,
            bool includeBoundingRadius = false)
        {
            var target = ImplementationClass.ITargetSelector.GetOrderedTargets(float.MaxValue)
                .FirstOrDefault(t =>
                    !t.IsZombie() &&
                    !Invulnerable.Check(t, damageType, ignoreShields));
            return target;
        }

        /// <summary>
        ///     Gets the best valid killable enemy heroes targets in the game inside a determined range.
        /// </summary>
        public static IEnumerable<Obj_AI_Hero> GetBestSortedTargetsInRange(
            float range,
            DamageType damageType = DamageType.True,
            bool ignoreShields = false,
            bool includeBoundingRadius = false)
        {
            range = range + (includeBoundingRadius ? UtilityClass.Player.BoundingRadius : 0);
            var targets = ImplementationClass.ITargetSelector.GetOrderedTargets(range)
                .Where(t =>
                    !t.IsZombie() &&
                    !Invulnerable.Check(t, damageType, ignoreShields));
            return targets;
        }

        /// <summary>
        ///     Gets the valid enemy heroes targets in the game.
        /// </summary>
        public static List<Obj_AI_Hero> GetEnemyHeroesTargets()
        {
            return GetEnemyHeroesTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid enemy heroes targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Hero> GetEnemyHeroesTargetsInRange(float range)
        {
            return GameObjects.EnemyHeroes.Where(h => h.IsValidTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid lane minions targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetEnemyLaneMinionsTargets()
        {
            return GetEnemyLaneMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid lane minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetEnemyLaneMinionsTargetsInRange(float range)
        {
            return GameObjects.EnemyMinions.Where(m => m.IsValidSpellTarget(range) && m.UnitSkinName.Contains("Minion") && !m.UnitSkinName.Contains("Odin")).ToList();
        }

        /// <summary>
        ///     Gets the valid generic (All but small) jungle minions targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetGenericJungleMinionsTargets()
        {
            return GetGenericJungleMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid generic (All but small) jungle minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetGenericJungleMinionsTargetsInRange(float range)
        {
            if (MenuClass.General["junglesmall"].As<MenuBool>().Enabled)
            {
                return GameObjects.Jungle.Where(m => m.IsValidSpellTarget(range)).ToList();
            }

            return GameObjects.Jungle.Where(m => (!GameObjects.JungleSmall.Contains(m) || m.UnitSkinName.Equals("Sru_Crab")) && m.IsValidSpellTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid large jungle minions targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetLargeJungleMinionsTargets()
        {
            return GetLargeJungleMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid large jungle minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetLargeJungleMinionsTargetsInRange(float range)
        {
            return GameObjects.JungleLarge.Where(m => m.IsValidSpellTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid legendary jungle minions targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetLegendaryJungleMinionsTargets()
        {
            return GetLegendaryJungleMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid legendary jungle minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetLegendaryJungleMinionsTargetsInRange(float range)
        {
            return GameObjects.JungleLegendary.Where(m => m.IsValidSpellTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid small jungle minions targets in the game.
        /// </summary>
        public static List<Obj_AI_Minion> GetSmallJungleMinionsTargets()
        {
            return GetSmallJungleMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid small jungle minions targets in the game inside a determined range.
        /// </summary>
        public static List<Obj_AI_Minion> GetSmallJungleMinionsTargetsInRange(float range)
        {
            return GameObjects.JungleSmall.Where(m => m.IsValidSpellTarget(range)).ToList();
        }

        #endregion
    }
}