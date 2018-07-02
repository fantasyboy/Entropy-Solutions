
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Caitlyn
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal()
        {
            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range).Where(t =>
                    !t.IsValidTarget(UtilityClass.Player.GetFullAttackRange(t))))
                {
                    var collisions = SpellClass.Q.GetPrediction(target).CollisionObjects
                        .Where(c => Extensions.GetAllGenericMinionsTargetsInRange(SpellClass.Q.Range).Contains(c))
                        .ToList();
                    if (collisions.Any())
                    {
                        if (target.HasBuff("caitlynyordletrapsight"))
                        {
                            if (UtilityClass.Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondForm) >= target.GetRealHealth())
                            {
                                SpellClass.Q.Cast(target);
                                break;
                            }
                        }
                        else
                        {
                            if (UtilityClass.Player.GetSpellDamage(target, SpellSlot.Q) >= target.GetRealHealth())
                            {
                                SpellClass.Q.Cast(target);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (UtilityClass.Player.GetSpellDamage(target, SpellSlot.Q, DamageStage.SecondForm) >= target.GetRealHealth())
                        {
                            SpellClass.Q.Cast(target);
                            break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}