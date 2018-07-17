
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Utils;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Lucian
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(EntropyEventArgs args)
        {
	        /// <summary>
	        ///     The Automatic R Orbwalking.
	        /// </summary>
	        if (MenuClass.Root["r"]["bool"].Enabled &&
	            MenuClass.Root["r"]["key"].Enabled)
	        {
		        DelayAction.Queue(() =>
			        {
				        Orbwalker.Move(Hud.CursorPositionUnclipped);
			        }, 100 + EnetClient.Ping);
	        }

			/// <summary>
			///     The Semi-Automatic R Management.
			/// </summary>
			if (SpellClass.R.Ready &&
                MenuClass.Root["r"]["bool"].Enabled)
            {
                var bestTarget = GameObjects.EnemyHeroes
                    .Where(t =>
                        !Invulnerable.Check(t) &&
                        t.IsValidTarget(SpellClass.R.Range) &&
                        MenuClass.Root["r"]["whitelist"][t.CharName.ToLower()].Enabled)
                    .MinBy(o => o.GetRealHealth());

                if (!IsCulling() &&
                    bestTarget != null &&
                    MenuClass.Root["r"]["key"].Enabled)
                {
                    if (SpellClass.W.Ready &&
                        bestTarget.IsValidTarget(SpellClass.W.Range))
                    {
                        SpellClass.W.Cast(bestTarget.Position);
                    }
                    SpellClass.R.Cast(bestTarget.Position);
                }
                else if (UtilityClass.Player.HasBuff("LucianR") &&
                     !MenuClass.Root["r"]["key"].Enabled)
                {
                    SpellClass.R.Cast();
                }
            }
        }

        #endregion
    }
}