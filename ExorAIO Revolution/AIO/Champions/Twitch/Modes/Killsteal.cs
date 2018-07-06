
using System.Linq;
using AIO.Utilities;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Twitch
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal(args)
        {
        }

        /// <summary>
        ///     Fired as fast as possible.
        /// </summary>
        public void ExpungeKillsteal(args)
        {
            /// <summary>
            ///     The KillSteal E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["killsteal"].As<MenuBool>().Enabled)
            {
                if (GameObjects.EnemyHeroes.Any(t =>
                    IsPerfectExpungeTarget(t) &&
                    t.GetRealHealth() < GetTotalExpungeDamage(t)))
                {
                    SpellClass.E.Cast();
                }
            }
        }

        #endregion
    }
}