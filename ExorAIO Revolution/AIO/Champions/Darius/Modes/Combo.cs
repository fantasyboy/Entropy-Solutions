
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression

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
    internal partial class Darius
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Combo(EntropyEventArgs args)
        {
            /// <summary>
            ///     The E Combo Logic.
            /// </summary>
            if (SpellClass.E.Ready)
            {
                var heroTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (heroTarget != null &&
                    !Invulnerable.Check(heroTarget, DamageType.Magical, false) &&
                    MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
                {
                    SpellClass.E.Cast(heroTarget);
                }
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready)
            {
                var heroTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                if (heroTarget != null &&
                    !Invulnerable.Check(heroTarget) &&
                    MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
                {
                    if ((SpellClass.W.Ready || UtilityClass.Player.HasBuff("dariusnoxiantacticsonh")) &&
                        MenuClass.Spells["q"]["customization"]["onlyqafterw"].As<MenuBool>().Enabled)
                    {
                        return;
                    }

                    switch (MenuClass.Spells["q"]["modes"]["combo"].As<MenuList>().Value)
                    {
                        case 0:
                            if (IsValidBladeTarget(heroTarget))
                            {
                                SpellClass.Q.Cast();
                            }
                            break;
                        case 1:
                            SpellClass.Q.Cast();
                            break;
                    }
                }
            }
        }

        #endregion
    }
}