using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;

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
        
        /// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
        public void Weaving(OnPreAttackEventArgs args)
        {
            var heroTarget = args.Target as AIHeroClient;
            if (heroTarget == null)
            {
                return;
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Q["combo"].As<MenuBool>().Enabled)
            {
                if (MenuClass.Miscellaneous["feathersweaving"].As<MenuBool>().Enabled)
                {
                    if (UtilityClass.Player.GetBuffCount("XayahPassiveActive") <= 3)
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
                MenuClass.W["combo"].As<MenuBool>().Enabled)
            {
                if (MenuClass.Miscellaneous["feathersweaving"].As<MenuBool>().Enabled)
                {
                    if (UtilityClass.Player.GetBuffCount("XayahPassiveActive") <= 3)
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