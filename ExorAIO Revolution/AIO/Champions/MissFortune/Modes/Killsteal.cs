
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.UI.Components;
using SharpDX;

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
        public void Killsteal(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Q Killsteal Logics.
            /// </summary>
            if (SpellClass.Q.Ready)
            {
                if (MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Enabled)
                {
                    foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range)
                        .Where(t => GetRealMissFortuneDamage(UtilityClass.Player.GetSpellDamage(t, SpellSlot.Q), t) >= t.GetRealHealth()))
                    {
                        SpellClass.Q.CastOnUnit(target);
                        break;
                    }
                }

                if (MenuClass.Spells["q2"]["killsteal"].As<MenuBool>().Enabled)
                {
                    foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q2.Range))
                    {
                        var unitsToIterate = Extensions.GetAllGenericUnitTargetsInRange(SpellClass.Q.Range)
                            .Where(m => !m.IsMoving && QCone(m).IsInside((Vector2)target.Position))
                            .OrderBy(m => m.HP)
                            .ToList();

                        var killableUnitsToIterate = unitsToIterate
                            .Where(m => m.GetRealHealth() < GetRealMissFortuneDamage(UtilityClass.Player.GetSpellDamage(m, SpellSlot.Q), m))
                            .ToList();

                        var realUnitsToIterate = killableUnitsToIterate.Any() ? killableUnitsToIterate : unitsToIterate;
                        foreach (var minion in realUnitsToIterate)
                        {
                            var damageToMinion = GetRealMissFortuneDamage(UtilityClass.Player.GetSpellDamage(minion, SpellSlot.Q), minion);
                            if (target.GetRealHealth() < GetRealMissFortuneDamage(UtilityClass.Player.GetSpellDamage(target, SpellSlot.Q, damageToMinion >= minion.GetRealHealth() ? DamageStage.Empowered : DamageStage.Default), target))
                            {
                                SpellClass.Q.CastOnUnit(minion);
                                break;
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}