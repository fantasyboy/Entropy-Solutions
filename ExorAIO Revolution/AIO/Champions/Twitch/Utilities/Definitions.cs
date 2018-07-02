
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Damage.JSON;
using Entropy.SDK.Extensions;
using AIO.Utilities;

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Twitch
    {
        /// <summary>
        ///     Gets the total expunge damage on a determined unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public double GetTotalExpungeDamage(Obj_AI_Base unit)
        {
            var player = UtilityClass.Player;
            return player.GetSpellDamage(unit, SpellSlot.E) +
                   player.GetSpellDamage(unit, SpellSlot.E, DamageStage.Buff);
        }

        /// <summary>
        ///     Returns true if the target is a perfectly valid expunge target.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public bool IsPerfectExpungeTarget(Obj_AI_Base unit)
        {
            if (unit.HasBuff("twitchdeadlyvenom") &&
                unit.IsValidSpellTarget(SpellClass.E.Range))
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
    }
}
