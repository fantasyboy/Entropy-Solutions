using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Caching;
using Entropy.SDK.Rendering;
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
				Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.Q.Range, Color.LightGreen);
			}

			/// <summary>
			///     Loads the W drawing.
			/// </summary>
			if (SpellClass.W.Ready &&
			    MenuClass.Drawings["w"].Enabled)
			{
				Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.W.Range, Color.Yellow);
			}

			/// <summary>
			///     Loads the E drawing.
			/// </summary>
			if (SpellClass.E.Ready &&
			    MenuClass.Drawings["e"].Enabled)
			{
				Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.E.Range, Color.Cyan);
			}

			/// <summary>
			///     Loads the R drawing.
			/// </summary>
			if (SpellClass.R.Ready &&
			    MenuClass.Drawings["r"].Enabled)
			{
				Renderer.DrawCircularRangeIndicator(UtilityClass.Player.Position, SpellClass.R.Range, Color.Red);
			}

			/// <summary>
			///     Loads the Soulbound drawing.
			/// </summary>
			if (SoulBound != null &&
			    MenuClass.Drawings["soulbound"].Enabled)
			{
				Renderer.DrawCircularRangeIndicator(SoulBound.Position, SoulBound.BoundingRadius, Color.Aquamarine);
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
				foreach (var hero in ObjectCache.EnemyHeroes.Where(IsPerfectRendTarget))
				{
					DamageIndicatorRendering.Render(hero, GetEDamage(hero));
				}

				var bigJungleMinions =
					Extensions.GetLargeJungleMinionsTargets().Concat(Extensions.GetLegendaryJungleMinionsTargets());
				foreach (var jungleMob in bigJungleMinions.Where(IsPerfectRendTarget))
				{
					DamageIndicatorRendering.Render(jungleMob, GetEDamage(jungleMob));
				}

				foreach (var mob in ObjectCache.EnemyLaneMinions.Where(IsPerfectRendTarget))
				{
					DamageIndicatorRendering.Render(mob, GetEDamage(mob));
				}
			}
		}

		#endregion
	}
}