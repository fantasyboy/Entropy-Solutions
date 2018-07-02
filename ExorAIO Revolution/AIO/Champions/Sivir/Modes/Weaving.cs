
using Aimtec;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Sivir
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
            ///     The Q Weaving Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                SpellClass.Q.Cast(heroTarget);
                return;
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