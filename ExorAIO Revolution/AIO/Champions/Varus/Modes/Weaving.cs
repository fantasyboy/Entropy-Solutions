
using Entropy;
using Entropy.SDK.Menu.Components;
using Entropy.SDK.Orbwalking;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Varus
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on post attack.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PostAttackEventArgs" /> instance containing the event data.</param>
        public void Weaving(object sender, PostAttackEventArgs args)
        {
            var heroTarget = args.Target as Obj_AI_Hero;
            if (heroTarget == null)
            {
                return;
            }

            /// <summary>
            ///     The E Weaving Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                !Invulnerable.Check(heroTarget) &&
                MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
            {
                if (UtilityClass.Player.SpellBook.GetSpell(SpellSlot.W).State.HasFlag(SpellState.NotLearned) ||
                    GetBlightStacks(heroTarget) >= MenuClass.Spells["e"]["customization"]["combostacks"].As<MenuSlider>().Value - 1)
                {
                    SpellClass.E.Cast(heroTarget);
                }
            }

            /// <summary>
            ///     The Q Weaving Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                !Invulnerable.Check(heroTarget) &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                if (UtilityClass.Player.SpellBook.GetSpell(SpellSlot.W).State.HasFlag(SpellState.NotLearned) ||
                    GetBlightStacks(heroTarget) >= MenuClass.Spells["q"]["customization"]["combostacks"].As<MenuSlider>().Value - 1)
                {
                    SpellClass.Q.StartCharging(heroTarget.ServerPosition);
                }
            }
        }

        #endregion
    }
}