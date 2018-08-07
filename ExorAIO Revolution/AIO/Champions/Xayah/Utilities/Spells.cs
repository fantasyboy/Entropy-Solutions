using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Spells;

namespace AIO.Champions
{
    /// <summary>
    ///     The settings class.
    /// </summary>
    internal partial class Xayah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 1100f);
            SpellClass.W = new Spell(SpellSlot.W);
            SpellClass.E = new Spell(SpellSlot.E, float.MaxValue);
            SpellClass.R = new Spell(SpellSlot.R, 1100f);

            SpellClass.Q.SetSkillshot(0.25f, 60f, 1400f, collision: false);
            SpellClass.E.SetSkillshot(0.5f,  45f, 2000f, collision: false);
            SpellClass.R.SetSkillshot(0.25f, 80f, 2000f, collision: false);
        }

        #endregion
    }
}