// ReSharper disable ArrangeMethodOrOperatorBody

using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using SharpDX;
using Rectangle = Entropy.SDK.Geometry.Rectangle;

#pragma warning disable 1587

namespace AIO.Champions
{
	/// <summary>
	///     The definitions class.
	/// </summary>
	internal partial class Lucian
	{
		#region Fields

		/// <summary>
		///     Returns true if the player is using the ultimate.
		/// </summary>
		public bool IsCulling()
		{
			return UtilityClass.Player.HasBuff("LucianR");
		}

		/// <summary>
		///     The Q Rectangle.
		/// </summary>
		/// <param name="unit">The unit.</param>
		public Rectangle QRectangle(AIBaseClient unit)
		{
			var qPred = SpellClass.Q.GetPrediction(unit).CastPosition;
			var qRect = new Rectangle(Vector3.Zero, Vector3.Zero, SpellClass.Q2.Width)
			{
				StartPoint = LocalPlayer.Instance.Position,
				EndPoint = LocalPlayer.Instance.Position.Extend(qPred, SpellClass.Q2.Range)
			};

			return qRect;
		}

		#endregion
	}
}