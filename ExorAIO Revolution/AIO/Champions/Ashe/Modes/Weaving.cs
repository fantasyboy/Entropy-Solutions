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
    internal partial class Ashe
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
                MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
            {
                if (!UtilityClass.Player.HasBuff("AsheQAttack") ||
                    !MenuClass.Spells["w"]["customization"]["nowflurry"].As<MenuBool>().Enabled)
                {
                    SpellClass.W.Cast(heroTarget);
                }
            }
        }

        #endregion
    }
}