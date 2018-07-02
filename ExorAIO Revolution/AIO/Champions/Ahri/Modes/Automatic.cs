
using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Ahri
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic()
        {
            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The Automatic E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["logical"].As<MenuBool>().Enabled)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(t =>
                    t.IsImmobile(SpellClass.E.Delay) &&
                    t.IsValidTarget(SpellClass.E.Range) &&
                    !Invulnerable.Check(t, DamageType.Magical, false)))
                {
                    SpellClass.E.Cast(target);
                }
            }
        }

        #endregion
    }
}