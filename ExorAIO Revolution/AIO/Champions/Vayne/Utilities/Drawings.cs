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
	internal partial class Vayne
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
				Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.Q.Range, Color.LightGreen);
			}

			/// <summary>
			///     Loads the E drawings.
			/// </summary>
			if (SpellClass.E.Ready &&
			    MenuClass.Drawings["e"].Enabled)
			{
				Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.E.Range, Color.Cyan);
			}
		}

		public void OnEndScene(EntropyEventArgs args)
		{
			/// <summary>
			///     Draws the W damage to healthbar.
			/// </summary>
			if (MenuClass.Drawings["wdmg"].Enabled)
			{
				foreach (var hero in ObjectCache.EnemyHeroes.Where(h => h.GetBuffCount("vaynesilvereddebuff") == 2))
				{
					DamageIndicatorRendering.Render(hero, UtilityClass.Player.GetAutoAttackDamage(hero));
				}

				foreach (var jungleMob in ObjectCache.LargeJungleMinions.Where(t => t.GetBuffCount("vaynesilvereddebuff") == 2))
				{
					DamageIndicatorRendering.Render(jungleMob, UtilityClass.Player.GetAutoAttackDamage(jungleMob));
				}

				foreach (var mob in ObjectCache.EnemyLaneMinions.Where(h => h.GetBuffCount("vaynesilvereddebuff") == 2))
				{
					DamageIndicatorRendering.Render(mob, UtilityClass.Player.GetAutoAttackDamage(mob));
				}
			}
		}

		#endregion
	}
}