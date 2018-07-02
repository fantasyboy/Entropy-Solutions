
using System.Linq;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Evelynn
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Laneclear()
        {
            /// <summary>
            ///     The Q Laneclear Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                (UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["laneclear"]) || !IsHateSpikeSkillshot()) &&
                MenuClass.Spells["q"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                var minion = Extensions.GetEnemyLaneMinionsTargetsInRange(GetRealQRange()).FirstOrDefault();
                if (minion != null)
                {
                    if (IsHateSpikeSkillshot())
                    {
                        SpellClass.Q.Cast(minion);
                    }
                    else
                    {
                        UtilityClass.CastOnUnit(SpellClass.Q, minion);
                    }
                }
            }

            /// <summary>
            ///     The E Laneclear Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["laneclear"]) &&
                MenuClass.Spells["e"]["laneclear"].As<MenuSliderBool>().Enabled)
            {
                var minion = Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.E.Range).FirstOrDefault(m => m.Name.Contains("Siege") || m.Name.Contains("Super"));
                if (minion != null)
                {
                    UtilityClass.CastOnUnit(SpellClass.E, minion);
                }
            }
        }

        #endregion
    }
}