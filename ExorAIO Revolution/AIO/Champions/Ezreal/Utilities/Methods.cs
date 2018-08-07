using Entropy;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Events;
using Gapcloser = AIO.Utilities.Gapcloser;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Ezreal
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the methods.
        /// </summary>
        public void Methods()
        {
            Tick.OnTick += OnTick;
            Orbwalker.OnPostAttack += OnPostAttack;
            Orbwalker.OnNonKillableMinion += OnNonKillableMinion;
            Renderer.OnRender += OnRender;
            AIBaseClient.OnProcessBasicAttack += OnProcessBasicAttack;
			Gapcloser.OnGapcloser += OnGapcloser;
        }

        #endregion
    }
}