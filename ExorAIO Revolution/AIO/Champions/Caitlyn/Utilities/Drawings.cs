
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Rendering;
using Entropy.SDK.UI.Components;
using Color = SharpDX.Color;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class Caitlyn
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the drawings.
        /// </summary>
        public void Drawings()
        {
            /// <summary>
            ///     Loads the Q drawing.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Drawings["q"].As<MenuBool>().Enabled)
            {
	            CircleRendering.Render(Color.LightGreen, SpellClass.Q.Range, UtilityClass.Player);
            }

            /// <summary>
            ///     Loads the W drawing.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Drawings["w"].As<MenuBool>().Enabled)
            {
                CircleRendering.Render(Color.Yellow, SpellClass.W.Range, UtilityClass.Player);
            }

            /// <summary>
            ///     Loads the E drawing.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Drawings["e"].As<MenuBool>().Enabled)
            {
                CircleRendering.Render(Color.Cyan, SpellClass.E.Range, UtilityClass.Player);
            }

            /// <summary>
            ///     Loads the R drawing.
            /// </summary>
            if (SpellClass.R.Ready)
            {
                if (MenuClass.Drawings["r"].As<MenuBool>().Enabled)
                {
                    CircleRendering.Render(Color.Red, SpellClass.R.Range, UtilityClass.Player);
                }

                if (MenuClass.Drawings["rmm"].As<MenuBool>().Enabled)
                {
                    Vector2Geometry.DrawCircleOnMinimap(UtilityClass.Player.Position, SpellClass.R.Range, Color.White);
                }
            }

            /// <summary>
            ///     Loads the R damage to healthbar.
            /// </summary>
            if (MenuClass.Drawings["rdmg"].As<MenuBool>().Enabled)
            {
	            var caitlynRDamage = new[] { 250, 475, 700 }[UtilityClass.Player.Spellbook.GetSpell(SpellSlot.R).Level - 1]
	                                 + 2 * UtilityClass.Player.CharIntermediate.FlatPhysicalDamageMod;
				foreach (var hero in Extensions.GetEnemyHeroesTargetsInRange(SpellClass.R.Range).Where(h =>
                    !Invulnerable.Check(h) &&
                    h.InfoBarPosition.OnScreen()))
                {
                    var totalCaitlynRDamage = LocalPlayer.Instance.CalculateDamage(hero, DamageType.Physical, caitlynRDamage);
	                DamageIndicatorRendering.Render(hero, totalCaitlynRDamage, hero.TotalShieldHealth() < totalCaitlynRDamage ? Color.Blue : Color.Orange);
				}
            }
        }

        #endregion
    }
}