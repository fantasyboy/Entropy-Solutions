// ReSharper disable ArrangeMethodOrOperatorBody

using System;
using System.Collections.Generic;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Damage.JSON;
using Entropy.SDK.Extensions;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Kalista
    {
        #region Fields

        /// <summary>
        ///     Gets or sets the SoulBound.
        /// </summary>
        public Obj_AI_Hero SoulBound;

        /// <summary>
        ///     Gets all the Offensive R Logics.
        /// </summary>
        public Dictionary<string, Tuple<string, string>> RLogics = new Dictionary<string, Tuple<string, string>>
        {
            { "Blitzcrank", new Tuple<string, string>("rocketgrab2",        "Balista")  },
            { "Skarner",    new Tuple<string, string>("SkarnerImpale",      "Skalista") },
            { "TahmKench",  new Tuple<string, string>("tahmkenchwdevoured", "Talista")  }
        };
        

        /// <summary>
        ///     Gets all the important jungle locations.
        /// </summary>
        internal readonly List<Vector3> Locations = new List<Vector3>
                                                        {
                                                            new Vector3(9827.56f, -71.2406f, 4426.136f),
                                                            new Vector3(4951.126f, -71.2406f, 10394.05f),
                                                            new Vector3(10998.14f, 51.72351f, 6954.169f),
                                                            new Vector3(7082.083f, 56.2041f, 10838.25f),
                                                            new Vector3(3804.958f, 52.11121f, 7875.456f),
                                                            new Vector3(7811.249f, 53.81299f, 4034.486f)
                                                        };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the total rend damage on a determined unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public double GetTotalRendDamage(Obj_AI_Base unit)
        {
            var player = UtilityClass.Player;
            return player.GetSpellDamage(unit, SpellSlot.E) +
                   player.GetSpellDamage(unit, SpellSlot.E, DamageStage.Buff);
        }

        /// <summary>
        ///     Returns true if the target is a perfectly valid rend target.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public bool IsPerfectRendTarget(Obj_AI_Base unit)
        {
            var spellRange = SpellClass.E.Range;
            var orbTarget = ImplementationClass.IOrbwalker.GetOrbwalkingTarget() as Obj_AI_Hero;

            if (orbTarget != null &&
                orbTarget.NetworkId != unit.NetworkId &&
                orbTarget.IsValidSpellTarget(SpellClass.E.Range))
            {
                spellRange = SpellClass.Q.Range;
            }

            if (unit.IsValidSpellTarget(spellRange) &&
                unit.HasBuff("kalistaexpungemarker"))
            {
                switch (unit.Type)
                {
                    case GameObjectType.obj_AI_Minion:
                        return true;

                    case GameObjectType.obj_AI_Hero:
                        var heroUnit = (Obj_AI_Hero)unit;
                        return !Invulnerable.Check(heroUnit);
                }
            }

            return false;
        }

        #endregion
    }
}