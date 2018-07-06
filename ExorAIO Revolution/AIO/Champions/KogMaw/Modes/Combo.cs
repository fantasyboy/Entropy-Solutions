
using System.Linq;
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
    internal partial class KogMaw
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Combo(EntropyEventArgs args)
        {
            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                GameObjects.EnemyHeroes.Any(t => t.IsValidTarget(SpellClass.W.Range)) &&
                MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
            {
                SpellClass.W.Cast();
            }

            /// <summary>
            ///     The R Combo Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["combo"].As<MenuSliderBool>().Enabled &&
                MenuClass.Spells["r"]["combo"].As<MenuSliderBool>().Value >=
                    UtilityClass.Player.GetRealBuffCount("kogmawlivingartillerycost"))
            {
                foreach (var target in Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.R.Range).Where(t =>
                    t.HPPercent() <= 40 &&
                    !Invulnerable.Check(t, DamageType.Magical) &&
                    MenuClass.Spells["r"]["whitelist"][t.CharName.ToLower()].As<MenuBool>().Enabled))
                {
                    if (target.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(target)))
                    {
                        if (MenuClass.Miscellaneous["onlyroutaarange"].As<MenuBool>().Enabled)
                        {
                            return;
                        }

                        if (IsUsingBioArcaneBarrage() &&
                            MenuClass.Miscellaneous["onlyroutw"].As<MenuBool>().Enabled)
                        {
                            return;
                        }
                    }

                    SpellClass.R.Cast(target);
                }
            }
        }

        #endregion
    }
}