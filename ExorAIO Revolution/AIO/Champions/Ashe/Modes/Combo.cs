
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
    internal partial class Ashe
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Combo(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.HasBuff("asheqcastready") &&
                GameObjects.EnemyHeroes.Any(t => t.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(t))) &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                SpellClass.Q.Cast();
            }

            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
            {
                var heroTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.W.Range);
                if (heroTarget != null)
                {
                    if (!Invulnerable.Check(heroTarget) &&
                        !heroTarget.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(heroTarget)))
                    {
                        SpellClass.W.Cast(heroTarget);
                    }
                }
            }
        }

        #endregion
    }
}