using Aimtec;
using AIO.Utilities;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Ezreal
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            ImplementationClass.IOrbwalker.PostAttack += OnPostAttack;
            ImplementationClass.IOrbwalker.OnNonKillableMinion += OnNonKillableMinion;
            Render.OnPresent += OnPresent;
            Obj_AI_Base.OnProcessAutoAttack += OnProcessAutoAttack;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        #endregion
    }
}