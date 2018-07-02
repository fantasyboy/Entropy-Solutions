
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using AIO.Utilities;

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
        public void Combo()
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
                    t.HealthPercent() <= 40 &&
                    !Invulnerable.Check(t, DamageType.Magical) &&
                    MenuClass.Spells["r"]["whitelist"][t.ChampionName.ToLower()].As<MenuBool>().Enabled))
                {
                    if (target.IsValidTarget(UtilityClass.Player.GetFullAttackRange(target)))
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