
using Aimtec;
using Aimtec.SDK.Damage;
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
    internal partial class Olaf
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Harass()
        {
            /// <summary>
            ///     The Q Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["harass"]) &&
                MenuClass.Spells["q"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range - 200f);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget) &&
                    MenuClass.Spells["q"]["whitelist"][bestTarget.ChampionName.ToLower()].As<MenuBool>().Enabled)
                {
                    SpellClass.Q.Cast(SpellClass.Q.GetPrediction(bestTarget).CastPosition.Extend(UtilityClass.Player.ServerPosition, -100f - bestTarget.BoundingRadius));
                }
            }

            /// <summary>
            ///     The E Harass Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget) &&
                    MenuClass.Spells["e"]["whitelist"][bestTarget.ChampionName.ToLower()].As<MenuBool>().Enabled)
                {
                    if (UtilityClass.Player.HealthPercent() >=
                            MenuClass.Spells["e"]["harass"].As<MenuSliderBool>().Value ||
                        UtilityClass.Player.GetSpellDamage(bestTarget, SpellSlot.E) >= bestTarget.GetRealHealth())
                    {
                        UtilityClass.CastOnUnit(SpellClass.E, bestTarget);
                    }
                }
            }
        }

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PreAttackEventArgs" /> instance containing the event data.</param>
        public void Harass(object sender, PreAttackEventArgs args)
        {
            var heroTarget = args.Target as Obj_AI_Hero;
            if (heroTarget == null)
            {
                return;
            }

            /// <summary>
            ///     The W Weaving Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["harass"]) &&
                MenuClass.Spells["w"]["harass"].As<MenuSliderBool>().Enabled)
            {
                if (!Invulnerable.Check(heroTarget) &&
                    MenuClass.Spells["q"]["whitelist"][heroTarget.ChampionName.ToLower()].As<MenuBool>().Enabled)
                {
                    SpellClass.W.Cast();
                }
            }
        }

        #endregion
    }
}