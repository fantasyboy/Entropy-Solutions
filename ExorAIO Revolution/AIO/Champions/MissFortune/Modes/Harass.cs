
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;
using SharpDX;

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
        ///     Fired when the game is updated.
        /// </summary>
        public void Harass(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Q Mixed Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q2"]["harass"]) &&
                MenuClass.Spells["q2"]["harass"].As<MenuSliderBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q2.Range).Where(t => MenuClass.Spells["q2"]["whitelist"][t.CharName.ToLower()].Enabled))
                {
                    var unitsToIterate = Extensions.GetAllGenericUnitTargetsInRange(SpellClass.Q.Range)
                        .Where(m => !m.IsMoving && QCone(m).IsInside((Vector2)target.Position))
                        .OrderBy(m => m.HP)
                        .ToList();

                    var killableUnitsToIterate = unitsToIterate
                        .Where(m => m.GetRealHealth() < GetRealMissFortuneDamage(UtilityClass.Player.GetSpellDamage(m, SpellSlot.Q), m))
                        .ToList();

                    var realUnitsToIterate = killableUnitsToIterate.Any() && MenuClass.Spells["q2"]["customization"]["harass"].As<MenuBool>().Enabled ? killableUnitsToIterate : unitsToIterate;
                    foreach (var minion in realUnitsToIterate)
                    {
                        SpellClass.Q.CastOnUnit(minion);
                        break;
                    }
                }
            }

            /// <summary>
            ///     The Harass E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["harass"]) &&
                MenuClass.Spells["e"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical, false) &&
                    MenuClass.Spells["e"]["whitelist"][bestTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                {
                    SpellClass.E.Cast(bestTarget);
                }
            }
        }

        #endregion
    }
}