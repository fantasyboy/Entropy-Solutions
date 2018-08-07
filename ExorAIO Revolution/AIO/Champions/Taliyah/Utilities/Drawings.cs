using AIO.Utilities;
using Entropy;
using Entropy.SDK.Rendering;
using Color = SharpDX.Color;

#pragma warning disable 1587

namespace AIO.Champions
{
	/// <summary>
	///     The drawings class.
	/// </summary>
	internal partial class Taliyah
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
			///     Loads the WorkedGrounds drawing.
			/// </summary>
			if (MenuClass.Drawings["grounds"].Enabled)
			{
				foreach (var ground in WorkedGrounds)
				{
					CircleRendering.Render(Color.Brown, WorkedGroundWidth, ground.Key);
				}
			}

			/// <summary>
			///     Loads the MineFields drawing.
			/// </summary>
			if (MenuClass.Drawings["boulders"].Enabled)
			{
				foreach (var boulder in MineField)
				{
					CircleRendering.Render(Color.Brown, BouldersWidth, boulder.Key);
				}
			}
		}

		public void OnEndScene(EntropyEventArgs args)
		{
			/// <summary>
			///     Loads the R minimap drawing.
			/// </summary>
			if (SpellClass.R.Ready &&
			    MenuClass.Drawings["rmm"].Enabled)
			{
				var scale = (TacticalMap.ScaleX + TacticalMap.ScaleY) / 4.5;
				TacticalMapRendering.Render(Color.White, UtilityClass.Player.Position, (float)(SpellClass.R.Range / scale));
			}
		}

		#endregion
	}
}