
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;

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
        
        /// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
        public void Jungleclear(OnPostAttackEventArgs args)
        {
            var jungleTarget = args.Target as AIMinionClient;
            if (jungleTarget == null ||
                !Extensions.GetGenericJungleMinionsTargets().Contains(jungleTarget) ||
                jungleTarget.GetRealHealth(DamageType.Physical) < UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 2)
            {
                return;
            }

            /// <summary>
            ///     The E Jungleclear Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["jungleclear"]) &&
                MenuClass.Spells["e"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                const int condemnPushDistance = 410;
                var playerPos = UtilityClass.Player.Position;

                var targetPos = jungleTarget.Position;
                var predPosition = SpellClass.E.GetPrediction(jungleTarget).CastPosition;

                for (var i = 60; i < condemnPushDistance; i += 10)
                {
                    if (!targetPos.Extend(playerPos, -i).IsWall() ||
                        !targetPos.Extend(playerPos, -i-60).IsWall())
                    {
                        continue;
                    }

                    if (MenuClass.Spells["e"]["emode"].As<MenuList>().Value == 0)
                    {
                        if (!predPosition.Extend(playerPos, -i).IsWall() ||
                            !predPosition.Extend(playerPos, -i-60).IsWall())
                        {
                            continue;
                        }
                    }

                    SpellClass.E.CastOnUnit(jungleTarget);
                }
            }

            /// <summary>
            ///     The Q Jungleclear Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["jungleclear"]) &&
                MenuClass.Spells["q"]["jungleclear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.Q.Cast(Hud.CursorPositionUnclipped);
            }
        }

        #endregion
    }
}