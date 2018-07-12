using Entropy;
using AIO.Utilities;
using Entropy.SDK.Orbwalking;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Xayah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            Render.OnPresent += OnPresent;
            Orbwalker.OnPreAttack += OnPreAttack;
            GameObject.OnCreate += OnCreate;
            GameObject.OnDestroy += OnDestroy;
            Gapcloser.OnGapcloser += OnGapcloser;
            AIBaseClient.OnProcessSpellCast += OnProcessSpellCast;
        }

        #endregion
    }
}