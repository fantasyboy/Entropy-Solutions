using Entropy;
using Entropy.SDK.Events;
using AIO.Utilities;
using Entropy.SDK.Orbwalking;

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
            Orbwalker.OnPreAttack += OnPreAttack;
            Orbwalker.OnPostAttack += OnPostAttack;
            Render.OnPresent += OnPresent;
            Gapcloser.OnGapcloser += OnGapcloser;
            Dash.HeroDashed += OnDash;
        }

        #endregion
    }
}