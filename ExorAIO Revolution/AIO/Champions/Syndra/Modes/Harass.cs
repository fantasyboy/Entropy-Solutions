
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

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
        public void Harass(EntropyEventArgs args)
        {
            /// <summary>
            ///     The W Harass Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["harass"]) &&
                MenuClass.Spells["w"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.W.Range + 200f);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical) &&
                    MenuClass.Spells["w"]["whitelist"][bestTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                {
                    if (!IsHoldingForceOfWillObject())
                    {
                        var obj = GetForceOfWillObject();
                        if (obj != null &&
                            obj.IsValid &&
                            obj.Distance(UtilityClass.Player) < SpellClass.W.Range)
                        {
                            SpellClass.W.CastOnUnit(obj);
                            return;
                        }
                    }
                    else
                    {
                        SpellClass.W.Cast(bestTarget.Position);
                    }
                }
            }

            /// <summary>
            ///     The Q Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["harass"]) &&
                MenuClass.Spells["q"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.Q.Range)
                    .FirstOrDefault(c => MenuClass.Spells["q"]["whitelist"][c.CharName.ToLower()].As<MenuBool>().Enabled);
                if (bestTarget != null)
                {
                    SpellClass.Q.Cast(SpellClass.Q.GetPrediction(bestTarget).CastPosition);
                }
            }
        }

        #endregion
    }
}