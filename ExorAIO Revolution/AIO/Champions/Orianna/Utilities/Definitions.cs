
using System;
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Caching;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Orianna
    {
        #region Public Methods and Operators

	    public int LastECastTime;

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

	        var possiblePosition = GameObjects.AllyHeroes.FirstOrDefault(a =>
		        !a.IsMe() &&
		        a.GetActiveBuffs().Any(b =>
			        b.Caster.IsMe() &&
			        b.Name.Equals("orianaghost")));
	        if (possiblePosition != null)
	        {
		        return possiblePosition;
	        }

			var possiblePosition2 = GameObjects.AllyMinions.FirstOrDefault(m =>
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