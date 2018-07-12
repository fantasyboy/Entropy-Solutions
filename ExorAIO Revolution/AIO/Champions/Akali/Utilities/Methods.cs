using Entropy;
using AIO.Utilities;
using Entropy.SDK.Orbwalking;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Akali
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            Orbwalker.OnPostAttack += OnPostAttack;
            Orbwalker.OnPreAttack += OnPreAttack;
            Render.OnPresent += OnPresent;
            Spellbook.OnLocalCastSpell += OnLocalCastSpell;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        #endregion
    }
}