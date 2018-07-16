using Entropy;
using Entropy.SDK.Events;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Predictions.RecallPrediction;
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
            Game.OnUpdate += OnUpdate;
            Spellbook.OnLocalCastSpell += OnLocalCastSpell;
            Renderer.OnRender += OnRender;
	        Renderer.OnEndScene += OnEndScene;
	        Teleports.OnTeleport += OnTeleport;
	        AIBaseClient.OnLevelUp += OnLevelUp;
            Orbwalker.OnPostAttack += OnPostAttack;
            AIBaseClient.OnProcessSpellCast += OnProcessSpellCast;
            Gapcloser.OnGapcloser += OnGapcloser;

	        RecallPrediction.Initialize();
		}

        #endregion
    }
}