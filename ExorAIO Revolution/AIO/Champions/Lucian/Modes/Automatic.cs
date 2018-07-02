
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using Entropy.SDK.Util;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Lucian
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic()
        {
            ImplementationClass.IOrbwalker.AttackingEnabled = !IsCulling();

            /// <summary>
            ///     The Automatic R Orbwalking.
            /// </summary>
            if (MenuClass.Spells["r"]["bool"].As<MenuBool>().Enabled &&
                MenuClass.Spells["r"]["key"].As<MenuKeyBind>().Enabled)
            {
                DelayAction.Queue(100 + Game.Ping, () =>
                    {
                        UtilityClass.Player.IssueOrder(OrderType.MoveTo, Game.CursorPos);
                    });
            }

            /// <summary>
            ///     The Semi-Automatic R Management.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["bool"].As<MenuBool>().Enabled)
            {
                var bestTarget = GameObjects.EnemyHeroes
                    .Where(t =>
                        !Invulnerable.Check(t) &&
                        t.IsValidTarget(SpellClass.R.Range) &&
                        MenuClass.Spells["r"]["whitelist"][t.ChampionName.ToLower()].As<MenuBool>().Enabled)
                    .MinBy(o => o.GetRealHealth());

                if (!IsCulling() &&
                    bestTarget != null &&
                    MenuClass.Spells["r"]["key"].As<MenuKeyBind>().Enabled)
                {
                    if (SpellClass.W.Ready &&
                        bestTarget.IsValidTarget(SpellClass.W.Range))
                    {
                        SpellClass.W.Cast(bestTarget.ServerPosition);
                    }
                    SpellClass.R.Cast(bestTarget.ServerPosition);
                }
                else if (UtilityClass.Player.HasBuff("LucianR") &&
                     !MenuClass.Spells["r"]["key"].As<MenuKeyBind>().Enabled)
                {
                    SpellClass.R.Cast();
                }
            }
        }

        #endregion
    }
}