// ReSharper disable ArrangeMethodOrOperatorBody


using System.Collections.Generic;
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using SharpDX;
using Entropy.SDK.Caching;

// ReSharper disable LoopCanBeConvertedToQuery

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Xayah
    {
        #region Fields

        /// <summary>
        ///     Returns the Feathers.
        /// </summary>
        public Dictionary<uint, Vector3> Feathers = new Dictionary<uint, Vector3>();

        public float LastCastedETime = 0;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns true if the target is flying by using R, else, false.
        /// </summary>
        public bool IsFlying()
        {
            return UtilityClass.Player.HasBuff("XayahR");
        }

        /// <summary>
        ///     Returns true if the target unit can be hit by any feather.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public bool CanFeathersHitUnit(AIBaseClient unit)
        {
            return CountFeathersHitOnUnit(unit) > 0;
        }

        /// <summary>
        ///     Returns the number of feathers which can hit the target unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public int CountFeathersHitOnUnit(AIBaseClient unit)
        {
            var hit = 0;
            foreach (var feather in Feathers)
            {
                var playerToFeatherRectangle = new Entropy.SDK.Geometry.Rectangle(feather.Value, UtilityClass.Player.Position, SpellClass.E.Width);
                if (playerToFeatherRectangle.IsInsidePolygon((Vector2)unit.Position))
                {
                    hit++;
                }
            }

            return hit;
        }

        /// <summary>
        ///     Returns the number of minions killable by E Cast.
        /// </summary>
        public int CountFeathersKillableMinions()
        {
            return Extensions.GetAllGenericMinionsTargets().Count(m => GetEDamage(m, CountFeathersHitOnUnit(m)) >= m.HP);
        }

        /// <summary>
        ///     Returns true if the target is a perfectly valid rend target.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public bool IsPerfectFeatherTarget(AIBaseClient unit)
        {
            if (unit.IsValidTarget() &&
                CanFeathersHitUnit(unit))
            {
                switch (unit.Type.TypeID)
                {
                    case GameObjectTypeID.AIMinionClient:
                        return true;

                    case GameObjectTypeID.AIHeroClient:
                        return !Invulnerable.Check((AIHeroClient)unit, DamageType.Physical, false);
                }
            }

            return false;
        }

        /// <summary>
        ///     Reloads the Feathers.
        /// </summary>
        public void ReloadFeathers()
        {
            foreach (var feather in ObjectCache.AllGameObjects.Where(o => o.IsValid))
            {
                switch (feather.Name)
                {
                    case "Xayah_Base_Passive_Dagger_Mark8s":
                        Feathers.Add(feather.NetworkID, feather.Position);
                        break;
                }
            }
        }

        #endregion
    }
}