
using Aimtec;
using AIO.Utilities;

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
        public void Automatic()
        {
            if (UtilityClass.Player.SpellBook.GetSpell(SpellSlot.E).Level == 5)
            {
                SpellClass.E.Width = UtilityClass.GetAngleByDegrees(60);
            }

            if (UtilityClass.Player.SpellBook.GetSpell(SpellSlot.R).Level == 3)
            {
                SpellClass.R.Range = 750f;
            }
        }

        #endregion
    }
}