using Entropy;
using AIO.Utilities;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Twitch
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            ImplementationClass.IOrbwalker.PostAttack += OnPostAttack;
            SpellBook.OnCastSpell += OnCastSpell;
            Render.OnPresent += OnPresent;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        #endregion
    }
}