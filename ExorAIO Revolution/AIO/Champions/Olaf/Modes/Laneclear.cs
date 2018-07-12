
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Olaf
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on post attack.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
        public void Laneclear(OnPreAttackEventArgs args)
        {
            /// <summary>
            ///     The Laneclear Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["laneclear"]) &&
                MenuClass.Spells["q"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                /*
                var farmLocation = SpellClass.Q.GetLinearFarmLocation(minions, SpellClass.Q.Width);
                if (farmLocation.MinionsHit >= MenuClass.Spells["q"]["customization"]["laneclear"].As<MenuSlider>().Value)
                {
                    SpellClass.Q.Cast(farmLocation.Position);
                }
                */
            }

            /// <summary>
            ///     The E Big Minions Lasthit Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["lasthit"].As<MenuBool>().Enabled)
            {
                foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.E.Range).Where(m =>
                    (m.CharName.Contains("Siege") || m.CharName.Contains("Super")) &&
                    m.GetRealHealth() < UtilityClass.Player.GetSpellDamage(m, SpellSlot.E)))
                {
                    SpellClass.E.CastOnUnit(minion);
                }
            }

            /// <summary>
            ///     The Laneclear W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.MPPercent()
                > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["w"]["laneclear"]) &&
                MenuClass.Spells["w"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                var mainMinion = Orbwalker.GetOrbwalkingTarget() as AIMinionClient;
                if (mainMinion != null)
                {
                    SpellClass.W.Cast();
                }
            }
        }

        #endregion
    }
}