using System.Linq;
using Entropy.SDK.Caching;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Rendering;
using NabbTracker.Utilities;
using Color = SharpDX.Color;

namespace NabbTracker.Trackers
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal class AttackRangeTracker
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the AttackRange Tracker.
        /// </summary>
        public static void Initialize()
        {
            foreach (var hero in ObjectCache.AllHeroes.Where(h =>
                !h.IsMe() &&
                !h.IsDead &&
                h.IsVisible))
            {
                if (hero.IsEnemy() &&
                    !MenuClass.AttackRangeTracker["enemies"].Enabled)
                {
                    continue;
                }

                if (hero.IsAlly() &&
                    !MenuClass.AttackRangeTracker["allies"].Enabled)
                {
                    continue;
                }

                var attackRange = hero.GetAutoAttackRange()+hero.BoundingRadius;
                CircleRendering.Render(UtilityClass.Player.Distance(hero) < attackRange
		            ? Colors.GetRealColor(Color.Red)
		            : Colors.GetRealColor(Color.Yellow), attackRange, hero);
            }
        }

        #endregion
    }
}