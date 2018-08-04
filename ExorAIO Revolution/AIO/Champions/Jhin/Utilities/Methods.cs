using Entropy;
using AIO.Utilities;
using Entropy.SDK.Orbwalking;

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
            Game.OnUpdate += OnUpdate;
            Spellbook.OnLocalCastSpell += OnLocalCastSpell;
            Renderer.OnRender += OnRender;
            AIBaseClient.OnProcessSpellCast += OnProcessSpellCast;
            Orbwalker.OnPreAttack += OnPreAttack;
            Orbwalker.OnPostAttack += OnPostAttack;
            Gapcloser.OnGapcloser += OnGapcloser;
            //AIBaseClient.OnIssueOrder += this.OnIssueOrder;
        }

        #endregion
    }
}