using Entropy;
using Entropy.SDK.Events;
using Entropy.SDK.Orbwalking;
using Gapcloser = AIO.Utilities.Gapcloser;

namespace AIO.Champions
{
	/// <summary>
	///     The methods class.
	/// </summary>
	internal partial class Caitlyn
	{
		#region Public Methods and Operators

		/// <summary>
		///     Initializes the methods.
		/// </summary>
		public void Methods()
		{
			Tick.OnTick += OnTick;
			Spellbook.OnLocalCastSpell += OnLocalCastSpell;
			Renderer.OnRender += OnRender;
			Renderer.OnEndScene += OnEndScene;
			Teleports.OnTeleport += OnTeleport;
			BuffManager.OnGainBuff += OnGainBuff;
			AIBaseClient.OnLevelUp += OnLevelUp;
			Orbwalker.OnPostAttack += OnPostAttack;
			AIBaseClient.OnProcessSpellCast += OnProcessSpellCast;
			Gapcloser.OnGapcloser += OnGapcloser;
		}

		#endregion
	}
}