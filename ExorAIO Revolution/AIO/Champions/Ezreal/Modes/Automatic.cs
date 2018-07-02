
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
    internal partial class Ezreal
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic()
        {
            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The Anti-Grab Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.IsBeingGrabbed() &&
                MenuClass.Spells["e"]["antigrab"].As<MenuBool>().Enabled)
            {
                var firstTower = ObjectManager.Get<Obj_AI_Turret>()
                    .Where(t => t.IsAlly && t.IsValidTarget(allyIsValidTarget: true))
                    .MinBy(t => t.Distance(UtilityClass.Player));
                if (firstTower != null)
                {
                    SpellClass.E.Cast(UtilityClass.Player.ServerPosition.Extend(firstTower.ServerPosition, SpellClass.E.Range));
                }
            }

            /// <summary>
            ///     The Tear Stacking Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.IsTearLikeItemReady() &&
                UtilityClass.Player.CountEnemyHeroesInRange(1500f) == 0 &&
                ImplementationClass.IOrbwalker.Mode == OrbwalkingMode.None &&
                !Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.Q.Range).Any() &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Miscellaneous["tear"]) &&
                MenuClass.Miscellaneous["tear"].As<MenuSliderBool>().Enabled)
            {
                SpellClass.Q.Cast(Game.CursorPos);
            }

            /// <summary>
            ///     The Semi-Automatic R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["bool"].As<MenuBool>().Enabled &&
                MenuClass.Spells["r"]["key"].As<MenuKeyBind>().Enabled)
            {
                var bestTarget = GameObjects.EnemyHeroes.Where(t =>
                        t.IsValidTarget(2000f) &&
                        !Invulnerable.Check(t, DamageType.Magical, false) &&
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