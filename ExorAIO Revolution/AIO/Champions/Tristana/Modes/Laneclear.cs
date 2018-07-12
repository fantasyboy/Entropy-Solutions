
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Tristana
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
        public void Laneclear(OnPreAttackEventArgs args)
        {
            /// <summary>
            ///     The Laneclear E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["laneclear"]) &&
                MenuClass.Spells["e"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                var idealMinion = Extensions.GetEnemyLaneMinionsTargets().FirstOrDefault(
                    m =>
                        m.IsValidTarget(SpellClass.E.Range) &&
                        Extensions.GetEnemyLaneMinionsTargets()
                            .Count(m2 => m2.Distance(m) < 200f) >= MenuClass.Spells["e"]["customization"]["laneclear"].Value);

                if (idealMinion != null)
                {
                    SpellClass.E.CastOnUnit(idealMinion);
                }
            }

            var minionTarget = Orbwalker.GetOrbwalkingTarget() as AIMinionClient;
            if (minionTarget == null ||
                !Extensions.GetEnemyLaneMinionsTargets().Contains(minionTarget))
            {
                return;
            }

            /// <summary>
            ///     The Laneclear Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["laneclear"].As<MenuBool>().Enabled)
            {
                SpellClass.Q.Cast();
            }
        }

        #endregion
    }
}