
using System;
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.UI.Components;
using Entropy.ToolKit;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Jinx
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(EntropyEventArgs args)
        {
            SpellClass.Q2.Range = SpellClass.Q.Range + 50f + 25f * SpellClass.Q.Level;

            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            Logging.Log($"Q Range: {SpellClass.Q.Range}, Distance: {Orbwalker.GetOrbwalkingTarget()?.DistanceToPlayer()}");

            /// <summary>
            ///     The Force Pow Pow Logic. 
            /// </summary>
            if (SpellClass.Q.Ready &&
                IsUsingFishBones() &&
                Orbwalker.Mode == OrbwalkingMode.None &&
                MenuClass.Miscellaneous["forcepowpow"].As<MenuBool>().Enabled)
            {
                SpellClass.Q.Cast();
            }

            /// <summary>
            ///     The Automatic E Logic. 
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["logical"].As<MenuBool>().Enabled)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(t =>
                    t.IsImmobile(SpellClass.E.Delay) &&
                    t.DistanceToPlayer() <= SpellClass.E.Range))
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

            /// <summary>
            ///     The Semi-Automatic R Management.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["bool"].As<MenuBool>().Enabled &&
                MenuClass.Spells["r"]["key"].As<MenuKeyBind>().Enabled)
            {
                var bestTarget = GameObjects.EnemyHeroes
                    .Where(t =>
                        !Invulnerable.Check(t) &&
                        t.IsValidTarget(SpellClass.R.Range) &&
                        MenuClass.Spells["r"]["whitelist"][t.CharName.ToLower()].As<MenuBool>().Enabled)
                    .MinBy(o => o.GetRealHealth());
                if (bestTarget != null)
                {
                    SpellClass.R.Cast(bestTarget);
                }
            }
        }

        #endregion
    }
}