
using Entropy;
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
    internal partial class Caitlyn
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