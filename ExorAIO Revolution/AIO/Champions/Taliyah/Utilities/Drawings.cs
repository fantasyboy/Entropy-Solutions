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
				TacticalMapRendering.Render(Color.White, UtilityClass.Player.Position, SpellClass.R.Range);
			}
		}

		#endregion
	}
}