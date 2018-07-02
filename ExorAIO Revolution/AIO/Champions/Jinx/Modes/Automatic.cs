
using System;
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using Entropy.SDK.Orbwalking;
using AIO.Utilities;

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
        public void Automatic()
        {
            SpellClass.Q2.Range = SpellClass.Q.Range + 50f + 25f * SpellClass.Q.Level;

            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            Console.WriteLine($"Q Range: {SpellClass.Q.Range}, Distance: {ImplementationClass.IOrbwalker.GetOrbwalkingTarget()?.Distance(UtilityClass.Player)}");

            /// <summary>
            ///     The Force Pow Pow Logic. 
            /// </summary>
            if (SpellClass.Q.Ready &&
                IsUsingFishBones() &&
                ImplementationClass.IOrbwalker.Mode == OrbwalkingMode.None &&
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
                    t.Distance(UtilityClass.Player) <= SpellClass.E.Range))
                {
                    SpellClass.E.Cast(target.ServerPosition);
                }
            }

            /// <summary>
            ///     The Automatic E on Teleport Logic. 
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["teleport"].As<MenuBool>().Enabled)
            {
                foreach (var minion in ObjectManager.Get<Obj_AI_Minion>().Where(m =>
                        m.IsEnemy &&
                        m.Distance(UtilityClass.Player) <= SpellClass.E.Range &&
                        m.ValidActiveBuffs().Any(b => b.Name.Equals("teleport_target"))))
                {
                    SpellClass.E.Cast(minion.ServerPosition);
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
                    SpellClass.R.Cast(bestTarget);
                }
            }
        }

        #endregion
    }
}