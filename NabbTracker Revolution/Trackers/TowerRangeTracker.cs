using System.Linq;
using Entropy.SDK.Caching;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Rendering;
using Entropy.SDK.UI.Components;
using NabbTracker.Utilities;

using Color = SharpDX.Color;

namespace NabbTracker.Trackers
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal class TowerRangeTracker
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the TowerRange Tracker.
        /// </summary>
        public static void Initialize()
        {
            foreach (var tower in ObjectCache.AllTurrets.Where(t =>
                !t.IsDead &&
                t.IsVisible))
            {
                if (tower.IsEnemy() &&
                    !MenuClass.TowerRangeTracker["enemies"].As<MenuBool>().Enabled)
                {
                    continue;
                }

                if (tower.IsAlly() &&
                    !MenuClass.TowerRangeTracker["allies"].As<MenuBool>().Enabled)
                {
                    continue;
                }

                var towerAutoAttackRange = 775f + tower.BoundingRadius + UtilityClass.Player.BoundingRadius - 10f;
	            CircleRendering.Render(tower.IsEnemy() && UtilityClass.Player.Distance(tower) <= towerAutoAttackRange
		            ? Colors.GetRealColor(Color.Red)
		            : Colors.GetRealColor(Color.LightGreen), towerAutoAttackRange, tower);
            }
        }

        #endregion
    }
}