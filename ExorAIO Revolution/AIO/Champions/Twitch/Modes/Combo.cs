
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Objects;
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
        ///     Called on tick update.
        /// </summary>
        public void Combo(EntropyEventArgs args)
        {
            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["aoe"] != null &&
                MenuClass.Spells["w"]["aoe"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.W.Range);
                if (bestTarget.IsValidTarget() &&
                    bestTarget.CountEnemyHeroesInRange(SpellClass.W.Width) >= MenuClass.Spells["w"]["aoe"].As<MenuSliderBool>().Value)
                {
                    SpellClass.W.Cast(bestTarget);
                }
            }
        }

        /// <summary>
        ///     Fired as fast as possible.
        /// </summary>
        public void ExpungeCombo(args)
        {
            /// <summary>
            ///     The Automatic Enemy E Logic.
            /// </summary>
            if (MenuClass.Spells["e"]["logical"].As<MenuSliderBool>().Enabled)
            {
                if (GameObjects.EnemyHeroes.Any(t =>
                    !Invulnerable.Check(t) &&
                    t.IsValidTarget(SpellClass.E.Range) &&
                    t.GetRealBuffCount("twitchdeadlyvenom") >=
                    MenuClass.Spells["e"]["logical"].As<MenuSliderBool>().Value))
                {
                    SpellClass.E.Cast();
                }
            }
        }

        #endregion
    }
}