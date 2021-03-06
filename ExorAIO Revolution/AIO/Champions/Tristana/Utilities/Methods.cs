using Entropy;
using AIO.Utilities;
using Entropy.SDK.Orbwalking;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Tristana
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            Orbwalker.OnPreAttack += OnPreAttack;
            Render.OnPresent += OnPresent;
            Gapcloser.OnGapcloser += OnGapcloser;

            //Events.OnInterruptableTarget += OnInterruptableTarget;
        }

        #endregion
    }
}