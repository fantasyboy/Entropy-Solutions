using Entropy;
using AIO.Utilities;
using Entropy.SDK.Orbwalking;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Ashe
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            Orbwalker.OnPostAttack += OnPostAttack;
            Render.OnPresent += OnPresent;
            AttackableUnit.OnLeaveVisible += OnLeaveVisibility;
            Gapcloser.OnGapcloser += OnGapcloser;

            //Events.OnInterruptableTarget += Ashe.OnInterruptableTarget;
        }

        #endregion
    }
}