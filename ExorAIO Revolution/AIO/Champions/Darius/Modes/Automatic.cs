
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
    internal partial class Darius
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(EntropyEventArgs args)
        {
            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The Automatic Q Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["q"]["aoe"] != null &&
                MenuClass.Spells["q"]["aoe"].As<MenuSliderBool>().Enabled)
            {
                var countValidTargets = GameObjects.EnemyHeroes.Count(t =>
                        !Invulnerable.Check(t) &&
                        t.IsValidTarget(SpellClass.Q.Range));
                if (countValidTargets >= MenuClass.Spells["q"]["aoe"].As<MenuSliderBool>().Value)
                {
                    SpellClass.Q.Cast();
                }
            }
        }

        #endregion
    }
}