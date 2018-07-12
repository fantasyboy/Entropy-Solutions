
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Jhin
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(EntropyEventArgs args)
        {
            Orbwalker.MovingEnabled = !IsUltimateShooting();
            Orbwalker.AttackingEnabled = !IsUltimateShooting();

            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The Automatic W Logic. 
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["logical"].As<MenuBool>().Enabled)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(t =>
                    t.HasBuff("jhinespotteddebuff") &&
                    t.IsImmobile(SpellClass.W.Delay) &&
                    t.IsValidTarget(SpellClass.W.Range) &&
                    !Invulnerable.Check(t, DamageType.Magical, false)))
                {
                    SpellClass.W.Cast(target.Position);
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
                    t.DistanceToPlayer() < SpellClass.E.Range))
                {
                    SpellClass.E.Cast(target.Position);
                }
            }

            /// <summary>
            ///     The Automatic E on Teleport Logic. 
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["teleport"].As<MenuBool>().Enabled)
            {
                foreach (var minion in ObjectManager.Get<AIMinionClient>().Where(m =>
                    m.IsEnemy() &&
                    m.DistanceToPlayer() <= SpellClass.E.Range &&
                    m.GetActiveBuffs().Any(b => b.Name.Equals("teleport_target"))))
                {
                    SpellClass.E.Cast(minion.Position);
                }
            }
        }

        #endregion
    }
}