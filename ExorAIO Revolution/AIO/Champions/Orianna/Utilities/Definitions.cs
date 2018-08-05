
using System;
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Caching;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using SharpDX;
using Rectangle = Entropy.SDK.Geometry.Rectangle;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Orianna
    {
        #region Public Methods and Operators

	    //public int LastECastTime = 0;

	    /// <summary>
	    ///     The E Rectangle.
	    /// </summary>
	    /// <param name="unit">The unit.</param>
	    public Rectangle ERectangle(AIBaseClient unit)
	    {
		    var eRect = new Rectangle(Vector3.Zero, Vector3.Zero, SpellClass.E.Width)
		    {
			    StartPoint = GetBall().Position,
			    EndPoint = LocalPlayer.Instance.Position
			};

		    return eRect;
	    }

		/// <summary>
		///     Gets the ball.
		/// </summary>
		public GameObject GetBall()
        {
			// Model is only called Orianna if the player has the ball on himself.
			if (UtilityClass.Player.ModelName.Equals("Orianna"))
			{
				return UtilityClass.Player;
			}

			// Check if any ally has the ball.
	        var possiblePosition = ObjectCache.AllyHeroes.FirstOrDefault(a =>
		        a.GetActiveBuffs().Any(b =>
					b.Caster.IsMe() &&
			        b.Name.Equals("orianaghost")));
	        if (possiblePosition != null)
	        {
		        return possiblePosition;
	        }

			var possiblePosition2 = ObjectCache.AllGameObjects.FirstOrDefault(o =>
                    o.IsValid &&
                    o.Name.Contains("Orianna_") && o.Name.Contains("_yomu_ring_"));
            if (possiblePosition2 != null)
            {
                return possiblePosition2;
            }

            return null;
        }

        #endregion
    }
}