
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Taliyah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Harass()
        {
            /// <summary>
            ///     The Q Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                (IsNearWorkedGround() ||
                 UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["harass"])) &&
                MenuClass.Spells["q"]["harass"].As<MenuSliderBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range - 100f);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget, DamageType.Magical) &&
                    MenuClass.Spells["q"]["whitelist"][bestTarget.ChampionName.ToLower()].As<MenuBool>().Enabled)
                {
                    switch (MenuClass.Spells["q"]["modes"]["harass"].As<MenuList>().Value)
                    {
                        case 0:
                            if (!IsNearWorkedGround())
                            {
                                SpellClass.Q.Cast(bestTarget);
                            }
                            break;
                        case 1:
                            SpellClass.Q.Cast(bestTarget);
                            break;
                    }
                }
            }
        }

        #endregion
    }
}