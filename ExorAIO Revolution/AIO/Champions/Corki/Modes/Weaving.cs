
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
    internal partial class Corki
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
            ///     The E Combo Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                !Invulnerable.Check(heroTarget, DamageType.Magical) &&
                MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
            {
                SpellClass.E.Cast();
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                !Invulnerable.Check(heroTarget, DamageType.Magical) &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                SpellClass.Q.Cast(heroTarget);
                return;
            }

            /// <summary>
            ///     The R Combo Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                !Invulnerable.Check(heroTarget, DamageType.Magical) &&
                MenuClass.Spells["r"]["combo"].As<MenuBool>().Enabled)
            {
                SpellClass.R.Cast(heroTarget);
            }
        }

        /// <summary>
        ///     Called on pre attack.
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
            ///     The E Combo Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                !Invulnerable.Check(heroTarget, DamageType.Magical) &&
                MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
            {
                SpellClass.E.Cast();
            }
        }

        #endregion
    }
}