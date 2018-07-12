
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
        public void Weaving(OnPostAttackEventArgs args)
        {
            var heroTarget = args.Target as AIHeroClient;
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

                if (UtilityClass.Player.Distance(Hud.CursorPositionUnclipped) <= UtilityClass.Player.GetAutoAttackRange() &&
                    MenuClass.Spells["q"]["customization"]["onlyqifmouseoutaarange"].As<MenuBool>().Enabled)
                {
                    return;
                }

                var posAfterQ = UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, 300f);
                var qRangeCheck = MenuClass.Spells["q"]["customization"]["qrangecheck"];
                if (qRangeCheck != null)
                {
                    if (qRangeCheck.As<MenuSliderBool>().Enabled &&
                        posAfterQ.EnemyHeroesCount(UtilityClass.Player.GetAutoAttackRange() + UtilityClass.Player.BoundingRadius) >= qRangeCheck.As<MenuSliderBool>().Value)
                    {
                        return;
                    }
                }

                if (posAfterQ.Distance(heroTarget) >
                        UtilityClass.Player.GetAutoAttackRange(heroTarget) &&
                    MenuClass.Spells["q"]["customization"]["noqoutaarange"].As<MenuBool>().Enabled)
                {
                    return;
                }

                if (posAfterQ..Position.IsUnderEnemyTurret() &&
                    MenuClass.Spells["q"]["customization"]["noqturret"].As<MenuBool>().Enabled)
                {
                    return;
                }

                SpellClass.Q.Cast(Hud.CursorPositionUnclipped);
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
                    SpellClass.E.CastOnUnit(heroTarget);
                }
            }
        }

        #endregion
    }
}