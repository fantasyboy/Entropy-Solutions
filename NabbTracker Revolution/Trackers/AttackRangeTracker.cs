using System.Drawing;
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;

namespace NabbTracker
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
            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(h =>
                !h.IsMe &&
                !h.IsDead &&
                h.IsVisible))
            {
                if (hero.IsEnemy &&
                    !MenuClass.AttackRangeTracker["enemies"].As<MenuBool>().Enabled)
                {
                    continue;
                }

                if (hero.IsAlly &&
                    !MenuClass.AttackRangeTracker["allies"].As<MenuBool>().Enabled)
                {
                    continue;
                }

                var attackRange = hero.AttackRange+hero.BoundingRadius;
                Render.Circle(hero.Position, attackRange, 30, UtilityClass.Player.Distance(hero) < attackRange
                    ? Colors.GetRealColor(Color.Red)
                    : Colors.GetRealColor(Color.Yellow));
            }
        }

        #endregion
    }
}