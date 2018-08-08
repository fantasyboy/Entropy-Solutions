
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.UI.Components;
using Entropy.SDK.Utils;
using Entropy.SDK.Caching;

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
                DelayAction.Queue(() =>
                    {
                        Orbwalker.Move(Hud.CursorPositionUnclipped);
                    }, 100 + EnetClient.Ping);
            }

			/// <summary>
			///     The E Before death Logic.
			/// </summary>
			if (SpellClass.E.Ready &&
				MenuClass.E["beforedeath"].Enabled &&
				LocalPlayer.Instance.HPPercent() <= MenuClass.E["beforedeath"].Value)
			{
				SpellClass.E.Cast();
			}

			/// <summary>
			///     The Semi-Automatic R Management.
			/// </summary>
			if (SpellClass.R.Ready &&
                MenuClass.R["bool"].As<MenuBool>().Enabled &&
                MenuClass.R["key"].As<MenuKeyBind>().Enabled)
            {
                var bestTarget = ObjectCache.EnemyHeroes
                    .Where(t =>
                        !Invulnerable.Check(t) &&
                        t.IsValidTargetEx(SpellClass.R.Range) &&
                        MenuClass.R["whitelist"][t.CharName.ToLower()].As<MenuBool>().Enabled)
                    .MinBy(o => o.GetRealHealth(DamageType.Physical));
                if (bestTarget != null)
                {
                    if (MenuClass.R["mode"].As<MenuList>().Value == 1)
                    {
                        SpellClass.Q.Cast(bestTarget);
                    }

                    SpellClass.R.Cast(bestTarget);
                }
            }
        }

        #endregion
    }
}