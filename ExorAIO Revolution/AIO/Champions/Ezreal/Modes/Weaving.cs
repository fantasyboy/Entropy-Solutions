
using System.Linq;
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
    internal partial class Ezreal
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on do-cast.
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
                var buffMenu = MenuClass.Spells["w"]["buff"];
                if (buffMenu != null)
                {
                    if (UtilityClass.Player.TotalAbilityDamage < GetMinimumApForApMode() &&
                        UtilityClass.Player.ManaPercent()
                            > ManaManager.GetNeededMana(SpellClass.W.Slot, buffMenu["logical"]) &&
                        buffMenu["logical"].As<MenuSliderBool>().Enabled &&
                        GameObjects.AllyHeroes.Any(a =>
                            !a.IsMe &&
                            a.IsValidTarget(SpellClass.W.Range, true) &&
                            buffMenu["allywhitelist"][a.ChampionName.ToLower()].As<MenuBool>().Enabled))
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