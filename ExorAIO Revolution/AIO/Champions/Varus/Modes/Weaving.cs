
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;

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
        
        /// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
        public void Weaving(OnPostAttackEventArgs args)
        {
            var heroTarget = args.Target as AIHeroClient;
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
                if (UtilityClass.Player.Spellbook.GetSpell(SpellSlot.W).State.HasFlag(SpellState.NotLearned) ||
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
                if (UtilityClass.Player.Spellbook.GetSpell(SpellSlot.W).State.HasFlag(SpellState.NotLearned) ||
                    GetBlightStacks(heroTarget) >= MenuClass.Spells["q"]["customization"]["combostacks"].As<MenuSlider>().Value - 1)
                {
                    SpellClass.Q.StartCharging(heroTarget.Position);
                }
            }
        }

        #endregion
    }
}