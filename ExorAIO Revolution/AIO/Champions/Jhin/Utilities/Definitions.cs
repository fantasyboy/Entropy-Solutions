// ReSharper disable ArrangeMethodOrOperatorBody

using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Geometry;
using SharpDX;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Jhin
    {
        #region Fields

        /// <summary>
        ///     The args End.
        /// </summary>
        public Vector3 End = Vector3.Zero;

        /// <returns>
        ///     The Jhin's ultimate shot count.
        /// </returns>
        public int UltimateShotsCount;

        #endregion

        #region Public Methods and Operators

        /// <returns>
        ///     Returns true if the player has the AA Fourth Shot, else false.
        /// </returns>
        public bool HasFourthShot()
        {
            return UtilityClass.Player.HasBuff("jhinpassiveattackbuff");
        }

        /// <returns>
        ///     Returns true if the player has the Ultimate Fourth Shot, else false.
        /// </returns>
        public bool HasUltimateFourthShot()
        {
            return UltimateShotsCount == 3;
        }

        /// <returns>
        ///     Returns true if the player is reloading, else false.
        /// </returns>
        public bool IsReloading()
        {
            return UtilityClass.Player.HasBuff("JhinPassiveReload");
        }

        /// <returns>
        ///     Returns true if the player has the AA Fourth Shot, else false.
        /// </returns>
        public bool IsUltimateShooting()
        {
            return SpellClass.R.Name.Equals("JhinRShot");
        }

	    /// <summary>
	    ///     The Ultimate Cone.
	    /// </summary>
	    public Sector UltimateCone()
	    {
		    var range = SpellClass.R2.Range;
		    var direction = (End - UtilityClass.Player.Position).Normalized();
		    var spot = End + direction * range;
		    return new Sector(UtilityClass.Player.Position, spot, SpellClass.R.Width, range);
	    }

		#endregion
	}
}