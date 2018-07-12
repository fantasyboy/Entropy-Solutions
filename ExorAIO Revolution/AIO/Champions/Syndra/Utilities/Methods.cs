
using Entropy;
using Entropy.SDK.Events;
using AIO.Utilities;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Syndra
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            GameObject.OnCreate += OnCreate;
            GameObject.OnDestroy += OnDestroy;
            Render.OnPresent += OnPresent;
            Spellbook.OnLocalCastSpell += OnLocalCastSpell;
            Gapcloser.OnGapcloser += OnGapcloser;
            Dash.HeroDashed += OnDash;

            //Events.OnInterrupt += this.OnInterrupt;
        }

        #endregion
    }
}