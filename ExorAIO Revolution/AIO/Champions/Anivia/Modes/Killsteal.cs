
using System.Linq;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Anivia
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal()
        {
            /// <summary>
            ///     The Q KillSteal Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range).Where(t =>
                    UtilityClass.Player.GetSpellDamage(t, SpellSlot.Q) >= t.GetRealHealth()))
                {
                    switch (UtilityClass.Player.SpellBook.GetSpell(SpellSlot.Q).ToggleState)
                    {
                        case 1:
                            SpellClass.Q.Cast(target);
                            break;
                    }
                    break;
                }

                if (FlashFrost != null &&
                    GameObjects.EnemyHeroes.Any(t =>
                        t.IsValidTarget(SpellClass.Q.Width, checkRangeFrom: FlashFrost.Position) &&
                        UtilityClass.Player.GetSpellDamage(t, SpellSlot.Q) >= t.GetRealHealth()))
                {
                    SpellClass.Q.Cast();
                }
            }

            /// <summary>
            ///     The E KillSteal Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["killsteal"].As<MenuBool>().Enabled)
            {
                foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.E.Range).Where(t =>
                    GetFrostBiteDamage(t) >= t.GetRealHealth()))
                {
                    UtilityClass.CastOnUnit(SpellClass.E, target);
                    break;
                }
            }
        }

        #endregion
    }
}