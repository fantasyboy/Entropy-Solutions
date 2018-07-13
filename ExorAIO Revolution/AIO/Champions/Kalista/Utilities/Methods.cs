using Entropy;
using Entropy.SDK.Orbwalking;

namespace AIO.Champions
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Kalista
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += OnUpdate;
            Orbwalker.OnPreAttack += OnPreAttack;
	        Renderer.OnRender   += OnRender;
	        Renderer.OnEndScene += OnEndScene;
			AIBaseClient.OnProcessSpellCast += OnProcessSpellCast;
		}

		#endregion
	}
}