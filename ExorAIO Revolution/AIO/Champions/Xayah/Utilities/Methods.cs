using Aimtec;
using AIO.Utilities;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Xayah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            Render.OnPresent += OnPresent;
            ImplementationClass.IOrbwalker.PreAttack += OnPreAttack;
            GameObject.OnCreate += OnCreate;
            GameObject.OnDestroy += OnDestroy;
            Gapcloser.OnGapcloser += OnGapcloser;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
        }

        #endregion
    }
}