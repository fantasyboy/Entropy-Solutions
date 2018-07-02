
using System.Linq;
using Entropy;
using Entropy.SDK.Events;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Vayne
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Combo()
        {
            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["engage"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget) &&
                    !bestTarget.IsValidTarget(UtilityClass.Player.GetFullAttackRange(bestTarget)))
                {
                    var posAfterQ = UtilityClass.Player.ServerPosition.Extend(Game.CursorPos, 300f);
                    if (posAfterQ.CountEnemyHeroesInRange(1000f) < 3 &&
                        UtilityClass.Player.Distance(Game.CursorPos) > UtilityClass.Player.AttackRange &&
                        bestTarget.Distance(posAfterQ) < UtilityClass.Player.GetFullAttackRange(bestTarget))
                    {
                        SpellClass.Q.Cast(posAfterQ);
                    }
                }
            }
        }

        /// <summary>
        ///     Fired as soon as possible.
        /// </summary>
        public void CondemnCombo()
        {
            /// <summary>
            ///     The E Stun Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                !UtilityClass.Player.IsDashing() &&
                MenuClass.Spells["e"]["emode"].As<MenuList>().Value != 2)
            {
                const int condemnPushDistance = 475;
                const int threshold = 50;

                foreach (var target in
                    GameObjects.EnemyHeroes.Where(t =>
                        !t.IsDashing() &&
                        t.IsValidTarget(SpellClass.E.Range) &&
                        !Invulnerable.Check(t, DamageType.Magical, false) &&
                        MenuClass.Spells["e"]["whitelist"][t.ChampionName.ToLower()].Enabled))
                {
                    for (var i = UtilityClass.Player.BoundingRadius; i < condemnPushDistance - threshold; i += 10)
                    {
                        switch (MenuClass.Spells["e"]["emode"].As<MenuList>().Value)
                        {
                            case 0:
                                if (IsPerfectWallPosition(target.ServerPosition, target, UtilityClass.Player.ServerPosition, i))
                                {
                                    if (target.IsImmobile(SpellClass.E.Delay))
                                    {
                                        UtilityClass.CastOnUnit(SpellClass.E, target);
                                        break;
                                    }

                                    var estimatedPosition = EstimatedPosition(target, SpellClass.E.Delay);
                                    if (IsPerfectWallPosition(estimatedPosition, target, UtilityClass.Player.ServerPosition, i))
                                    {
                                        UtilityClass.CastOnUnit(SpellClass.E, target);
                                    }
                                }
                                break;

                            default:
                                if (IsPerfectWallPosition(target.ServerPosition, target, UtilityClass.Player.ServerPosition, i))
                                {
                                    UtilityClass.CastOnUnit(SpellClass.E, target);
                                }
                                break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}