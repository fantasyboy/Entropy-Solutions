﻿using Entropy;
using Entropy.SDK.Extensions.Geometry;
using SharpDX;

namespace AIO.Champions
{
	/// <summary>
	///     The definitions class.
	/// </summary>
	internal partial class Vayne
	{
		/// <summary>
		///     Returns the estimated position the target will be after the condemn's delay finishes.
		/// </summary>
		/// <param name="unit">The unit.</param>
		/// <param name="delay">The delay.</param>
		public Vector3 EstimatedPosition(AIBaseClient unit, float delay)
		{
			var paths = unit.Path;
			var unitPosition = unit.Position;
			if (paths.Length == 0)
			{
				return unitPosition;
			}

			for (var i = 0; i < paths.Length - 1; i++)
			{
				var previousPath = paths[i];
				var currentPath = paths[i + 1];
				var direction = (currentPath - previousPath).Normalized();
				var velocity = direction * unit.CharIntermediate.MoveSpeed;

				delay = delay + EnetClient.Ping / 1000f;
				unitPosition = unit.Position + velocity * delay;
			}

			return unitPosition;
		}

		/// <summary>
		///     Returns wether or not a position is a perfect wall position.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <param name="target">The target.</param>
		/// <param name="startPos">The starting position of the check.</param>
		/// <param name="amount">The amount.</param>
		public bool IsPerfectWallPosition(Vector3 point, AIHeroClient target, Vector3 startPos, float amount)
		{
			return point.Extend(startPos, -amount).IsWall() &&
			       point.Extend(startPos, -(amount + target.BoundingRadius)).IsWall();
		}
	}
}