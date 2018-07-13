
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Caching;
using Entropy.SDK.Rendering;
using Entropy.SDK.UI.Components;
using Entropy.SDK.Extensions.Objects;
using Color = SharpDX.Color;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Vayne
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the drawings.
        /// </summary>
        public void Drawings()
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
            ///     Loads the E drawings.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Drawings["e"].As<MenuBool>().Enabled)
            {
                CircleRendering.Render(Color.Cyan, SpellClass.E.Range, UtilityClass.Player);
            }
        }

	    public void OnEndScene(EntropyEventArgs args)
	    {
		    /// <summary>
		    ///     Loads the E damage to healthbar.
		    /// </summary>
		    if (MenuClass.Drawings["wdmg"].As<MenuBool>().Enabled)
		    {
			    foreach (var hero in ObjectCache.EnemyHeroes)
			    {
				    DamageIndicatorRendering.Render(hero, GetWDamage(hero));
			    }

			    foreach (var jungleMob in ObjectCache.JungleMinions.Where(t => t.IsJungleMinion()))
			    {
					DamageIndicatorRendering.Render(jungleMob, GetWDamage(jungleMob));
				}

			    foreach (var mob in ObjectCache.EnemyLaneMinions)
			    {
					DamageIndicatorRendering.Render(mob, GetWDamage(mob));
				}

		    }
	    }

		#endregion
	}
}