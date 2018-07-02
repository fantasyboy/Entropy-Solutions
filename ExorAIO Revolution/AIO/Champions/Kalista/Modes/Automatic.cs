
using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using AIO.Utilities;

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
            var passiveObject = ObjectManager.Get<GameObject>().FirstOrDefault(o => o.IsValid && o.Name == "Kalista_Base_P_LinkIcon.troy");
            if (passiveObject != null)
            {
                SoulBound = GameObjects.AllyHeroes
                    .Where(a => !a.IsMe)
                    .MinBy(o => o.Distance(passiveObject));
            }

            if (UtilityClass.Player.IsRecalling())
            {
                return;
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
                if (SoulBound.CountEnemyHeroesInRange(800f) > 0 &&
                    SoulBound.HealthPercent() <=
                        MenuClass.Spells["r"]["lifesaver"].As<MenuSliderBool>().Value &&
                    MenuClass.Spells["r"]["lifesaver"].As<MenuSliderBool>().Enabled)
                {
                    SpellClass.R.Cast();
                }

                var option = RLogics.FirstOrDefault(k => k.Key == SoulBound.ChampionName).Value;
                if (option != null)
                {
                    var buffName = option.Item1;
                    var menuOption = option.Item2;

                    /// <summary>
                    ///     The Offensive R Logics.
                    /// </summary>
                    if (RLogics.ContainsKey(SoulBound.ChampionName) &&
                        GameObjects.EnemyHeroes.Any(t =>
                            t.HasBuff(buffName) &&
                            MenuClass.Spells["r"][menuOption].As<MenuBool>().Enabled &&
                            t.Distance(UtilityClass.Player.ServerPosition) > UtilityClass.Player.GetFullAttackRange(t)))
                    {
                        SpellClass.R.Cast();
                    }
                }
            }

            /// <summary>
            ///     The Spot W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                !UtilityClass.Player.IsUnderEnemyTurret() &&
                ImplementationClass.IOrbwalker.Mode == OrbwalkingMode.None &&
                UtilityClass.Player.CountEnemyHeroesInRange(1500f) == 0 &&
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.Spells["w"]["logical"]) &&
                MenuClass.Spells["w"]["logical"].As<MenuSliderBool>().Enabled)
            {
                foreach (var loc in Locations.Where(l =>
                    UtilityClass.Player.Distance(l) <= SpellClass.W.Range &&
                    !ObjectManager.Get<Obj_AI_Minion>().Any(m => m.Distance(l) <= 1000f && m.UnitSkinName.Equals("KalistaSpawn"))))
                {
                    SpellClass.W.Cast(loc);
                }
            }
        }

        /// <summary>
        ///     Fired as fast as possible.
        /// </summary>
        public void RendAutomatic()
        {
            /// <summary>
            ///     The Automatic E Logics.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["ondeath"].As<MenuBool>().Enabled &&
                ImplementationClass.IHealthPrediction.GetPrediction(UtilityClass.Player, 1000 + Game.Ping) <= 0)
            {
                SpellClass.E.Cast();
            }
        }

        #endregion
    }
}