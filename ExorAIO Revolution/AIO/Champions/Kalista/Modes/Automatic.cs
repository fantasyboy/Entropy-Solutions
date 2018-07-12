
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Caching;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Kalista
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic()
        {
            var passiveObject = ObjectCache.AllGameObjects.FirstOrDefault(o => o.IsValid && o.Name == "Kalista_Base_P_LinkIcon.troy");
            if (passiveObject != null)
            {
                SoulBound = GameObjects.AllyHeroes
                    .Where(a => !a.IsMe())
                    .MinBy(o => o.Distance(passiveObject));
            }

            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

	        /// <summary>
	        ///     The Automatic E Logics.
	        /// </summary>
	        if (SpellClass.E.Ready                                                &&
	            MenuClass.Spells["e"]["beforedeath"].As<MenuSliderBool>().Enabled &&
	            LocalPlayer.Instance.HPPercent() <= MenuClass.Spells["e"]["beforedeath"].As<MenuSliderBool>().Value)
	        {
		        SpellClass.E.Cast();
	        }

			/// <summary>
			///     The R Logics.
			/// </summary>
			if (SpellClass.R.Ready &&
                SoulBound != null)
            {
                /// <summary>
                ///     The Lifesaver R Logic.
                /// </summary>
                if (SoulBound.EnemyHeroesCount(800f) > 0 &&
                    SoulBound.HPPercent() <=
                        MenuClass.Spells["r"]["lifesaver"].As<MenuSliderBool>().Value &&
                    MenuClass.Spells["r"]["lifesaver"].As<MenuSliderBool>().Enabled)
                {
                    SpellClass.R.Cast();
                }

                var option = RLogics.FirstOrDefault(k => k.Key == SoulBound.CharName).Value;
                if (option != null)
                {
                    var buffName = option.Item1;
                    var menuOption = option.Item2;

                    /// <summary>
                    ///     The Offensive R Logics.
                    /// </summary>
                    if (RLogics.ContainsKey(SoulBound.CharName) &&
                        GameObjects.EnemyHeroes.Any(t =>
                            t.HasBuff(buffName) &&
                            MenuClass.Spells["r"][menuOption].As<MenuBool>().Enabled &&
                            t.Distance(UtilityClass.Player.Position) > UtilityClass.Player.GetAutoAttackRange(t)))
                    {
                        SpellClass.R.Cast();
                    }
                }
            }

            /// <summary>
            ///     The Spot W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                !UtilityClass.Player.Position.IsUnderEnemyTurret() &&
                Orbwalker.Mode == OrbwalkingMode.None &&
                UtilityClass.Player.EnemyHeroesCount(1500f) == 0 &&
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["logical"]) &&
                MenuClass.Spells["w"]["logical"].As<MenuSliderBool>().Enabled)
            {
                foreach (var loc in Locations.Where(l =>
                    UtilityClass.Player.Distance(l) <= SpellClass.W.Range &&
                    !ObjectCache.AllMinions.Any(m => m.Distance(l) <= 1000f && m.CharName.Equals("KalistaSpawn"))))
                {
                    SpellClass.W.Cast(loc);
                }
            }
        }

        #endregion
    }
}