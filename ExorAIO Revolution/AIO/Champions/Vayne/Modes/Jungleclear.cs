
using Entropy;
using Entropy.SDK.Damage;
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
    internal partial class Vayne
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on post attack.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PostAttackEventArgs" /> instance containing the event data.</param>
        public void Jungleclear(object sender, PostAttackEventArgs args)
        {
            var jungleTarget = args.Target as Obj_AI_Minion;
            if (jungleTarget == null ||
                !Extensions.GetGenericJungleMinionsTargets().Contains(jungleTarget) ||
                jungleTarget.GetRealHealth() < UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 2)
            {
                return;
            }

            /// <summary>
            ///     The E Jungleclear Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["jungleclear"]) &&
                MenuClass.Spells["e"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                const int condemnPushDistance = 410;
                var playerPos = UtilityClass.Player.ServerPosition;

                var targetPos = jungleTarget.ServerPosition;
                var predPosition = SpellClass.E.GetPrediction(jungleTarget).CastPosition;

                for (var i = 60; i < condemnPushDistance; i += 10)
                {
                    if (!targetPos.Extend(playerPos, -i).IsWall(true) ||
                        !targetPos.Extend(playerPos, -i-60).IsWall(true))
                    {
                        continue;
                    }

                    if (MenuClass.Spells["e"]["emode"].As<MenuList>().Value == 0)
                    {
                        if (!predPosition.Extend(playerPos, -i).IsWall(true) ||
                            !predPosition.Extend(playerPos, -i-60).IsWall(true))
                        {
                            continue;
                        }
                    }

                    UtilityClass.CastOnUnit(SpellClass.E, jungleTarget);
                }
            }

            /// <summary>
            ///     The Q Jungleclear Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["jungleclear"]) &&
                MenuClass.Spells["q"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.Q.Cast(Game.CursorPos);
            }
        }

        #endregion
    }
}