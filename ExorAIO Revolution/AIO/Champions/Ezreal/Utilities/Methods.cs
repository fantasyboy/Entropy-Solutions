using Entropy;
using AIO.Utilities;
using Entropy.SDK.Orbwalking;

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
            Game.OnUpdate += OnUpdate;
            Orbwalker.OnPostAttack += OnPostAttack;
            Orbwalker.OnNonKillableMinion += OnNonKillableMinion;
            Render.OnPresent += OnPresent;
            AIBaseClient.OnProcessAutoAttack += OnProcessAutoAttack;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        #endregion
    }
}