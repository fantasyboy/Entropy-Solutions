using Entropy;
using AIO.Utilities;
using Entropy.SDK.Spells;

namespace AIO.Champions
{
	/// <summary>
	///     The spells class.
	/// </summary>
	internal partial class Ezreal
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public void Spells()
        {
            SpellClass.Q = new Spell(SpellSlot.Q, 1150f);
            SpellClass.W = new Spell(SpellSlot.W, 1000f);
            SpellClass.E = new Spell(SpellSlot.E, 475f);
            SpellClass.R = new Spell(SpellSlot.R, 1500f);

            SpellClass.Q.SetSkillshot(0.25f, SpellClass.Q.SpellData.LineWidth, SpellClass.Q.SpellData.MissileSpeed);
            SpellClass.W.SetSkillshot(0.25f, SpellClass.W.SpellData.LineWidth, SpellClass.W.SpellData.MissileSpeed, collision: false);
            SpellClass.R.SetSkillshot(1f, SpellClass.R.SpellData.LineWidth, SpellClass.R.SpellData.MissileSpeed, collision: false);
        }

        #endregion
    }
}