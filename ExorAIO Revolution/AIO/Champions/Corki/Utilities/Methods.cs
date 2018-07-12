using Entropy;
using AIO.Utilities;
using Entropy.SDK.Orbwalking;

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
            Orbwalker.OnPostAttack += OnPostAttack;
            Orbwalker.OnPreAttack += OnPreAttack;
            Orbwalker.OnNonKillableMinion += OnNonKillableMinion;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        #endregion
    }
}