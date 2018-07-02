
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
    internal partial class KogMaw
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic()
        {
            SpellClass.W.Range = 610 + 20 * SpellClass.W.Level;
            SpellClass.R.Range = 900f + 300f * SpellClass.R.Level;

            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The Automatic Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["logical"].As<MenuBool>().Enabled)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(t =>
                    !Invulnerable.Check(t) &&
                    t.IsImmobile(SpellClass.Q.Delay) &&
                    t.Distance(UtilityClass.Player) < SpellClass.Q.Range))
                {
                    SpellClass.Q.Cast(target.ServerPosition);
                }
            }

            /// <summary>
            ///     The Automatic E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["logical"].As<MenuBool>().Enabled)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(t =>
                    t.IsImmobile(SpellClass.E.Delay) &&
                    !Invulnerable.Check(t, DamageType.Magical, false) &&
                    t.Distance(UtilityClass.Player) < SpellClass.E.Range))
                {
                    SpellClass.E.Cast(target.ServerPosition);
                }
            }
        }

        #endregion
    }
}