using Entropy;
using Entropy.SDK.Events;
using Gapcloser = AIO.Utilities.Gapcloser;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Orianna
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the methods.
        /// </summary>
        public void Methods()
        {
	        Tick.OnTick += OnTick;
            Spellbook.OnLocalCastSpell += OnLocalCastSpell;
            AIBaseClient.OnProcessSpellCast += OnProcessSpellCast;
            Renderer.OnRender += OnRender;
            Gapcloser.OnGapcloser += OnGapcloser;
	        Interrupter.OnInterruptableSpell += OnInterruptableSpell;
		}

        #endregion
    }
}