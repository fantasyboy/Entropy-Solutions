
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;
using SharpDX;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Lucian
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
                /// <summary>
                ///     Normal.
                /// </summary>
                if (MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Enabled)
                {
                    foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range).Where(t =>
                        UtilityClass.Player.GetSpellDamage(t, SpellSlot.Q) >= t.GetRealHealth()))
                    {
                        SpellClass.Q.CastOnUnit(target);
                        break;
                    }
                }

                /// <summary>
                ///     Extended.
                /// </summary>
                if (MenuClass.Spells["q2"]["killsteal"].As<MenuBool>().Enabled)
                {
                    foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q2.Range).Where(t =>
                        !t.IsValidTarget(SpellClass.Q.Range) &&
                        UtilityClass.Player.GetSpellDamage(t, SpellSlot.Q) >= t.GetRealHealth()))
                    {
                        foreach (var minion in Extensions.GetAllGenericUnitTargetsInRange(SpellClass.Q.Range))
                        {
                            if (minion.NetworkID != target.NetworkID &&
                                QRectangle(minion).IsInside((Vector2)target.Position))
                            {
                                SpellClass.Q.CastOnUnit(minion);
                                break;
                            }
                        }
                    }
                }
            }

            /// <summary>
            ///     The KillSteal W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.W.Range).Where(t =>
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.W) >= t.GetRealHealth()))
                {
                    SpellClass.W.Cast(target);
                    break;
                }
            }
        }

        #endregion
    }
}