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
    internal partial class KogMaw
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
            ///     The Q Weaving Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                SpellClass.Q.Cast(heroTarget);
                return;
            }

            /// <summary>
            ///     The E Weaving Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
            {
                if (MenuClass.Miscellaneous["onlyeoutaarange"].As<MenuBool>().Enabled)
                {
                    return;
                }

                if (IsUsingBioArcaneBarrage() &&
                    MenuClass.Miscellaneous["onlyeoutw"].As<MenuBool>().Enabled)
                {
                    return;
                }

                SpellClass.E.Cast(heroTarget);
            }
        }

        #endregion
    }
}