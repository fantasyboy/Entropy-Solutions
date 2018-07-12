using System.Drawing;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Menu.Components;

namespace NabbTracker
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
            foreach (var tower in ObjectManager.Get<AITurretClient>().Where(t =>
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
                Render.Circle(tower.Position, towerAutoAttackRange, 30, tower.IsEnemy() && UtilityClass.Player.Distance(tower) <= towerAutoAttackRange
                    ? Colors.GetRealColor(Color.Red)
                    : Colors.GetRealColor(Color.LightGreen));
            }
        }

        #endregion
    }
}