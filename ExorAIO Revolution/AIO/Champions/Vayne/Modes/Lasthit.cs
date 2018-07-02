using System.Linq;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using Entropy.SDK.Orbwalking;
using AIO.Utilities;

#pragma warning disable 1587
namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Vayne
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PostAttackEventArgs" /> instance containing the event data.</param>
        public void Lasthit(object sender, PostAttackEventArgs args)
        {
            /// <summary>
            ///     The Q FarmHelper Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["farmhelper"]) &&
                MenuClass.Spells["q"]["farmhelper"].As<MenuSliderBool>().Enabled)
            {
                var posAfterQ = UtilityClass.Player.Position.Extend(Game.CursorPos, 300f);
                if (Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.Q.Range).Any(m =>
                        m.Distance(posAfterQ) < UtilityClass.Player.AttackRange &&
                        m != ImplementationClass.IOrbwalker.GetOrbwalkingTarget() &&
                        posAfterQ.CountEnemyHeroesInRange(UtilityClass.Player.GetFullAttackRange(m)) <= 2 &&
                        m.GetRealHealth() <
                            UtilityClass.Player.GetAutoAttackDamage(m) +
                            UtilityClass.Player.GetSpellDamage(m, SpellSlot.Q)))
                {
                    SpellClass.Q.Cast(Game.CursorPos);
                }
            }
        }

        #endregion
    }
}