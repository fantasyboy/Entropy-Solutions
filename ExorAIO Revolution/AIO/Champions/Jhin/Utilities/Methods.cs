using Entropy;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Events;
using Gapcloser = AIO.Utilities.Gapcloser;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Jhin
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the methods.
        /// </summary>
        public void Methods()
        {
            Tick.OnTick += OnTick;
            Spellbook.OnLocalCastSpell += OnLocalCastSpell;
            Renderer.OnRender += OnRender;
            AIBaseClient.OnProcessSpellCast += OnProcessSpellCast;
            Orbwalker.OnPreAttack += OnPreAttack;
            Orbwalker.OnPostAttack += OnPostAttack;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        #endregion
    }
}