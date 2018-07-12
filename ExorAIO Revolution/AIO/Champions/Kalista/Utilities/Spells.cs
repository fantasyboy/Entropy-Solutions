using Entropy;
using AIO.Utilities;
using Entropy.SDK.Spells;

namespace AIO.Champions
{
    /// <summary>
    ///     The settings class.
    /// </summary>
    internal partial class Kalista
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 1150f);
            SpellClass.W = new Spell(SpellSlot.W, 3000f);
            SpellClass.E = new Spell(SpellSlot.E, 1000f);
            SpellClass.R = new Spell(SpellSlot.R, 1100f);

            SpellClass.Q.SetSkillshot(0.25f, 60f, 1750f);
        }

        #endregion
    }
}