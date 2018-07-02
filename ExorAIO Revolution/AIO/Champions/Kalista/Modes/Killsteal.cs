
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Kalista
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal()
        {
            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range).Where(t =>
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.Q) >= t.GetRealHealth()))
                {
                    var collisions = SpellClass.Q.GetPrediction(target).CollisionObjects
                        .Where(c => Extensions.GetAllGenericMinionsTargetsInRange(SpellClass.Q.Range).Contains(c))
                        .ToList();
                    if (collisions.Any())
                    {
                        if (collisions.All(c =>
                            c.GetRealHealth() <= UtilityClass.Player.GetSpellDamage(c, SpellSlot.Q)))
                        {
                            SpellClass.Q.Cast(SpellClass.Q.GetPrediction(target).CastPosition);
                            break;
                        }
                    }
                    else
                    {
                        SpellClass.Q.Cast(SpellClass.Q.GetPrediction(target).CastPosition);
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     Fired as fast as possible.
        /// </summary>
        public void RendKillsteal()
        {
            /// <summary>
            ///     The KillSteal E Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["killsteal"].As<MenuBool>().Enabled)
            {
                if (GameObjects.EnemyHeroes.Any(t =>
                        IsPerfectRendTarget(t) &&
                        t.GetRealHealth() < GetTotalRendDamage(t)))
                {
                    SpellClass.E.Cast();
                }
            }
        }

        #endregion
    }
}