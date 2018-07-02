
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
    internal partial class Lucian
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Harass()
        {
            /// <summary>
            ///     The Extended Q Mixed Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q2"]["mixed"]) &&
                MenuClass.Spells["q2"]["mixed"].As<MenuSliderBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q2.Range).Where(t =>
                    !t.IsValidTarget(SpellClass.Q.Range) &&
                    MenuClass.Spells["q2"]["whitelist"][t.ChampionName.ToLower()].Enabled))
                {
                    foreach (var minion in Extensions.GetAllGenericUnitTargetsInRange(SpellClass.Q.Range))
                    {
                        if (minion.NetworkId != target.NetworkId &&
                            QRectangle(minion).IsInside((Vector2)target.ServerPosition))
                        {
                            UtilityClass.CastOnUnit(SpellClass.Q, minion);
                            break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}