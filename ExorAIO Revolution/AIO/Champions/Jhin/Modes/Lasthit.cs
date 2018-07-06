using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587
namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Jhin
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void LastHit(EntropyEventArgs args)
        {
            /// <summary>
            ///     The LastHit Q Logics.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["lasthit"]) &&
                MenuClass.Spells["q"]["lasthit"].As<MenuSliderBool>().Enabled)
            {
                if (!IsReloading() &&
                    MenuClass.Spells["q"]["customization"]["lasthitonreload"].As<MenuBool>().Enabled)
                {
                    return;
                }

                foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.Q.Range))
                {
                    if (minion.GetRealHealth() <= UtilityClass.Player.GetSpellDamage(minion, SpellSlot.Q))
                    {
                        SpellClass.Q.CastOnUnit(minion);
                    }
                }
            }
        }

        #endregion
    }
}