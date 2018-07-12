
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
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
    internal partial class Caitlyn
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(EntropyEventArgs args)
        {
            SpellClass.R.Range = 1500f + 500f * SpellClass.R.Level;

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
                    CanTrap(t) &&
                    t.IsImmobile(SpellClass.W.Delay) &&
                    t.Distance(UtilityClass.Player) < SpellClass.W.Range))
                {
                    SpellClass.W.Cast(UtilityClass.Player.Position.Extend(target.Position, UtilityClass.Player.Distance(target)+target.BoundingRadius/2));
                    UpdateEnemyTrapTime(target.NetworkID);
                }
            }

            /// <summary>
            ///     The Automatic W on Teleport Logic. 
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["teleport"].As<MenuBool>().Enabled)
            {
                foreach (var minion in ObjectManager.Get<AIMinionClient>().Where(m =>
                    m.IsEnemy() &&
                    m.Distance(UtilityClass.Player) <= SpellClass.W.Range &&
                    m.GetActiveBuffs().Any(b => b.Name.Equals("teleport_target"))))
                {
                    SpellClass.W.Cast(minion.Position);
                }
            }

            /// <summary>
            ///     The Automatic Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["logical"].As<MenuBool>().Enabled)
            {
                switch (Orbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                    case OrbwalkingMode.Harass:
                        foreach (var target in GameObjects.EnemyHeroes.Where(t =>
                            !Invulnerable.Check(t) &&
                            t.IsImmobile(SpellClass.Q.Delay) &&
                            t.IsValidTarget(SpellClass.Q.Range) &&
                            t.HasBuff("caitlynyordletrapdebuff")))
                        {
                            SpellClass.Q.Cast(target.Position);
                        }
                        break;
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
                    SpellClass.R.CastOnUnit(bestTarget);
                }
            }
        }

        #endregion
    }
}