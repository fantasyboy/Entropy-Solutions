using Entropy;
using AIO.Utilities;

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
            SpellBook.OnCastSpell += OnCastSpell;
            Render.OnPresent += OnPresent;
            AIBaseClient.OnProcessSpellCast += OnProcessSpellCast;
            ImplementationClass.IOrbwalker.PreAttack += OnPreAttack;
            ImplementationClass.IOrbwalker.PostAttack += OnPostAttack;
            Gapcloser.OnGapcloser += OnGapcloser;
            //AIBaseClient.OnIssueOrder += this.OnIssueOrder;
        }

        #endregion
    }
}