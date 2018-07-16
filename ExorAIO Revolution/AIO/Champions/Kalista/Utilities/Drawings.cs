
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Caching;
using Entropy.SDK.Rendering;
using Entropy.SDK.Extensions.Objects;
using Color = SharpDX.Color;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Kalista
    {
        #region Public Methods and Operators

	    /// <summary>
	    ///     Initializes the drawings.
	    /// </summary>
	    public void OnRender(EntropyEventArgs args)
	    {
		    /// <summary>
		    ///     Loads the Q drawing.
		    /// </summary>
		    if (SpellClass.Q.Ready &&
		        MenuClass.Drawings["q"].Enabled)
		    {
			    CircleRendering.Render(Color.LightGreen, SpellClass.Q.Range, UtilityClass.Player);
		    }

		    /// <summary>
		    ///     Loads the W drawing.
		    /// </summary>
		    if (SpellClass.W.Ready &&
		        MenuClass.Drawings["w"].Enabled)
		    {
			    CircleRendering.Render(Color.Yellow, SpellClass.W.Range, UtilityClass.Player);
		    }

		    /// <summary>
		    ///     Loads the E drawing.
		    /// </summary>
		    if (SpellClass.E.Ready &&
		        MenuClass.Drawings["e"].Enabled)
		    {
			    CircleRendering.Render(Color.Cyan, SpellClass.E.Range, UtilityClass.Player);
		    }

		    /// <summary>
		    ///     Loads the R drawing.
		    /// </summary>
		    if (SpellClass.R.Ready &&
		        MenuClass.Drawings["r"].Enabled)
		    {
			    CircleRendering.Render(Color.Red, SpellClass.R.Range, UtilityClass.Player);
		    }

		    /// <summary>
		    ///     Loads the Soulbound drawing.
		    /// </summary>
		    if (SoulBound != null &&
		        MenuClass.Drawings["soulbound"].Enabled)
		    {
			    for (var i = 0; i < MenuClass.Drawings["soulbound"].Value; i++)
			    {
				    CircleRendering.Render(Color.Cyan, SoulBound.BoundingRadius + 5 * i, SoulBound);
			    }
		    }
		}

	    public void OnEndScene(EntropyEventArgs args)
	    {
			/// <summary>
			///     Draws the E damage to healthbar.
			/// </summary>
			if (SpellClass.E.Ready &&
				MenuClass.Drawings["edmg"].Enabled)
			{
				foreach (var hero in ObjectCache.EnemyHeroes.Where(t => t.IsValidTarget(SpellClass.E.Range)))
				{
					DamageIndicatorRendering.Render(hero, GetEDamage(hero));
				}

				foreach (var jungleMob in ObjectCache.JungleMinions.Where(t => t.IsJungleMinion() && t.IsValidTarget(SpellClass.E.Range)))
				{
					DamageIndicatorRendering.Render(jungleMob, GetEDamage(jungleMob));
				}

				foreach (var mob in ObjectCache.EnemyLaneMinions.Where(t => t.IsValidTarget(SpellClass.E.Range)))
				{
					DamageIndicatorRendering.Render(mob, GetEDamage(mob));
				}

			}
		}

        #endregion
    }
}