
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using Entropy.SDK.Orbwalking;
using AIO.Utilities;

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
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PreAttackEventArgs" /> instance containing the event data.</param>
        public void Laneclear(object sender, PreAttackEventArgs args)
        {
            /// <summary>
            ///     The Laneclear E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.ManaPercent()
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
                    UtilityClass.CastOnUnit(SpellClass.E, idealMinion);
                }
            }

            var minionTarget = ImplementationClass.IOrbwalker.GetOrbwalkingTarget() as Obj_AI_Minion;
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