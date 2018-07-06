
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;

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
        public double GetTotalExpungeDamage(AIBaseClient unit)
        {
            var player = UtilityClass.Player;
            return player.GetSpellDamage(unit, SpellSlot.E) +
                   player.GetSpellDamage(unit, SpellSlot.E, DamageStage.Buff);
        }

        /// <summary>
        ///     Returns true if the target is a perfectly valid expunge target.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public bool IsPerfectExpungeTarget(AIBaseClient unit)
        {
            if (unit.HasBuff("twitchdeadlyvenom") &&
                unit.IsValidSpellTarget(SpellClass.E.Range))
            {
                switch (unit.Type)
                {
                    case GameObjectType.AIMinionClient:
                        return true;

                    case GameObjectType.AIHeroClient:
                        var heroUnit = (AIHeroClient)unit;
                        return !Invulnerable.Check(heroUnit);
                }
            }

            return false;
        }
    }
}
