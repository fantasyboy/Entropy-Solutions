using Aimtec;
using AIO.Utilities;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Orianna
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            SpellBook.OnCastSpell += OnCastSpell;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Render.OnPresent += OnPresent;
            Gapcloser.OnGapcloser += OnGapcloser;

            //Events.OnInterruptableTarget += OnInterruptableTarget;
        }

        #endregion
    }
}