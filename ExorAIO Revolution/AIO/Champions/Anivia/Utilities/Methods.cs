using Entropy;
using AIO.Utilities;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Anivia
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            Render.OnPresent += OnPresent;
            GameObject.OnCreate += OnCreate;
            GameObject.OnDestroy += OnDestroy;
            ImplementationClass.IOrbwalker.OnNonKillableMinion += OnNonKillableMinion;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        #endregion
    }
}