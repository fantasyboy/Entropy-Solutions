using Entropy;
using AIO.Utilities;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Taliyah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            GameObject.OnCreate += OnCreate;
            GameObject.OnDestroy += OnDestroy;
            Spellbook.OnLocalCastSpell += OnLocalCastSpell;
            AIBaseClient.OnProcessSpellCast += OnProcessSpellCast;
            Render.OnPresent += OnPresent;
            Gapcloser.OnGapcloser += OnGapcloser;

            //Events.OnInterruptableTarget += Taliyah.OnInterruptableTarget;
        }

        #endregion
    }
}