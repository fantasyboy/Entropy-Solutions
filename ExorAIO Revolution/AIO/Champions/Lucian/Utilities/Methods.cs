using Entropy;
using AIO.Utilities;
using Entropy.SDK.Orbwalking;

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
            Game.OnUpdate += OnUpdate;
            Orbwalker.OnPostAttack += OnPostAttack;
            Renderer.OnRender += OnRender;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        #endregion
    }
}