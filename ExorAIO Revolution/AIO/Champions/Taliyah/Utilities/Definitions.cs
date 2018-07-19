
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable LoopCanBeConvertedToQuery

using System.Collections.Generic;
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Caching;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using SharpDX;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Taliyah
    {
        #region Fields

        public double LastWTime;

        /// <summary>
        ///     Returns the Boulders' width.
        /// </summary>
        public const float BouldersWidth = 30f;

        /// <summary>
        ///     Returns the number of maximum hittable boulders per enemy.
        /// </summary>
        public const int MaxHittableBoulders = 4;

        /// <summary>
        ///     Returns the W Push distance.
        /// </summary>
        public const float WPushDistance = 300f; //TODO: Find real push distance.

        /// <summary>
        ///     Returns the Worked Ground's width.
        /// </summary>
        public const float WorkedGroundWidth = 412.5f;

        /// <summary>
        ///     Returns the MineField position.
        /// </summary>
        public Dictionary<GameObject, Vector3> MineField = new Dictionary<GameObject, Vector3>();

        /// <summary>
        ///     Returns the WorkedGrounds position.
        /// </summary>
        public Dictionary<GameObject, Vector3> WorkedGrounds = new Dictionary<GameObject, Vector3>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns true if there are any worked grounds in a determined range from the player.
        /// </summary>
        /// <param name="range">The range.</param>
        public bool AnyTerrainInRange(float range)
        {
            return CountTerrainsInRange(range) > 0;
        }

        /// <summary>
        ///     Returns the number of worked grounds in a determined range from the player.
        /// </summary>
        /// <param name="range">The range.</param>
        public int CountTerrainsInRange(float range)
        {
            return WorkedGrounds.Count(o => o.Value.DistanceToPlayer() <= range);
        }

        /// <summary>
        ///     Gets the Best position inside the MineField for W Cast.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public Vector3 GetBestBouldersHitPosition(AIBaseClient unit)
        {
            var mostBouldersHit = 0;
            var mostBouldersHitPos = Vector3.Zero;
            foreach (var mine in MineField)
            {
                var unitToMineRectangle = new Vector2Geometry.Rectangle((Vector2)unit.Position, (Vector2)unit.Position.Extend((Vector2)mine.Value, WPushDistance), unit.BoundingRadius);
                var bouldersHit = MineField.Count(o => unitToMineRectangle.IsInside((Vector2)o.Value));
                if (bouldersHit > mostBouldersHit)
                {
                    mostBouldersHit = bouldersHit;
                    mostBouldersHitPos = mine.Value;

                    if (bouldersHit >= MaxHittableBoulders)
                    {
                        break;
                    }
                }
            }

            return mostBouldersHitPos;
        }

        /// <summary>
        ///     Gets the approximative number of mines hit by the target inside the MineField after W Cast.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public int GetBestBouldersHitPositionHitBoulders(AIBaseClient unit)
        {
            var mostBouldersHit = 0;
            foreach (var mine in MineField)
            {
                var unitToMineRectangle = new Vector2Geometry.Rectangle((Vector2)unit.Position, (Vector2)unit.Position.Extend((Vector2)mine.Value, WPushDistance), unit.BoundingRadius);
                var bouldersHit = MineField.Count(o => unitToMineRectangle.IsInside((Vector2)o.Value));
                if (bouldersHit > mostBouldersHit)
                {
                    mostBouldersHit = bouldersHit;
                }
            }

            return mostBouldersHit;
        }

        /// <summary>
        ///     Returns the position the target would have after being pulled by W.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public Vector3 GetUnitPositionAfterPull(AIBaseClient unit)
        {
            var targetPred = SpellClass.W.GetPrediction(unit).CastPosition;
            return targetPred.Extend(UtilityClass.Player.Position, WPushDistance);
        }

        /// <summary>
        ///     Returns the position the target would have after being pushed by W.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public Vector3 GetUnitPositionAfterPush(AIBaseClient unit)
        {
            var targetPred = SpellClass.W.GetPrediction(unit).CastPosition;
            return targetPred.Extend(UtilityClass.Player.Position, -WPushDistance);
        }

        /// <summary>
        ///     Returns the position the target would have after being pushed by W, based on the user's option preference.
        /// </summary>
        /// <param name="target">The target.</param>
        public Vector3 GetTargetPositionAfterW(AIHeroClient target)
        {
            var position = new Vector3();
            switch (MenuClass.W["selection"][target.CharName.ToLower()].Value)
            {
                case 0:
                    position = GetUnitPositionAfterPull(target);
                    break;
                case 1:
                    position = GetUnitPositionAfterPush(target);
                    break;

                /// <summary>
                ///     Pull if killable else Push.
                /// </summary>
                case 2:
	                var isKillable = target.GetRealHealth() < (IsNearWorkedGround()
		                                 ? GetQDamage(target)
		                                 : GetQDamage(target, 3)) + GetWDamage(target) + GetEDamage(target);
					if (isKillable)
                    {
                        position = GetUnitPositionAfterPull(target);
                    }
                    else
                    {
                        position = GetUnitPositionAfterPush(target);
                    }
                    break;

                /// <summary>
                ///     Pull if not near else Push.
                /// </summary>
                case 3:
                    if (target.DistanceToPlayer() >= UtilityClass.Player.GetAutoAttackRange(target))
                    {
                        position = GetUnitPositionAfterPull(target);
                    }
                    else
                    {
                        position = GetUnitPositionAfterPush(target);
                    }
                    break;

                /// <summary>
                ///     Ignore Target If Possible.
                /// </summary>
                case 4:
                    if (!GameObjects.EnemyHeroes.Any(t =>
                            t.IsValidTarget(SpellClass.W.Range) &&
                            MenuClass.W["selection"][t.CharName.ToLower()].Value < 4))
                    {
                        if (target.DistanceToPlayer() >= UtilityClass.Player.GetAutoAttackRange(target))
                        {
                            position = GetUnitPositionAfterPull(target);
                        }
                        else
                        {
                            position = GetUnitPositionAfterPush(target);
                        }
                    }
                    break;
            }

            return position;
        }

        /// <summary>
        ///     Returns true if the player is near a worked ground.
        /// </summary>
        public bool IsNearWorkedGround()
        {
            return AnyTerrainInRange(WorkedGroundWidth);
        }

        /// <summary>
        ///     Reloads the MineField.
        /// </summary>
        public void ReloadMineField()
        {
            foreach (var mine in ObjectCache.AllGameObjects.Where(o => o.IsValid))
            {
                switch (mine.Name)
                {
                    case "Taliyah_Base_E_Mines":
                        MineField.Add(mine, mine.Position);
                        break;
                }
            }
        }

        /// <summary>
        ///     Reloads the WorkedGrounds.
        /// </summary>
        public void ReloadWorkedGrounds()
        {
            foreach (var ground in ObjectCache.AllGameObjects.Where(o => o.IsValid))
			{
                switch (ground.Name)
                {
                    case "Taliyah_Base_Q_aoe":
                    case "Taliyah_Base_Q_aoe_river":
                        WorkedGrounds.Add(ground, ground.Position);
                        break;
                }
            }
        }

        #endregion
    }
}