
using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Syndra
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
            if (SpellClass.Q.Ready)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical) &&
                    MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
                {
                    SpellClass.Q.Cast(SpellClass.Q.GetPrediction(bestTarget).CastPosition);
                }
            }

            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.W.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical))
                {
                    if (!IsHoldingForceOfWillObject())
                    {
                        var obj = GetForceOfWillObject();
                        if (obj != null &&
                            obj.IsValid &&
                            obj.Distance(UtilityClass.Player) < SpellClass.W.Range)
                        {
                            UtilityClass.CastOnUnit(SpellClass.W, obj);
                            return;
                        }
                    }
                    else
                    {
                        SpellClass.W.Cast(bestTarget.ServerPosition);
                    }
                }
            }

            /// <summary>
            ///     The E Combo Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.E.Range).Where(t =>
                    MenuClass.Spells["e"]["whitelist"][t.ChampionName.ToLower()].As<MenuBool>().Enabled))
                {
                    foreach (var sphere in DarkSpheres.Where(s =>
                        CanSphereHitUnit(target, s) &&
                        s.Key != HoldedSphere?.NetworkId))
                    {
                        SelectedDarkSphereNetworkId = sphere.Key;
                        SpellClass.E.Cast(target.ServerPosition);
                    }
                }
            }
        }

        #endregion
    }
}