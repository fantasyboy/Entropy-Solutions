
using System.Linq;
using AIO.Utilities;
using Entropy;
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
        public void Harass(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Extended Q Mixed Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q2"]["mixed"]) &&
                MenuClass.Spells["q2"]["mixed"].As<MenuSliderBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q2.Range).Where(t =>
                    !t.IsValidTarget(SpellClass.Q.Range) &&
                    MenuClass.Spells["q2"]["whitelist"][t.CharName.ToLower()].Enabled))
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

        #endregion
    }
}