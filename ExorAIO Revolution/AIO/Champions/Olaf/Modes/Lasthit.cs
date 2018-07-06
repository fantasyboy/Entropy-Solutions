using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587
namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Olaf
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void LastHit(EntropyEventArgs args)
        {
            /// <summary>
            ///     The E Big Minions Lasthit Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["lasthit"].As<MenuBool>().Enabled)
            {
                foreach (var minion in Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.E.Range).Where(m =>
                    (m.CharName.Contains("Siege") || m.CharName.Contains("Super")) &&
                    m.GetRealHealth() < UtilityClass.Player.GetSpellDamage(m, SpellSlot.E)))
                {
                    SpellClass.E.CastOnUnit(minion);
                }
            }
        }

        #endregion
    }
}