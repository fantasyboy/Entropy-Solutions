
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;

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
                MenuClass.Root["q"]["combo"].Enabled)
            {
                if (heroTarget.GetBuffCount("vaynesilvereddebuff") != 1 &&
                    MenuClass.Root["q"]["customization"]["wstacks"].Enabled)
                {
                    return;
                }

                if (UtilityClass.Player.Distance(Hud.CursorPositionUnclipped) <= UtilityClass.Player.GetAutoAttackRange() &&
                    MenuClass.Root["q"]["customization"]["onlyqifmouseoutaarange"].Enabled)
                {
                    return;
                }

                var posAfterQ = UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, 300f);
                var qRangeCheck = MenuClass.Root["q"]["customization"]["qrangecheck"];
                if (qRangeCheck != null)
                {
                    if (qRangeCheck.Enabled &&
                        posAfterQ.EnemyHeroesCount(UtilityClass.Player.GetAutoAttackRange() + UtilityClass.Player.BoundingRadius) >= qRangeCheck.Value)
                    {
                        return;
                    }
                }

                if (posAfterQ.Distance(heroTarget) >
                        UtilityClass.Player.GetAutoAttackRange(heroTarget) &&
                    MenuClass.Root["q"]["customization"]["noqoutaarange"].Enabled)
                {
                    return;
                }

                if (posAfterQ.IsUnderEnemyTurret() &&
                    MenuClass.Root["q"]["customization"]["noqturret"].Enabled)
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
                MenuClass.Root["e"]["killsteal"].Enabled)
            {
                var shouldIncludeWDamage = heroTarget.GetBuffCount("vaynesilvereddebuff") == 1;
                if (UtilityClass.Player.GetAutoAttackDamage(heroTarget) + GetEDamage(heroTarget) +
                    (shouldIncludeWDamage ? GetWDamage(heroTarget) : 0) >= heroTarget.GetRealHealth(DamageType.Physical))
                {
                    SpellClass.E.CastOnUnit(heroTarget);
                }
            }
        }

        #endregion
    }
}