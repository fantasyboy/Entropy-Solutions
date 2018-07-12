using Entropy;
using AIO.Utilities;
using Entropy.SDK.Orbwalking;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class MissFortune
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            Orbwalker.OnPreAttack += OnPreAttack;
            Orbwalker.OnPostAttack += OnPostAttack;
            Spellbook.OnLocalCastSpell += OnLocalCastSpell;
            Render.OnPresent += OnPresent;
            Gapcloser.OnGapcloser += OnGapcloser;
            BuffManager.OnRemoveBuff += OnRemoveBuff;
        }

        #endregion
    }
}