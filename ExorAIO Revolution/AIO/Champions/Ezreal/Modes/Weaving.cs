
using System.Linq;
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
    internal partial class Ezreal
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on do-cast.
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
            ///     The W Weaving Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
            {
                var buffMenu = MenuClass.Spells["w"]["buff"];
                if (buffMenu != null)
                {
                    if (UtilityClass.Player.TotalAbilityDamage < GetMinimumApForApMode() &&
                        UtilityClass.Player.MPPercent()
                            > ManaManager.GetNeededMana(SpellClass.W.Slot, buffMenu["logical"]) &&
                        buffMenu["logical"].As<MenuSliderBool>().Enabled &&
                        GameObjects.AllyHeroes.Any(a =>
                            !a.IsMe() &&
                            a.IsValidTarget(SpellClass.W.Range, true) &&
                            buffMenu["allywhitelist"][a.CharName.ToLower()].As<MenuBool>().Enabled))
                    {
                        return;
                    }
                }

                SpellClass.W.Cast(heroTarget);
            }
        }

        #endregion
    }
}