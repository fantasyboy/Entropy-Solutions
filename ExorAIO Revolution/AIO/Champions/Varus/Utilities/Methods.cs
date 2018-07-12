using Entropy;
using AIO.Utilities;
using Entropy.SDK.Orbwalking;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Varus
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            Orbwalker.OnPreAttack += OnPreAttack;
            Orbwalker.OnPostAttack += OnPostAttack;
            Render.OnPresent += OnPresent;
            Spellbook.OnLocalCastSpell += OnLocalCastSpell;
            Gapcloser.OnGapcloser += OnGapcloser;

            //Events.OnInterruptableTarget += Ashe.OnInterruptableTarget;
        }

        #endregion
    }
}