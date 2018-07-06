﻿using System.Collections.Generic;
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

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
        public static List<AIMinionClient> GetAllGenericMinionsTargets()
        {
            return GetAllGenericMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid generic (lane or jungle) minions targets in the game inside a determined range.
        /// </summary>
        public static List<AIMinionClient> GetAllGenericMinionsTargetsInRange(float range)
        {
            return GetEnemyLaneMinionsTargetsInRange(range).Concat(GetGenericJungleMinionsTargetsInRange(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid generic unit targets in the game.
        /// </summary>
        public static List<AIBaseClient> GetAllGenericUnitTargets()
        {
            return GetAllGenericUnitTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid generic unit targets in the game inside a determined range.
        /// </summary>
        public static List<AIBaseClient> GetAllGenericUnitTargetsInRange(float range)
        {
            return GameObjects.EnemyHeroes.Where(h => h.IsValidTarget(range)).Concat<AIBaseClient>(GetAllGenericMinionsTargetsInRange(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid enemy pet targets in the game.
        /// </summary>
        public static List<AIMinionClient> GetEnemyPets()
        {
            return GetEnemyPetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid enemy pets in the game inside a determined range.
        /// </summary>
        public static List<AIMinionClient> GetEnemyPetsInRange(float range)
        {
            return ObjectManager.Get<AIMinionClient>().Where(h => h.IsValidTarget(range) && UtilityClass.PetList.Contains(h.Name)).ToList();
        }

        /// <summary>
        ///     Gets the valid ally pet targets in the game.
        /// </summary>
        public static List<AIMinionClient> GetAllyPets()
        {
            return GetAllyPetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid ally pets in the game inside a determined range.
        /// </summary>
        public static List<AIMinionClient> GetAllyPetsInRange(float range)
        {
            return ObjectManager.Get<AIMinionClient>().Where(h => h.IsValidTarget(range, true) && UtilityClass.PetList.Contains(h.Name)).ToList();
        }

        /// <summary>
        ///     Gets the valid ally heroes targets in the game.
        /// </summary>
        public static List<AIHeroClient> GetAllyHeroesTargets()
        {
            return GetAllyHeroesTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid ally heroes targets in the game inside a determined range.
        /// </summary>
        public static List<AIHeroClient> GetAllyHeroesTargetsInRange(float range)
        {
            return GameObjects.AllyHeroes.Where(h => h.IsValidTarget(range, true)).ToList();
        }

        /// <summary>
        ///     Gets the valid ally lane minions targets in the game.
        /// </summary>
        public static List<AIMinionClient> GetAllyLaneMinionsTargets()
        {
            return GetAllyLaneMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid ally lane minions targets in the game inside a determined range.
        /// </summary>
        public static List<AIMinionClient> GetAllyLaneMinionsTargetsInRange(float range)
        {
            return GameObjects.AllyMinions.Where(m => m.IsValidTarget(range, true)).ToList();
        }

        /// <summary>
        ///     Gets the best valid enemy heroes targets in the game.
        /// </summary>
        public static List<AIHeroClient> GetBestEnemyHeroesTargets()
        {
            return GetBestEnemyHeroesTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the best valid enemy heroes targets in the game inside a determined range.
        /// </summary>
        public static List<AIHeroClient> GetBestEnemyHeroesTargetsInRange(float range)
        {
            return ImplementationClass.ITargetSelector.GetOrderedTargets(range);
        }

        /// <summary>
        ///     Gets the best valid enemy hero target in the game.
        /// </summary>
        public static AIHeroClient GetBestEnemyHeroTarget()
        {
            return GetBestEnemyHeroTargetInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the best valid enemy hero target in the game inside a determined range.
        /// </summary>
        public static AIHeroClient GetBestEnemyHeroTargetInRange(float range)
        {
			/*
            var selectedTarget = TargetSelector.GetSelectedTarget();
            if (selectedTarget != null &&
                selectedTarget.IsValidTarget(range))
            {
                return selectedTarget;
            }*/

            var orbTarget = ImplementationClass.IOrbwalker.GetOrbwalkingTarget();
            if (orbTarget is AIHeroClient &&
                orbTarget.IsValidTarget(range))
            {
                return orbTarget;
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
	    ///     Returns true if this unit is able to be targetted by spells 
	    /// </summary>
	    /// <param name="unit">The unit.</param>
	    /// <param name="range">The range.</param>
	    public static bool IsValidSpellTarget(this AttackableUnit unit, float range = float.MaxValue)
	    {
		    if (!unit.IsValidTarget(range))
		    {
			    return false;
		    }

		    if (unit is AIHeroClient)
		    {
			    return true;
		    }

		    var mUnit = unit as AIMinionClient;
		    if (mUnit == null)
		    {
			    return false;
		    }

		    var name = mUnit.CharName.ToLower();
		    if (name.Contains("ward") || name.Contains("sru_plant_") || name.Contains("barrel"))
		    {
			    return false;
		    }

		    return true;
	    }

		/// <summary>
		///     Gets the best valid killable enemy hero target in the game inside a determined range.
		/// </summary>
		public static AIHeroClient GetBestSortedTarget(
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
        public static IEnumerable<AIHeroClient> GetBestSortedTargetsInRange(
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
        public static List<AIHeroClient> GetEnemyHeroesTargets()
        {
            return GetEnemyHeroesTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid enemy heroes targets in the game inside a determined range.
        /// </summary>
        public static List<AIHeroClient> GetEnemyHeroesTargetsInRange(float range)
        {
            return GameObjects.EnemyHeroes.Where(h => h.IsValidTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid lane minions targets in the game.
        /// </summary>
        public static List<AIMinionClient> GetEnemyLaneMinionsTargets()
        {
            return GetEnemyLaneMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid lane minions targets in the game inside a determined range.
        /// </summary>
        public static List<AIMinionClient> GetEnemyLaneMinionsTargetsInRange(float range)
        {
            return GameObjects.EnemyMinions.Where(m => m.IsValidSpellTarget(range) && m.CharName.Contains("Minion") && !m.CharName.Contains("Odin")).ToList();
        }

        /// <summary>
        ///     Gets the valid generic (All but small) jungle minions targets in the game.
        /// </summary>
        public static List<AIMinionClient> GetGenericJungleMinionsTargets()
        {
            return GetGenericJungleMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid generic (All but small) jungle minions targets in the game inside a determined range.
        /// </summary>
        public static List<AIMinionClient> GetGenericJungleMinionsTargetsInRange(float range)
        {
            if (MenuClass.General["junglesmall"].As<MenuBool>().Enabled)
            {
                return GameObjects.Jungle.Where(m => m.IsValidSpellTarget(range)).ToList();
            }

            return GameObjects.Jungle.Where(m => (!GameObjects.JungleSmall.Contains(m) || m.CharName.Equals("Sru_Crab")) && m.IsValidSpellTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid large jungle minions targets in the game.
        /// </summary>
        public static List<AIMinionClient> GetLargeJungleMinionsTargets()
        {
            return GetLargeJungleMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid large jungle minions targets in the game inside a determined range.
        /// </summary>
        public static List<AIMinionClient> GetLargeJungleMinionsTargetsInRange(float range)
        {
            return GameObjects.JungleLarge.Where(m => m.IsValidSpellTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid legendary jungle minions targets in the game.
        /// </summary>
        public static List<AIMinionClient> GetLegendaryJungleMinionsTargets()
        {
            return GetLegendaryJungleMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid legendary jungle minions targets in the game inside a determined range.
        /// </summary>
        public static List<AIMinionClient> GetLegendaryJungleMinionsTargetsInRange(float range)
        {
            return GameObjects.JungleLegendary.Where(m => m.IsValidSpellTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid small jungle minions targets in the game.
        /// </summary>
        public static List<AIMinionClient> GetSmallJungleMinionsTargets()
        {
            return GetSmallJungleMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid small jungle minions targets in the game inside a determined range.
        /// </summary>
        public static List<AIMinionClient> GetSmallJungleMinionsTargetsInRange(float range)
        {
            return GameObjects.JungleSmall.Where(m => m.IsValidSpellTarget(range)).ToList();
        }

        #endregion
    }
}