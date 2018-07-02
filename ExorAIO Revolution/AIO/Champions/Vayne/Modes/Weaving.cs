
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
    internal partial class Vayne
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on post attack.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PostAttackEventArgs" /> instance containing the event data.</param>
        public void Weaving(object sender, PostAttackEventArgs args)
        {
            var heroTarget = args.Target as Obj_AI_Hero;
            if (heroTarget == null)
            {
                return;
            }

            /// <summary>
            ///     The Q Weaving Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                if (heroTarget.GetRealBuffCount("vaynesilvereddebuff") != 1 &&
                    MenuClass.Spells["q"]["customization"]["wstacks"].As<MenuBool>().Enabled)
                {
                    return;
                }

                if (UtilityClass.Player.Distance(Game.CursorPos) <= UtilityClass.Player.AttackRange &&
                    MenuClass.Spells["q"]["customization"]["onlyqifmouseoutaarange"].As<MenuBool>().Enabled)
                {
                    return;
                }

                var posAfterQ = UtilityClass.Player.ServerPosition.Extend(Game.CursorPos, 300f);
                var qRangeCheck = MenuClass.Spells["q"]["customization"]["qrangecheck"];
                if (qRangeCheck != null)
                {
                    if (qRangeCheck.As<MenuSliderBool>().Enabled &&
                        posAfterQ.CountEnemyHeroesInRange(UtilityClass.Player.AttackRange + UtilityClass.Player.BoundingRadius) >= qRangeCheck.As<MenuSliderBool>().Value)
                    {
                        return;
                    }
                }

                if (posAfterQ.Distance(heroTarget) >
                        UtilityClass.Player.GetFullAttackRange(heroTarget) &&
                    MenuClass.Spells["q"]["customization"]["noqoutaarange"].As<MenuBool>().Enabled)
                {
                    return;
                }

                if (posAfterQ.PointUnderEnemyTurret() &&
                    MenuClass.Spells["q"]["customization"]["noqturret"].As<MenuBool>().Enabled)
                {
                    return;
                }

                SpellClass.Q.Cast(Game.CursorPos);
            }

            if (heroTarget.IsZombie())
            {
                return;
            }

            /// <summary>
            ///     The E KillSteal Weaving Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                !Invulnerable.Check(heroTarget) &&
                heroTarget.IsValidTarget(SpellClass.E.Range+heroTarget.BoundingRadius) &&
                MenuClass.Spells["e"]["killsteal"].As<MenuBool>().Enabled)
            {
                var shouldIncludeWDamage = heroTarget.GetBuffCount("vaynesilvereddebuff") == 1;
                if (UtilityClass.Player.GetAutoAttackDamage(heroTarget) +
                    UtilityClass.Player.GetSpellDamage(heroTarget, SpellSlot.E) +
                    (shouldIncludeWDamage ? UtilityClass.Player.GetSpellDamage(heroTarget, SpellSlot.W) : 0) >= heroTarget.GetRealHealth())
                {
                    UtilityClass.CastOnUnit(SpellClass.E, heroTarget);
                }
            }
        }

        #endregion
    }
}