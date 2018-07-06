using AIO.Utilities;
using Entropy;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Twitch
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
            ///     The W Weaving Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                heroTarget.GetRealBuffCount("TwitchDeadlyVenom") <=
                    MenuClass.Spells["w"]["combo"].As<MenuSliderBool>().Value &&
                MenuClass.Spells["w"]["combo"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.W.Cast(heroTarget);
            }

            /// <summary>
            ///     The Q Weaving Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                SpellClass.Q.Cast();
            }
        }

        #endregion
    }
}