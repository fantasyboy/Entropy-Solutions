
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
		    var ePred = SpellClass.E.GetPrediction(unit).CastPosition;
		    var eRect = new Rectangle(Vector3.Zero, Vector3.Zero, SpellClass.E.Width)
		    {
			    StartPoint = LocalPlayer.Instance.Position,
			    EndPoint = LocalPlayer.Instance.Position.Extend(ePred, SpellClass.E.Range)
		    };

		    return eRect;
	    }

		/// <summary>
		///     Gets the ball.
		/// </summary>
		public AIBaseClient GetBall()
        {
			// Return null if ball is traveling
	        if (ObjectCache.AllGameObjects.Any(o =>
		        o.Type.TypeID == GameObjectTypeID.DrawFX && o.IsValid && o.Name == "OrianaIzuna"))
	        {
		        return null;
	        }

	        var possiblePosition = ObjectCache.AllyHeroes.FirstOrDefault(a =>
		        !a.IsMe() &&
		        a.GetActiveBuffs().Any(b =>
			        b.Caster.IsMe() &&
			        b.Name.Equals("orianaghost")));
	        if (possiblePosition != null)
	        {
		        return possiblePosition;
	        }

			var possiblePosition2 = ObjectCache.AllyMinions.FirstOrDefault(m =>
                    Math.Abs(m.HP) > 0 &&
                    m.ModelName.Equals("OriannaBall"));
            if (possiblePosition2 != null)
            {
                return possiblePosition2;
            }

            if (UtilityClass.Player.HasBuff("orianaghostself"))
            {
                return UtilityClass.Player;
            }

            return null;
        }

        #endregion
    }
}