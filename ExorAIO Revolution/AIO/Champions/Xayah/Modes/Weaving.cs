
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
    internal partial class Xayah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on post attack.
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
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                if (MenuClass.Miscellaneous["feathersweaving"].As<MenuBool>().Enabled)
                {
                    if (UtilityClass.Player.GetRealBuffCount("XayahPassiveActive") <= 3)
                    {
                        SpellClass.Q.Cast(heroTarget);
                        return;
                    }
                }
                else
                {
                    SpellClass.Q.Cast(heroTarget);
                }
            }

            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
            {
                if (MenuClass.Miscellaneous["feathersweaving"].As<MenuBool>().Enabled)
                {
                    if (UtilityClass.Player.GetRealBuffCount("XayahPassiveActive") <= 3)
                    {
                        SpellClass.W.Cast();
                    }
                }
                else
                {
                    SpellClass.W.Cast();
                }
            }
        }

        #endregion
    }
}