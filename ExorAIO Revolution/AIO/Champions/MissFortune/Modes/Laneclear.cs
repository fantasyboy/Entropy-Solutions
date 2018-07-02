
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;
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
        ///     Fired when the game is updated.
        /// </summary>
        public void Laneclear()
        {
            /// <summary>
            ///     The Q Laneclear Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["laneclear"]) &&
                MenuClass.Spells["q"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                var bestMinion = Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.Q.Range)
                    .Where(m => m.Health < UtilityClass.Player.GetSpellDamage(m, SpellSlot.Q))
                    .FirstOrDefault(m => Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.Q2.Range)
                     .Any(m2 =>
                        m2.NetworkId != m.NetworkId &&
                        QCone(m).IsInside((Vector2)m2.ServerPosition) &&
                        m2.Health < UtilityClass.Player.GetSpellDamage(m2, SpellSlot.Q, DamageStage.Empowered) + GetMinionLoveTapDamageMultiplier()));
                if (bestMinion != null)
                {
                    UtilityClass.CastOnUnit(SpellClass.Q, bestMinion);
                }
            }

            /// <summary>
            ///     The Laneclear W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["laneclear"]) &&
                MenuClass.Spells["w"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                if (Extensions.GetEnemyLaneMinionsTargetsInRange(UtilityClass.Player.AttackRange).Any())
                {
                    SpellClass.W.Cast();
                }
            }

            /// <summary>
            ///     The Laneclear E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["laneclear"]) &&
                MenuClass.Spells["e"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                /*
                var farmLocation = SpellClass.E.GetCircularFarmLocation(Extensions.GetEnemyLaneMinionsTargets(), SpellClass.E.Width);
                if (farmLocation.MinionsHit >= MenuClass.Spells["e"]["customization"]["laneclear"].As<MenuSlider>().Value)
                {
                    SpellClass.E.Cast(farmLocation.Position);
                }
                */
            }
        }

        #endregion
    }
}