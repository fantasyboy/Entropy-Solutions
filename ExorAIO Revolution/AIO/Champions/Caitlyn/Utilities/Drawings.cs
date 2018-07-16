
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Caching;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Rendering;
using Color = SharpDX.Color;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Caitlyn
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
	    }

	    public void OnEndScene(EntropyEventArgs args)
	    {
		    /// <summary>
		    ///     Loads the R drawings.
		    /// </summary>
		    if (SpellClass.R.Ready)
		    {
			    /// <summary>
			    ///     Draws the R range into minimap.
			    /// </summary>
				if (MenuClass.Drawings["rmm"].Enabled)
			    {
				    TacticalMapRendering.Render(Color.White, UtilityClass.Player.Position, SpellClass.R.Range);
			    }

			    /// <summary>
			    ///     Draws the R damage into enemy healthbar.
			    /// </summary>
				if (MenuClass.Drawings["rdmg"].Enabled)
			    {
				    foreach (var hero in ObjectCache.EnemyHeroes.Where(t => t.IsValidTarget(SpellClass.R.Range)))
				    {
					    DamageIndicatorRendering.Render(hero, GetRDamage(hero));
				    }
			    }
			}

			/// <summary>
			///     Draws the Passive damage to healthbar.
			/// </summary>
			if (MenuClass.Drawings["passivedmg"].Enabled)
		    {
			    foreach (var hero in ObjectCache.EnemyHeroes.Where(t => t.IsValidTarget(1250f)))
			    {
				    if (UtilityClass.Player.HasBuff("caitlynheadshot") ||
				        UtilityClass.Player.HasBuff("caitlynheadshotrangecheck") &&
				        hero.HasBuff("caitlynyordletrapinternal"))
				    {
					    DamageIndicatorRendering.Render(hero, UtilityClass.Player.GetAutoAttackDamage(hero));
					}
			    }

			    if (UtilityClass.Player.HasBuff("caitlynheadshot"))
			    {
				    foreach (var jungleMob in ObjectCache.JungleMinions.Where(t => t.IsJungleMinion() && t.IsValidTarget(1250f)))
				    {
					    DamageIndicatorRendering.Render(jungleMob, UtilityClass.Player.GetAutoAttackDamage(jungleMob));
				    }

				    foreach (var mob in ObjectCache.EnemyLaneMinions.Where(t => t.IsValidTarget(1250f)))
				    {
					    DamageIndicatorRendering.Render(mob, UtilityClass.Player.GetAutoAttackDamage(mob));
				    }
				}
		    }
		}

        #endregion
    }
}