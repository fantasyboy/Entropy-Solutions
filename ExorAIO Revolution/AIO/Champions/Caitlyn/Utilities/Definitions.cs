
using System.Collections.Generic;
using System.Linq;
using Entropy;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Caitlyn
    {
        #region Fields

        /// <summary>
        ///     Initializes the enemy's trap data.
        /// </summary>
        public Dictionary<int, double> EnemyTrapData = new Dictionary<int, double>();

        /// <summary>
        ///     Initializes the trap time check for each enemy.
        /// </summary>
        public void InitializeTrapTimeCheck()
        {
            foreach (var hero in GameObjects.EnemyHeroes)
            {
                EnemyTrapData.Add(hero.NetworkID, 0d);
            }
        }

        /// <summary>
        ///     Gets an enemy's last Trap time.
        /// </summary>
        /// <param name="networkId">The networkId.</param>
        public double GetLastEnemyTrapTime(int networkId)
        {
            return EnemyTrapData.FirstOrDefault(k => k.Key == networkId).Value;
        }

        /// <summary>
        ///     Returns true if an enemy can be trapped, else, false.
        /// </summary>
        /// <param name="hero">The hero.</param>
        public bool CanTrap(AIHeroClient hero)
        {
            return Game.TickCount - GetLastEnemyTrapTime(hero.NetworkID) >= 4000 - SpellClass.W.Delay * 1000;
        }

        /// <summary>
        ///     Updates an enemy's last Trap time.
        /// </summary>
        /// <param name="networkId">The networkId.</param>
        public void UpdateEnemyTrapTime(int networkId)
        {
            EnemyTrapData[networkId] = Game.TickCount;
        }

        #endregion
    }
}