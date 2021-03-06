using Entropy;
using Entropy.SDK.Events;
using Entropy.SDK.Orbwalking;
using Gapcloser = AIO.Utilities.Gapcloser;

namespace AIO.Champions
{
	/// <summary>
	///     The methods class.
	/// </summary>
	internal partial class Vayne
	{
		#region Public Methods and Operators

		/// <summary>
		///     Sets the methods.
		/// </summary>
		public void Methods()
		{
			Tick.OnTick += OnTick;
			Orbwalker.OnPreAttack += OnPreAttack;
			Orbwalker.OnPostAttack += OnPostAttack;
			Renderer.OnRender += OnRender;
			Renderer.OnEndScene += OnEndScene;
			Gapcloser.OnGapcloser += OnGapcloser;
			Dash.OnNewDash += OnDash;
			Interrupter.OnInterruptableSpell += OnInterruptableSpell;
		}

		#endregion
	}
}