
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Caching;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;

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
                MenuClass.Q["combo"].Enabled)
            {
                SpellClass.Q.Cast(heroTarget);
                return;
            }

            /// <summary>
            ///     The W Weaving Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.W["combo"].Enabled)
            {
                var buffMenu = MenuClass.W["buff"];
                if (buffMenu != null)
                {
                    if (UtilityClass.PlayerData.TotalAbilityDamage() < GetMinimumApForApMode() &&
                        UtilityClass.Player.MPPercent()
                            > ManaManager.GetNeededMana(SpellClass.W.Slot, buffMenu["logical"]) &&
                        buffMenu["logical"].Enabled &&
                        ObjectCache.AllyHeroes.Any(a =>
                            !a.IsMe() &&
                            a.IsValidTarget(SpellClass.W.Range, true) &&
                            buffMenu["allywhitelist"][a.CharName.ToLower()].Enabled))
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