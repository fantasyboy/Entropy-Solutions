
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class MissFortune
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Combo()
        {
            /// <summary>
            ///     The Extended Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q2"]["combo"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q2.Range).Where(t => MenuClass.Spells["q2"]["whitelist"][t.ChampionName.ToLower()].Enabled))
                {
                    var unitsToIterate = Extensions.GetAllGenericUnitTargetsInRange(SpellClass.Q.Range)
                        .Where(m => !m.IsMoving && QCone(m).IsInside((Vector2)target.ServerPosition))
                        .OrderBy(m => m.Health)
                        .ToList();

                    var killableUnitsToIterate = unitsToIterate
                        .Where(m => m.GetRealHealth() < GetRealMissFortuneDamage(UtilityClass.Player.GetSpellDamage(m, SpellSlot.Q), m))
                        .ToList();

                    var realUnitsToIterate = killableUnitsToIterate.Any() && MenuClass.Spells["q2"]["customization"]["combo"].As<MenuBool>().Enabled ? killableUnitsToIterate : unitsToIterate;
                    foreach (var minion in realUnitsToIterate)
                    {
                        UtilityClass.CastOnUnit(SpellClass.Q, minion);
                        break;
                    }
                }
            }

            /// <summary>
            ///     The Harass E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical, false))
                {
                    SpellClass.E.Cast(bestTarget);
                }
            }
        }

        #endregion
    }
}