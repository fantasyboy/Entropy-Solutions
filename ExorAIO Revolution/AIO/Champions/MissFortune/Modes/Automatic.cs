
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class MissFortune
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(EntropyEventArgs args)
        {
            var passiveObject = ObjectManager.Get<GameObject>().FirstOrDefault(o => o.IsValid && o.Name == "MissFortune_Base_P_Mark.troy");
            if (passiveObject != null)
            {
                var passiveUnit = ObjectManager.Get<AttackableUnit>()
                    .MinBy(o => o.Distance(passiveObject));

                LoveTapTargetNetworkId = passiveUnit?.NetworkID ?? 0;
            }
            else
            {
                LoveTapTargetNetworkId = 0;
            }

            if (UtilityClass.Player.IsRecalling())
            {
                return;
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
                        MenuClass.Spells["r"]["whitelist"][t.CharName.ToLower()].As<MenuBool>().Enabled)
                    .MinBy(o => o.EnemyHeroesCount(300f));

                if (bestTarget != null &&
                    !IsUltimateShooting() &&
                    MenuClass.Spells["r"]["key"].As<MenuKeyBind>().Enabled)
                {
                    if (SpellClass.E.Ready &&
                        bestTarget.IsValidTarget(SpellClass.E.Range))
                    {
                        SpellClass.E.Cast(bestTarget.Position);
                    }

                    SpellClass.R.Cast(bestTarget.Position);
                }
                else if (IsUltimateShooting() &&
                     !MenuClass.Spells["r"]["key"].As<MenuKeyBind>().Enabled)
                {
                    UtilityClass.Player.IssueOrder(OrderType.MoveTo, Hud.CursorPositionUnclipped);
                }
            }
        }

        #endregion
    }
}