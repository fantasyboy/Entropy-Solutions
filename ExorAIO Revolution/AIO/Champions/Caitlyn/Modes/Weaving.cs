using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Caitlyn
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
                !Invulnerable.Check(heroTarget) &&
                MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
            {
                if (heroTarget.IsValidTarget(SpellClass.E.Range - 250f) &&
                    heroTarget.Distance(SpellClass.E.GetPrediction(heroTarget).CastPosition) < SpellClass.E.Range - 300f)
                {
                    SpellClass.E.Cast(heroTarget);
                }
            }
        }

        #endregion
    }
}