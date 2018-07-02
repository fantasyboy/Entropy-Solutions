
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Evelynn
    {
        /// <summary>
        ///     Returns true if the target is Allured.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public bool IsAllured(Obj_AI_Base unit)
        {
            return unit.HasBuff("EvelynnW");
        }

        /// <summary>
        ///     Returns true if the target is fully Allured.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public bool IsFullyAllured(Obj_AI_Base unit)
        {
            if (unit.HasBuff("EvelynnW"))
            {
                var normalObjects = ObjectManager.Get<GameObject>().Where(o => o.IsValid && o.Name == "Evelynn_Base_W_Fizz_Mark_Decay");
                return normalObjects.Any(o => ObjectManager.Get<Obj_AI_Base>().Where(t => t.Team != o.Team).MinBy(t => t.Distance(o)) == unit);
            }

            return false;
        }

        /// <summary>
        ///     Returns the real range of the Q spell.
        /// </summary>
        public float GetRealQRange()
        {
            return IsHateSpikeSkillshot() ? SpellClass.Q.Range : SpellClass.Q2.Range;
        }

        /// <summary>
        ///     Returns true if the E is in the Empowered state.
        /// </summary>
        public bool IsWhiplashEmpowered()
        {
            return UtilityClass.Player.GetSpell(SpellSlot.E).ToggleState == 2;
        }

        /// <summary>
        ///     Returns true if the Q is in the Empowered state.
        /// </summary>
        public bool IsHateSpikeSkillshot()
        {
            return UtilityClass.Player.GetSpell(SpellSlot.Q).ToggleState == 1;
        }

        /// <summary>
        ///     Returns Last Caress' push back distance from the starting point.
        /// </summary>
        public int LastCaressPushBackDistance()
        {
            return 700;
        }

        /// <summary>
        ///     The real Sector part of the Last Caress.
        /// </summary>
        public Vector2Geometry.Sector RSector()
        {
            var targetPos = UtilityClass.Player.Path[1];
            var range = SpellClass.R.Range;
            var dir = (targetPos - UtilityClass.Player.ServerPosition).Normalized();
            var spot = targetPos + dir * range;

            return new Vector2Geometry.Sector((Vector2)targetPos, (Vector2)spot, SpellClass.R.Width, range);
        }

        /// <summary>
        ///     Draws the Sector part of the Last Caress.
        /// </summary>
        public Vector3Geometry.Sector DrawRSector()
        {
            var targetPos = UtilityClass.Player.Path[1];
            var range = SpellClass.R.Range;
            var dir = (targetPos - UtilityClass.Player.Position).Normalized();
            var spot = targetPos + dir * range;

            return new Vector3Geometry.Sector(targetPos, spot, SpellClass.Q2.Width, range);
        }
    }
}