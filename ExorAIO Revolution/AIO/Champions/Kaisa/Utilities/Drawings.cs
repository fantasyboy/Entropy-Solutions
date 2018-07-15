
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Caching;
using Entropy.SDK.Damage;
using Entropy.SDK.Rendering;
using Entropy.SDK.Extensions.Objects;
using Color = SharpDX.Color;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Kaisa
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the drawings.
        /// </summary>
        public void OnPresent(EntropyEventArgs args)
        {
            /// <summary>
            ///     Loads the W drawing.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Drawings["w"].Enabled)
            {
                CircleRendering.Render(Color.Yellow, SpellClass.W.Range, UtilityClass.Player);
            }

            /// <summary>
            ///     Loads the R drawings.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Drawings["r"].Enabled)
            {
                CircleRendering.Render(Color.Red, SpellClass.R.Range, UtilityClass.Player);
            }
        }

	    public void OnEndScene(EntropyEventArgs args)
	    {
			/// <summary>
			///     Draws the Passive damage to healthbar.
			/// </summary>
			if (MenuClass.Drawings["passivedmg"].Enabled)
		    {
			    foreach (var hero in ObjectCache.EnemyHeroes.Where(h => h.GetBuffCount("kaisapassivemarker") == 4))
			    {
				    DamageIndicatorRendering.Render(hero, UtilityClass.Player.GetAutoAttackDamage(hero));
			    }

			    foreach (var jungleMob in ObjectCache.JungleMinions.Where(t => t.IsJungleMinion() && t.GetBuffCount("kaisapassivemarker") == 4))
			    {
					DamageIndicatorRendering.Render(jungleMob, UtilityClass.Player.GetAutoAttackDamage(jungleMob));
				}

			    foreach (var mob in ObjectCache.EnemyLaneMinions.Where(h => h.GetBuffCount("kaisapassivemarker") == 4))
			    {
					DamageIndicatorRendering.Render(mob, UtilityClass.Player.GetAutoAttackDamage(mob));
				}
		    }
	    }

		#endregion
	}
}