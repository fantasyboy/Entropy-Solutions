using Aimtec;
using AIO.Utilities;

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
            ImplementationClass.IOrbwalker.PostAttack += OnPostAttack;
            Render.OnPresent += OnPresent;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        #endregion
    }
}