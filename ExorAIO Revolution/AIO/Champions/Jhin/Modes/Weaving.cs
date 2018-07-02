
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
    internal partial class Jhin
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
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                !HasFourthShot() &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                UtilityClass.CastOnUnit(SpellClass.Q, heroTarget);
                return;
            }

            /// <summary>
            ///     The E Combo Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                HasFourthShot() &&
                MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
            {
                SpellClass.E.Cast(heroTarget);
            }
        }

        /// <summary>
        ///     Called on pre attack.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PreAttackEventArgs" /> instance containing the event data.</param>
        public void Weaving(object sender, PreAttackEventArgs args)
        {
            var heroTarget = args.Target as Obj_AI_Hero;
            if (heroTarget == null)
            {
                return;
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                HasFourthShot() &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                UtilityClass.CastOnUnit(SpellClass.Q, heroTarget);
            }
        }

        #endregion
    }
}