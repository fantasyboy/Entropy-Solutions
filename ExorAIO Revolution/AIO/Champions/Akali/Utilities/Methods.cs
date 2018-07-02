using Entropy;
using AIO.Utilities;

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
            ImplementationClass.IOrbwalker.PostAttack += OnPostAttack;
            ImplementationClass.IOrbwalker.PreAttack += OnPreAttack;
            Render.OnPresent += OnPresent;
            SpellBook.OnCastSpell += OnCastSpell;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        #endregion
    }
}