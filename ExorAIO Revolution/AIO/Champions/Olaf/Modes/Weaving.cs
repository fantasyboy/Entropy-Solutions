
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
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
        public void Weaving(OnPreAttackEventArgs args)
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
                MenuClass.Spells["e"]["combo"].As<MenuSliderBool>().Enabled)
            {
                if (UtilityClass.Player.HPPercent()
                        >= MenuClass.Spells["e"]["combo"].As<MenuSliderBool>().Value ||
                    UtilityClass.Player.GetSpellDamage(heroTarget, SpellSlot.E) >= heroTarget.GetRealHealth())
                {
                    SpellClass.E.CastOnUnit(heroTarget);
                }
            }

            /// <summary>
            ///     The W Weaving Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
            {
                SpellClass.W.Cast();
            }
        }

        #endregion
    }
}