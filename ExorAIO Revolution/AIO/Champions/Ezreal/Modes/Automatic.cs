
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.UI.Components;
using Entropy.SDK.Caching;

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
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(EntropyEventArgs args)
        {
            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The Anti-Grab Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.IsBeingGrabbed() &&
                MenuClass.E["antigrab"].Enabled)
            {
                var firstTower = ObjectCache.AllyTurrets
                    .Where(t => t.IsValidTargetEx(allyIsValidTargetEx: true))
                    .MinBy(t => t.DistanceToPlayer());
                if (firstTower != null)
                {
                    SpellClass.E.Cast(UtilityClass.Player.Position.Extend(firstTower.Position, SpellClass.E.Range));
                }
            }

            /// <summary>
            ///     The Tear Stacking Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.IsTearLikeItemReady() &&
                UtilityClass.Player.EnemyHeroesCount(1500f) == 0 &&
                Orbwalker.Mode == OrbwalkingMode.None &&
                !Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.Q.Range).Any() &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Miscellaneous["tear"]) &&
                MenuClass.Miscellaneous["tear"].Enabled)
            {
                SpellClass.Q.Cast(Hud.CursorPositionUnclipped);
            }

            /// <summary>
            ///     The Semi-Automatic R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.R["bool"].Enabled &&
                MenuClass.R["key"].As<MenuKeyBind>().Enabled)
            {
                var bestTarget = ObjectCache.EnemyHeroes.Where(t =>
                        t.IsValidTargetEx(2000f) &&
                        !Invulnerable.Check(t, DamageType.Magical, false) &&
                        MenuClass.R["whitelist"][t.CharName.ToLower()].Enabled)
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