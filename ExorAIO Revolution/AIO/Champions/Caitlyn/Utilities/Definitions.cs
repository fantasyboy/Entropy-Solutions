
using Entropy;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Caitlyn
    {
		#region Fields

		/// <summary>
		///     Returns true if an enemy can be trapped, else, false.
		/// </summary>
		/// <param name="hero">The hero.</param>
		public bool CanTrap(AIHeroClient hero)
        {
	        return !hero.HasBuff("caitlynyordletrapsight");
        }

		#endregion
	}
}