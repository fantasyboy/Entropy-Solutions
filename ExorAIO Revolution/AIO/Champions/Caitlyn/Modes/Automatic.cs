
using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using AIO.Utilities;

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
        public void Automatic()
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
                    SpellClass.W.Cast(UtilityClass.Player.ServerPosition.Extend(target.ServerPosition, UtilityClass.Player.Distance(target)+target.BoundingRadius/2));
                    UpdateEnemyTrapTime(target.NetworkId);
                }
            }

            /// <summary>
            ///     The Automatic W on Teleport Logic. 
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["teleport"].As<MenuBool>().Enabled)
            {
                foreach (var minion in ObjectManager.Get<Obj_AI_Minion>().Where(m =>
                    m.IsEnemy &&
                    m.Distance(UtilityClass.Player) <= SpellClass.W.Range &&
                    m.ValidActiveBuffs().Any(b => b.Name.Equals("teleport_target"))))
                {
                    SpellClass.W.Cast(minion.ServerPosition);
                }
            }

            /// <summary>
            ///     The Automatic Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["logical"].As<MenuBool>().Enabled)
            {
                switch (ImplementationClass.IOrbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                    case OrbwalkingMode.Mixed:
                        foreach (var target in GameObjects.EnemyHeroes.Where(t =>
                            !Invulnerable.Check(t) &&
                            t.IsImmobile(SpellClass.Q.Delay) &&
                            t.IsValidTarget(SpellClass.Q.Range) &&
                            t.HasBuff("caitlynyordletrapdebuff")))
                        {
                            SpellClass.Q.Cast(target.ServerPosition);
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
                        MenuClass.Spells["r"]["whitelist"][t.ChampionName.ToLower()].As<MenuBool>().Enabled)
                    .MinBy(o => o.GetRealHealth());
                if (bestTarget != null)
                {
                    UtilityClass.CastOnUnit(SpellClass.R, bestTarget);
                }
            }
        }

        #endregion
    }
}