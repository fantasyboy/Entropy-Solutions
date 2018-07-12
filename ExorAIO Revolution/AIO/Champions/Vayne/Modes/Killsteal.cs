
using System.Linq;
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
    internal partial class Vayne
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal(EntropyEventArgs args)
        {
            /// <summary>
            ///     The E KillSteal Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.E.Range).Where(t =>
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.E) +
                    (t.GetBuffCount("vaynesilvereddebuff") == 2 ? UtilityClass.Player.GetSpellDamage(t, SpellSlot.W) : 0) >= t.GetRealHealth()))
                {
                    SpellClass.E.CastOnUnit(target);
                    break;
                }
            }
        }

        #endregion
    }
}