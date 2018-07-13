
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Caching;
using Entropy.SDK.Rendering;
using Entropy.SDK.UI.Components;
using SharpDX;
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
		        MenuClass.Drawings["q"].As<MenuBool>().Enabled)
		    {
			    CircleRendering.Render(Color.LightGreen, SpellClass.Q.Range, UtilityClass.Player);
		    }

		    /// <summary>
		    ///     Loads the W drawing.
		    /// </summary>
		    if (SpellClass.W.Ready &&
		        MenuClass.Drawings["w"].As<MenuBool>().Enabled)
		    {
			    CircleRendering.Render(Color.Yellow, SpellClass.W.Range, UtilityClass.Player);
		    }

		    /// <summary>
		    ///     Loads the E drawing.
		    /// </summary>
		    if (SpellClass.E.Ready &&
		        MenuClass.Drawings["e"].As<MenuBool>().Enabled)
		    {
			    CircleRendering.Render(Color.Cyan, SpellClass.E.Range, UtilityClass.Player);
		    }

		    /// <summary>
		    ///     Loads the R drawing.
		    /// </summary>
		    if (SpellClass.R.Ready &&
		        MenuClass.Drawings["r"].As<MenuBool>().Enabled)
		    {
			    CircleRendering.Render(Color.Red, SpellClass.R.Range, UtilityClass.Player);
		    }

		    /// <summary>
		    ///     Loads the Soulbound drawing.
		    /// </summary>
		    if (SoulBound != null &&
		        MenuClass.Drawings["soulbound"].As<MenuSliderBool>().Enabled)
		    {
			    for (var i = 0; i < MenuClass.Drawings["soulbound"].As<MenuSliderBool>().Value; i++)
			    {
				    CircleRendering.Render(Color.Cyan, SoulBound.BoundingRadius + 5 * i, SoulBound);
			    }
		    }
		}

	    public void OnEndScene(EntropyEventArgs args)
	    {
			/// <summary>
			///     Loads the E damage to healthbar.
			/// </summary>
			if (SpellClass.E.Ready &&
				MenuClass.Drawings["edmg"].As<MenuBool>().Enabled)
			{
				foreach (var hero in ObjectCache.EnemyHeroes.Where(t => t.IsValidTarget(SpellClass.E.Range) && t.IsVisibleOnScreen))
				{
					DamageIndicatorRendering.Render(hero, GetEDamage(hero));
				}

				foreach (var mob in ObjectCache.JungleMinions.Where(t => t.IsValidTarget(SpellClass.E.Range) && t.IsVisibleOnScreen))
				{
					DamageIndicatorRendering.Render(mob, GetEDamage(mob));
				}

				foreach (var mob in ObjectCache.EnemyLaneMinions.Where(t => t.IsValidTarget(SpellClass.E.Range) && t.IsVisibleOnScreen))
				{
					DamageIndicatorRendering.Render(mob, GetEDamage(mob));
				}

			}
		}

        #endregion
    }
}