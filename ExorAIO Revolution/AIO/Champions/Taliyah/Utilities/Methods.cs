using Entropy;
using Entropy.SDK.Events;
using Gapcloser = AIO.Utilities.Gapcloser;

namespace AIO.Champions
{
	/// <summary>
	///     The methods class.
	/// </summary>
	internal partial class Taliyah
	{
		#region Public Methods and Operators

		/// <summary>
		///     Initializes the methods.
		/// </summary>
		public void Methods()
		{
			Tick.OnTick += OnTick;
			GameObject.OnCreate += OnCreate;
			GameObject.OnDelete += OnDelete;
			Spellbook.OnLocalCastSpell += OnLocalCastSpell;
			AIBaseClient.OnFinishCast += OnFinishCast;
			Renderer.OnRender += OnRender;
			AIBaseClient.OnLevelUp += OnLevelUp;
			Teleports.OnTeleport += OnTeleport;
			Renderer.OnEndScene += OnEndScene;
			Gapcloser.OnGapcloser += OnGapcloser;
			Interrupter.OnInterruptableSpell += OnInterruptableSpell;
		}

		#endregion
	}
}