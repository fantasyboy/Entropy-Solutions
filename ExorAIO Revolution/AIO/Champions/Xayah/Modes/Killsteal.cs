
using System.Linq;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Xayah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal()
        {
        }

        /// <summary>
        ///     Fired as fast as possible.
        /// </summary>
        public void BladeCallerKillsteal()
        {
            /// <summary>
            ///     The KillSteal E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["killsteal"].As<MenuBool>().Enabled)
            {
                if (GameObjects.EnemyHeroes.Any(h =>
                    IsPerfectFeatherTarget(h) &&
                    h.GetRealHealth() < GetPerfectFeatherDamage(h, CountFeathersHitOnUnit(h))))
                {
                    SpellClass.E.Cast();
                }
            }
        }

        #endregion
    }
}