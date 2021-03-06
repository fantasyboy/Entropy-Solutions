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
		public void OnRender(EntropyEventArgs args)
		{
			/// <summary>
			///     Loads the W drawing.
			/// </summary>
			if (SpellClass.W.Ready &&
			    MenuClass.Drawings["w"].Enabled)
			{
				Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.W.Range, Color.Yellow);
			}

			/// <summary>
			///     Loads the R drawings.
			/// </summary>
			if (SpellClass.R.Ready &&
			    MenuClass.Drawings["r"].Enabled)
			{
				Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.R.Range, Color.Red);
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

				var bigJungleMinions =
					Extensions.GetLargeJungleMinionsTargets().Concat(Extensions.GetLegendaryJungleMinionsTargets());
				foreach (var jungleMob in bigJungleMinions.Where(h => h.GetBuffCount("kaisapassivemarker") == 4))
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