// ReSharper disable ArrangeMethodOrOperatorBody


using Aimtec;
using Aimtec.SDK.Extensions;
using AIO.Utilities;

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
        public Vector2Geometry.Sector UltimateCone()
        {
            var targetPos = End;
            var range = SpellClass.R.Range-UtilityClass.Player.BoundingRadius;
            var dir = (targetPos - UtilityClass.Player.ServerPosition).Normalized();
            var spot = targetPos + dir * range;

            return new Vector2Geometry.Sector((Vector2)End.Extend(UtilityClass.Player.ServerPosition, End.Distance(UtilityClass.Player)+UtilityClass.Player.BoundingRadius*3), (Vector2)spot, SpellClass.R2.Width, range);
        }

        /// <summary>
        ///     The Ultimate Cone.
        /// </summary>
        public Vector3Geometry.Sector DrawUltimateCone()
        {
            var targetPos = End;
            var range = SpellClass.R.Range - UtilityClass.Player.BoundingRadius;
            var dir = (targetPos - UtilityClass.Player.Position).Normalized();
            var spot = targetPos + dir * range;

            return new Vector3Geometry.Sector(End.Extend(UtilityClass.Player.Position, End.Distance(UtilityClass.Player) + UtilityClass.Player.BoundingRadius * 3), spot, SpellClass.R2.Width, range);
        }

        #endregion
    }
}