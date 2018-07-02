using Aimtec;
using Aimtec.SDK.Events;
using AIO.Utilities;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Vayne
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            ImplementationClass.IOrbwalker.PreAttack += OnPreAttack;
            ImplementationClass.IOrbwalker.PostAttack += OnPostAttack;
            Render.OnPresent += OnPresent;
            Gapcloser.OnGapcloser += OnGapcloser;
            Dash.HeroDashed += OnDash;
        }

        #endregion
    }
}