
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable LoopCanBeConvertedToQuery

using System.Collections.Generic;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

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
        public Dictionary<int, Vector3> MineField = new Dictionary<int, Vector3>();

        /// <summary>
        ///     Returns the WorkedGrounds position.
        /// </summary>
        public Dictionary<int, Vector3> WorkedGrounds = new Dictionary<int, Vector3>();

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
            return WorkedGrounds.Count(o => o.Value.Distance(UtilityClass.Player.Position) <= range);
        }

        /// <summary>
        ///     Gets the Best position inside the MineField for W Cast.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public Vector3 GetBestBouldersHitPosition(Obj_AI_Base unit)
        {
            var mostBouldersHit = 0;
            var mostBouldersHitPos = Vector3.Zero;
            foreach (var mine in MineField)
            {
                var unitToMineRectangle = new Vector2Geometry.Rectangle((Vector2)unit.ServerPosition, (Vector2)unit.ServerPosition.Extend((Vector2)mine.Value, WPushDistance), unit.BoundingRadius);
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
        public int GetBestBouldersHitPositionHitBoulders(Obj_AI_Base unit)
        {
            var mostBouldersHit = 0;
            foreach (var mine in MineField)
            {
                var unitToMineRectangle = new Vector2Geometry.Rectangle((Vector2)unit.ServerPosition, (Vector2)unit.ServerPosition.Extend((Vector2)mine.Value, WPushDistance), unit.BoundingRadius);
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
        public Vector3 GetUnitPositionAfterPull(Obj_AI_Base unit)
        {
            var targetPred = SpellClass.W.GetPrediction(unit).CastPosition;
            return targetPred.Extend(UtilityClass.Player.Position, WPushDistance);
        }

        /// <summary>
        ///     Returns the position the target would have after being pushed by W.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public Vector3 GetUnitPositionAfterPush(Obj_AI_Base unit)
        {
            var targetPred = SpellClass.W.GetPrediction(unit).CastPosition;
            return targetPred.Extend(UtilityClass.Player.Position, -WPushDistance);
        }

        /// <summary>
        ///     Returns the position the target would have after being pushed by W, based on the user's option preference.
        /// </summary>
        /// <param name="target">The target.</param>
        public Vector3 GetTargetPositionAfterW(Obj_AI_Hero target)
        {
            var position = new Vector3();
            switch (MenuClass.Spells["w"]["selection"][target.ChampionName.ToLower()].As<MenuList>().Value)
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
                    var isKillable = target.GetRealHealth() < UtilityClass.Player.GetSpellDamage(target, SpellSlot.Q) * (IsNearWorkedGround() ? 1 : 3) +
                                     UtilityClass.Player.GetSpellDamage(target, SpellSlot.W) +
                                     UtilityClass.Player.GetSpellDamage(target, SpellSlot.E);
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
                    if (UtilityClass.Player.Distance(GetUnitPositionAfterPull(target)) >= 200f)
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
                            MenuClass.Spells["w"]["selection"][t.ChampionName.ToLower()].As<MenuList>().Value < 3))
                    {
                        if (UtilityClass.Player.Distance(GetUnitPositionAfterPull(target)) >= 200f)
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
            foreach (var mine in ObjectManager.Get<GameObject>().Where(o => o != null && o.IsValid))
            {
                switch (mine.Name)
                {
                    case "Taliyah_Base_E_Mines.troy":
                        MineField.Add(mine.NetworkId, mine.Position);
                        break;
                }
            }
        }

        /// <summary>
        ///     Reloads the WorkedGrounds.
        /// </summary>
        public void ReloadWorkedGrounds()
        {
            foreach (var ground in ObjectManager.Get<GameObject>().Where(o => o != null && o.IsValid))
            {
                switch (ground.Name)
                {
                    case "Taliyah_Base_Q_aoe.troy":
                    case "Taliyah_Base_Q_aoe_river.troy":
                        WorkedGrounds.Add(ground.NetworkId, ground.Position);
                        break;
                }
            }
        }

        #endregion
    }
}