// ReSharper disable ArrangeMethodOrOperatorBody
// ReSharper disable LoopCanBeConvertedToQuery

using System.Collections.Generic;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Syndra
    {
        #region Fields

        /// <summary>
        ///     Returns the HoldedSphere.
        /// </summary>
        public GameObject HoldedSphere;

        /// <summary>
        ///     Returns the DarkSpheres.
        /// </summary>
        public Dictionary<int, Vector3> DarkSpheres = new Dictionary<int, Vector3>();

        /// <summary>
        ///     Returns the Selected DarkSphere's NetworkID.
        /// </summary>
        public int SelectedDarkSphereNetworkId = 0;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns true if the 'obj' object is a DarkSphere, else false.
        /// </summary>
        /// <param name="obj">The object.</param>
        public bool IsDarkSphere(GameObject obj)
        {
            switch (obj.Name)
            {
                case "Syndra_Base_Q_idle.troy":
                case "Syndra_Base_Q_Lv5_idle.troy":
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     Returns true if Syndra is currently holding a Force of Will object, else false.
        /// </summary>
        public bool IsHoldingForceOfWillObject()
        {
            return SpellClass.W.ToggleState == 2;
        }

        /// <summary>
        ///     Gets the best Force of Will object.
        /// </summary>
        public GameObject GetForceOfWillObject()
        {
            var possibleTarget1 = GameObjects.JungleLarge.FirstOrDefault(m => m.IsValidSpellTarget(SpellClass.W.Range));
            if (possibleTarget1 != null)
            {
                return possibleTarget1;
            }

            var possibleTarget2 = GameObjects.EnemyMinions.FirstOrDefault(m => m.IsValidSpellTarget(SpellClass.W.Range));
            if (possibleTarget2 != null)
            {
                return possibleTarget2;
            }

            var possibleTarget3 = ObjectManager.Get<GameObject>().FirstOrDefault(o =>
                    o.IsValid &&
                    IsDarkSphere(o) &&
                    o.NetworkId != SelectedDarkSphereNetworkId &&
                    o.Distance(UtilityClass.Player.ServerPosition) <= SpellClass.W.Range);
            if (possibleTarget3 != null)
            {
                return possibleTarget3;
            }

            return null;
        }

        /// <summary>
        ///     Returns true if the target unit can be hit by any sphere.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public bool CanSpheresHitUnit(Obj_AI_Base unit)
        {
            foreach (var sphere in DarkSpheres)
            {
                var targetPos = (Vector2)unit.ServerPosition;
                if (DarkSphereScatterRectangle(sphere).IsInside(targetPos) &&
                    UtilityClass.Player.Distance(sphere.Value) < SpellClass.E.Range)
                {
                    switch (unit.Type)
                    {
                        case GameObjectType.obj_AI_Minion:
                            return true;
                        case GameObjectType.obj_AI_Hero:
                            return !Invulnerable.Check((Obj_AI_Hero)unit, DamageType.Magical, false);
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Returns true if the target unit can be hit by a determined sphere.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="sphere">The sphere.</param>
        public bool CanSphereHitUnit(Obj_AI_Base unit, KeyValuePair<int, Vector3> sphere)
        {
            var targetPos = (Vector2)unit.ServerPosition;
            if (DarkSphereScatterRectangle(sphere).IsInside(targetPos) &&
                UtilityClass.Player.Distance(sphere.Value) < SpellClass.E.Range)
            {
                switch (unit.Type)
                {
                    case GameObjectType.obj_AI_Minion:
                        return true;
                    case GameObjectType.obj_AI_Hero:
                        return !Invulnerable.Check((Obj_AI_Hero)unit, DamageType.Magical, false);
                }
            }

            return false;
        }

        /// <summary>
        ///     Gets the real Damage the R spell would deal to a determined enemy hero.
        /// </summary>
        /// <param name="target">The target.</param>
        public double GetTotalUnleashedPowerDamage(Obj_AI_Hero target)
        {
            var player = UtilityClass.Player;
            var singleSphereDamage = player.GetSpellDamage(target, SpellSlot.R) / 3;
            return singleSphereDamage * player.SpellBook.GetSpell(SpellSlot.R).Ammo;
        }

        /// <summary>
        ///     Returns true if the target is a perfectly valid stunnable target.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public bool IsPerfectSphereTarget(Obj_AI_Base unit)
        {
            if (unit.IsValidTarget() &&
                CanSpheresHitUnit(unit))
            {
                switch (unit.Type)
                {
                    case GameObjectType.obj_AI_Minion:
                        return true;

                    case GameObjectType.obj_AI_Hero:
                        return !Invulnerable.Check((Obj_AI_Hero)unit, DamageType.Magical, false);
                }
            }

            return false;
        }

        /// <summary>
        ///     The Sphere Scatter Rectangle.
        /// </summary>
        /// <param name="sphere">The sphere.</param>
        public Vector2Geometry.Rectangle DarkSphereScatterRectangle(KeyValuePair<int, Vector3> sphere)
        {
            return new Vector2Geometry.Rectangle(
                           (Vector2)sphere.Value.Extend(UtilityClass.Player.Position, SpellClass.Q.Width*2),
                           (Vector2)sphere.Value.Extend(UtilityClass.Player.Position, -1050f-SpellClass.Q.Width/2+UtilityClass.Player.Distance(sphere.Value)),
                           SpellClass.Q.Width);
        }

        /// <summary>
        ///     The Scatter the Weak Cone.
        /// </summary>
        /// <param name="targetPos">The target position.</param>
        public Vector2Geometry.Sector ScatterTheWeakCone(Vector3 targetPos)
        {
            return new Vector2Geometry.Sector(
                        (Vector2)UtilityClass.Player.Position,
                        (Vector2)targetPos,
                        SpellClass.E.Width,
                        SpellClass.E.Range - 50f);
        }

        /// <summary>
        ///     Reloads the DarkSpheres.
        /// </summary>
        public void ReloadDarkSpheres()
        {
            foreach (var sphere in ObjectManager.Get<GameObject>().Where(o => o != null && o.IsValid))
            {
                if (DarkSpheres.Any(o => o.Key == sphere.NetworkId))
                {
                    DarkSpheres.Remove(sphere.NetworkId);
                }

                switch (sphere.Name)
                {
                    case "Syndra_Base_Q_idle.troy":
                    case "Syndra_Base_Q_Lv5_idle.troy":
                        DarkSpheres.Add(sphere.NetworkId, sphere.Position);
                        break;
                }
            }
        }

        #endregion
    }
}