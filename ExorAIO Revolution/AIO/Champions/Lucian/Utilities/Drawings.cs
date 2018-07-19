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
	internal partial class Lucian
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
			///     Loads the Extended Q drawing.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    MenuClass.Drawings["qextended"].Enabled)
			{
				CircleRendering.Render(Color.Green, SpellClass.Q2.Range, UtilityClass.Player);
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

		#endregion
	}
}