
using Entropy;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Evelynn
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Automatic(EntropyEventArgs args)
        {
            if (!UtilityClass.Player.GetSpell(SpellSlot.W).State.HasFlag(SpellState.NotLearned))
            {
                SpellClass.W.Range = 1100 + 100 * UtilityClass.Player.GetSpell(SpellSlot.W).Level;
            }
        }

        #endregion
    }
}