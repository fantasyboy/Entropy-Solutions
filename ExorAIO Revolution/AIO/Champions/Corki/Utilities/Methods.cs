using Entropy;
using AIO.Utilities;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Corki
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            Render.OnPresent += OnPresent;
            ImplementationClass.IOrbwalker.PostAttack += OnPostAttack;
            ImplementationClass.IOrbwalker.PreAttack += OnPreAttack;
            ImplementationClass.IOrbwalker.OnNonKillableMinion += OnNonKillableMinion;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        #endregion
    }
}