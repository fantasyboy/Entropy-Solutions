
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Varus
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

            if (IsChargingPiercingArrow())
            {
                SpellClass.Q.Range = 925+UtilityClass.Player.BoundingRadius + 7*SpellClass.Q.ChargePercent;
            }

            /// <summary>
            ///     The Automatic R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["aoe"] != null &&
                MenuClass.Spells["r"]["aoe"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = GameObjects.EnemyHeroes
                    .Where(t =>
                        t.IsValidTarget(SpellClass.R.Range-150f) &&
                        !Invulnerable.Check(t, DamageType.Magical, false))
                    .MinBy(t2 => t2.CountEnemyHeroesInRange(550f));

                if (bestTarget != null &&
                    bestTarget.CountEnemyHeroesInRange(550f) >= MenuClass.Spells["r"]["aoe"].As<MenuSliderBool>().Value)
                {
                    SpellClass.R.Cast(bestTarget);
                }
            }

            /// <summary>
            ///     The Semi-Automatic R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["bool"].As<MenuBool>().Enabled &&
                MenuClass.Spells["r"]["key"].As<MenuKeyBind>().Enabled)
            {
                var bestTarget = GameObjects.EnemyHeroes.Where(t =>
                        t.IsValidTarget(SpellClass.R.Range-150f) &&
                        !Invulnerable.Check(t, DamageType.Magical, false) &&
                        MenuClass.Spells["r"]["whitelist"][t.CharName.ToLower()].As<MenuBool>().Enabled)
                    .MinBy(o => o.GetRealHealth());
                if (bestTarget != null)
                {
                    SpellClass.R.Cast(bestTarget);
                }
            }
        }

        #endregion
    }
}