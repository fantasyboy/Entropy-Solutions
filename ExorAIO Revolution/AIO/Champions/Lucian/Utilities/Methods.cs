using Entropy;
using Entropy.SDK.Events;
using Entropy.SDK.Orbwalking;
using Gapcloser = AIO.Utilities.Gapcloser;

namespace AIO.Champions
{
	/// <summary>
	///     The methods class.
	/// </summary>
	internal partial class Lucian
	{
		#region Public Methods and Operators

		/// <summary>
		///     The methods.
		/// </summary>
		public void Methods()
		{
			Tick.OnTick += OnTick;
			Orbwalker.OnPostAttack += OnPostAttack;
			Renderer.OnRender += OnRender;
			Gapcloser.OnGapcloser += OnGapcloser;
		}

		#endregion
	}
}