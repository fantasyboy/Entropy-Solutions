using Aimtec;
using AIO.Utilities;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Jinx
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            ImplementationClass.IOrbwalker.PreAttack += OnPreAttack;
            Render.OnPresent += OnPresent;
            Gapcloser.OnGapcloser += OnGapcloser;

            //Events.OnInterruptableTarget += OnInterruptableTarget;
        }

        #endregion
    }
}