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
					var scale = (TacticalMap.ScaleX + TacticalMap.ScaleY) / 4.5;
					TacticalMapRendering.Render(Color.White, UtilityClass.Player.Position, (float)(SpellClass.R.Range / scale));
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
					var bigJungleMinions =
						Extensions.GetLargeJungleMinionsTargets().Concat(Extensions.GetLegendaryJungleMinionsTargets());
					foreach (var jungleMob in bigJungleMinions.Where(t => t.IsValidTarget(1250f)))
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