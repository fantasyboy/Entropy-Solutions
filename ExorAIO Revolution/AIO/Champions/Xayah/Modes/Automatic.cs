
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.UI.Components;
using Entropy.SDK.Utils;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Xayah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(EntropyEventArgs args)
        {
            Orbwalker.AttackingEnabled = !IsFlying();
            Orbwalker.MovingEnabled = Game.ClockTime - LastCastedETime >= 0.55;

            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The Automatic R Orbwalking.
            /// </summary>
            if (IsFlying())
            {
                DelayAction.Queue(100 + Game.Ping, () =>
                    {
                        UtilityClass.Player.IssueOrder(OrderType.MoveTo, Hud.CursorPositionUnclipped);
                    });
            }

            /// <summary>
            ///     The Semi-Automatic R Management.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["bool"].As<MenuBool>().Enabled &&
                MenuClass.Spells["r"]["key"].As<MenuKeyBind>().Enabled)
            {
                var bestTarget = GameObjects.EnemyHeroes
                    .Where(t =>
                        !Invulnerable.Check(t) &&
                        t.IsValidTarget(SpellClass.R.Range) &&
                        MenuClass.Spells["r"]["whitelist"][t.CharName.ToLower()].As<MenuBool>().Enabled)
                    .MinBy(o => o.GetRealHealth());
                if (bestTarget != null)
                {
                    if (MenuClass.Spells["r"]["mode"].As<MenuList>().Value == 1)
                    {
                        SpellClass.Q.Cast(bestTarget);
                    }

                    SpellClass.R.Cast(bestTarget);
                }
            }
        }

        /// <summary>
        ///     Fired as fast as possible.
        /// </summary>
        public void BladeCallerAutomatic(EntropyEventArgs args)
        {
            /// <summary>
            ///     The E Before death Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["ondeath"].As<MenuBool>().Enabled &&
                ImplementationClass.IHealthPrediction.GetPrediction(UtilityClass.Player, 1000 + Game.Ping) <= 0)
            {
                SpellClass.E.Cast();
            }
        }

        #endregion
    }
}