using Entropy;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Events;
using Gapcloser = AIO.Utilities.Gapcloser;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Xayah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the methods.
        /// </summary>
        public void Methods()
        {
			Tick.OnTick += OnTick;
			Renderer.OnRender += OnRender;
            Orbwalker.OnPreAttack += OnPreAttack;
            GameObject.OnCreate += OnCreate;
            GameObject.OnDelete += OnDelete;
			Gapcloser.OnGapcloser += OnGapcloser;
            AIBaseClient.OnProcessSpellCast += OnProcessSpellCast;
        }

        #endregion
    }
}