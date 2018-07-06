
using Entropy.SDK.Caching;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Rendering;
using Entropy.SDK.UI;
using Entropy.SDK.UI.Components;
using SharpDX;

using System;
using System.Linq;

using Entropy;
using Entropy.SDK.Events;
using Entropy.ToolKit;
using Color = SharpDX.Color;

namespace Devtropy
{
    internal static class Program
    {
        #region Static Fields

        //public static double LastSpellTime = 0;
	    private static Menu Menu;

        #endregion

        #region Methods

        /// <summary>
        ///     Loads the menus.
        /// </summary>
        private static void InitMenu()
        {
	        Menu = new Menu("devtropy", "Devtropy", true)
	        {
		        new MenuSlider("range", "Max object dist from cursor", 400, 50, 2000),
		        new MenuBool("showobjects", "Show GameObjects"),
		        new MenuBool("printobjects", "Print GameObjects on Console"),
	        };

	        var selection = new Menu("select", "GameObject Tracking Selection:");

			foreach (var type in Enum.GetNames(typeof(GameObjectTypeID)))
	        {
		        selection.Add(new MenuBool(type.ToLower(), type));
	        }

	        Menu.Add(selection);
			Menu.Attach();
        }

        /// <summary>
        ///     The entry point of the application.
        /// </summary>
        private static void Main()
        {
	        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
	        Loading.OnLoadingComplete                  += Loading_OnLoadingComplete;
        }

	    private static void CurrentDomain_UnhandledException(UnhandledExceptionEventArgs e)
	    {
		    if (e.ExceptionObject is Exception exception)
		    {
			    exception.ToolKitLog("Unexpected error occurred in Devtropy.");
		    }
	    }

	    private static void Loading_OnLoadingComplete()
	    {
		    try
		    {
				InitMenu();

			    Game.OnUpdate                     += OnUpdate;
			    AIBaseClient.OnProcessSpellCast   += OnProcessSpellCast;
			    AIBaseClient.OnProcessBasicAttack += OnProcessBasicAttack;
			    Spellbook.OnLocalCastSpell        += OnLocalCastSpell;
			    AIBaseClient.OnAnimationPlay      += OnAnimationPlay;
			}
		    catch (Exception exception)
		    {
			    exception.ToolKitLog();
		    }
		}

	    /// <summary>
		///     Called on do-cast.
		/// </summary>
		/// <param name="args">The <see cref="AIBaseClientCastEventArgs" /> instance containing the event data.</param>
		private static void OnProcessBasicAttack(AIBaseClientCastEventArgs args)
        {
            if (args.Caster is AIHeroClient)
            {
                Console.WriteLine("Autoattack Name: " + args.SpellData.Name);
            }
        }

		/// <summary>
		///     Called on spell cast.
		/// </summary>
		/// <param name="args">The <see cref="SpellbookLocalCastSpellEventArgs" /> instance containing the event data.</param>
		private static void OnLocalCastSpell(SpellbookLocalCastSpellEventArgs args)
        {
            /*
            if (sender.IsMe() && args.Slot == SpellSlot.Q)
            {
                LastRTime = Game.ClockTime;
            }*/
        }

		/// <summary>
		///     Called on animation trigger.
		/// </summary>
		/// <param name="args">The <see cref="AIBaseClientAnimationPlayEventArgs" /> instance containing the event data.</param>
		private static void OnAnimationPlay(AIBaseClientAnimationPlayEventArgs args)
        {
            if (args.Sender.IsMe()())
            {
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine($"Animation Name: {args.AnimationName}");
                Console.WriteLine("----------------------------------------------");
            }
        }

		/// <summary>
		///     Called on process spellcast.
		/// </summary>
		/// <param name="args">The <see cref="AIBaseClientCastEventArgs" /> instance containing the event data.</param>
		private static void OnProcessSpellCast(AIBaseClientCastEventArgs args)
        {
            if (args.Caster.IsMe()())
            {
                var spellData = args.SpellData;

                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("Name: "                      + spellData.Name);
                Console.WriteLine("Width: "                     + spellData.LineWidth);
                Console.WriteLine("CastRange: "                 + spellData.CastRange);
                Console.WriteLine("CastRadius: "                + spellData.CastRadius);
                Console.WriteLine("CastRangeDisplayOverride: "  + spellData.CastRangeDisplayOverride);
                Console.WriteLine("CastRadiusSecondary: "       + spellData.CastRadiusSecondary);
                Console.WriteLine("MissileSpeed: "              + spellData.MissileSpeed);
                Console.WriteLine("----------------------------------------------");
            }
        }

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        private static void OnUpdate(EntropyEventArgs args)
        {
            if (!Menu["showobjects"].As<MenuBool>().Enabled)
            {
                return;
            }

            foreach (var obj in ObjectCache.AllGameObjects.Where(o =>
				Menu["select"][o.Type.TypeID.ToString().ToLower()].As<MenuBool>().Enabled &&
				Enum.GetNames(typeof(GameObjectTypeID)).Contains(o.Type.TypeID.ToString()) &&
				Hud.CursorPositionUnclipped.Distance(o) < Menu["range"].Value))
            {
                switch (obj.Type.TypeID)
                {
                    case GameObjectTypeID.AIBaseClient:
                        var baseUnit = (AIBaseClient)obj;

                        TextRendering.Render("Name: "         + baseUnit.Name,       Color.OrangeRed, new Vector2(obj.Position.X, obj.Position.Y + 20));
                        TextRendering.Render("UnitSkinName: " + baseUnit.CharName,   Color.OrangeRed, new Vector2(obj.Position.X, obj.Position.Y + 35));
                        TextRendering.Render("Type: "         + baseUnit.Type,       Color.OrangeRed, new Vector2(obj.Position.X, obj.Position.Y + 50));
                        TextRendering.Render("NetworkID: "    + baseUnit.NetworkID,  Color.OrangeRed, new Vector2(obj.Position.X, obj.Position.Y + 65));
                        TextRendering.Render("X: "            + baseUnit.Position.X, Color.OrangeRed, new Vector2(obj.Position.X, obj.Position.Y + 95));
                        TextRendering.Render("Y: "            + baseUnit.Position.Y, Color.OrangeRed, new Vector2(obj.Position.X + 100, obj.Position.Y + 95));

                        var unitBuffs = baseUnit.GetActiveBuffs().ToArray();
                        if (unitBuffs.Length > 0)
                        {
                            TextRendering.Render("Buffs:", Color.Yellow, new Vector2(obj.Position.X, obj.Position.Y + 115));
                            TextRendering.Render("------", Color.Yellow, new Vector2(obj.Position.X, obj.Position.Y + 125));

                            for (var i = 0; i < unitBuffs.Length; i++)
                            {
                                TextRendering.Render(baseUnit.GetBuffCount(unitBuffs[i].Name) + "x " + unitBuffs[i].Name, Color.OrangeRed, new Vector2(obj.Position.X, obj.Position.Y + 135 + 12 * i));
                            }
                        }
                        break;

                    case GameObjectTypeID.AIHeroClient:
                        var heroUnit = (AIHeroClient)obj;
                         
                        TextRendering.Render("Name: "         + heroUnit.Name,                                                          Color.OrangeRed, new Vector2(obj.Position.X,       obj.Position.Y + 20));
                        TextRendering.Render("ChampionName: " + heroUnit.CharName,                                                      Color.OrangeRed, new Vector2(obj.Position.X,       obj.Position.Y + 35));
                        TextRendering.Render("ModelName: "    + heroUnit.ModelName,                                                     Color.OrangeRed, new Vector2(obj.Position.X,       obj.Position.Y + 50));
                        TextRendering.Render("Type: "         + heroUnit.Type,                                                          Color.OrangeRed, new Vector2(obj.Position.X,       obj.Position.Y + 65));
                        TextRendering.Render("NetworkID: "    + heroUnit.NetworkID,                                                     Color.OrangeRed, new Vector2(obj.Position.X,       obj.Position.Y + 80));
                        TextRendering.Render("Health: "       + heroUnit.HP + "/" + heroUnit.MaxHP + "(" + heroUnit.HPPercent() + "%)", Color.OrangeRed, new Vector2(obj.Position.X,       obj.Position.Y + 95));
                        TextRendering.Render("X: "            + heroUnit.Position.X,                                                    Color.OrangeRed, new Vector2(obj.Position.X,       obj.Position.Y + 110));
                        TextRendering.Render("Y: "            + heroUnit.Position.Y,                                                    Color.OrangeRed, new Vector2(obj.Position.X + 100, obj.Position.Y + 110));

                        TextRendering.Render("Spells:", Color.Yellow, new Vector2(obj.Position.X, obj.Position.Y + 130));
                        TextRendering.Render("-------", Color.Yellow, new Vector2(obj.Position.X, obj.Position.Y + 140));

                        const int spellLength = 150;
                        var spellSlots = new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R };
                        for (var i = 0; i < spellSlots.Length; i++)
                        {
                            var slot = spellSlots[i];
                            var spell = heroUnit.Spellbook.GetSpell(slot);
                            TextRendering.Render($"({slot}): {spell.SpellData.Name}", Color.OrangeRed, new Vector2(obj.Position.X,       obj.Position.Y + spellLength + 15 * i));
                            TextRendering.Render($"ToggleState: {spell.ToggleState}", Color.OrangeRed, new Vector2(obj.Position.X + 200, obj.Position.Y + spellLength + 15 * i));
                            TextRendering.Render($"Ammo: {spell.Ammo}",               Color.OrangeRed, new Vector2(obj.Position.X + 300, obj.Position.Y + spellLength + 15 * i));
                        }

                        TextRendering.Render("SummonerSpells:", Color.Yellow, new Vector2(obj.Position.X, obj.Position.Y + 220));
                        TextRendering.Render("---------------", Color.Yellow, new Vector2(obj.Position.X, obj.Position.Y + 230));

                        const int summonerSpellLength = 240;
                        var summonerSpellSlots = new[] { SpellSlot.Summoner1, SpellSlot.Summoner2 };
                        for (var i = 0; i < summonerSpellSlots.Length; i++)
                        {
                            var slot = summonerSpellSlots[i];
                            var spell = heroUnit.Spellbook.GetSpell(slot);
                            TextRendering.Render($"({slot}): {spell.SpellData.Name}", Color.OrangeRed, new Vector2(obj.Position.X,       obj.Position.Y + summonerSpellLength + 15 * i));
                            TextRendering.Render($"ToggleState: {spell.ToggleState}", Color.OrangeRed, new Vector2(obj.Position.X + 200, obj.Position.Y + summonerSpellLength + 15 * i));
                            TextRendering.Render($"Ammo: {spell.Ammo}",               Color.OrangeRed, new Vector2(obj.Position.X + 300, obj.Position.Y + summonerSpellLength + 15 * i));
                        }

                        var heroBuffs = heroUnit.GetActiveBuffs().ToArray();
                        if (heroBuffs.Length > 0)
                        {
                            TextRendering.Render("Buffs:", Color.OrangeRed, new Vector2(obj.Position.X, obj.Position.Y + 280));
                            TextRendering.Render("------", Color.OrangeRed, new Vector2(obj.Position.X, obj.Position.Y + 290));

                            for (var i = 0; i < heroBuffs.Length; i++)
                            {
                                TextRendering.Render($"{heroUnit.GetBuffCount(heroBuffs[i].Name)}x {heroBuffs[i].Name}", Color.OrangeRed, new Vector2(obj.Position.X, obj.Position.Y + 300 + 12 * i));
                            }
                        }
                        break;

                    case GameObjectTypeID.MissileClient:
                        var missileUnit = (MissileClient)obj;
                        TextRendering.Render("Missile Speed: " + missileUnit.SpellData.MissileSpeed, Color.OrangeRed, new Vector2(obj.Position.X, obj.Position.Y + 50));
                        break;

                    default:
                        TextRendering.Render("Name: "         + obj.Name,       Color.OrangeRed, new Vector2(obj.Position.X,       obj.Position.Y + 20));
                        TextRendering.Render("Type: "         + obj.Type,       Color.OrangeRed, new Vector2(obj.Position.X,       obj.Position.Y + 50));
                        TextRendering.Render("NetworkID: "    + obj.NetworkID,  Color.OrangeRed, new Vector2(obj.Position.X,       obj.Position.Y + 65));
                        TextRendering.Render("X: "            + obj.Position.X, Color.OrangeRed, new Vector2(obj.Position.X,       obj.Position.Y + 80));
                        TextRendering.Render("Y: "            + obj.Position.Y, Color.OrangeRed, new Vector2(obj.Position.X + 100, obj.Position.Y + 80));

                        if (obj.Type.TypeID == GameObjectTypeID.AIMinionClient)
                        {
                            var minionUnit = (AIMinionClient)obj;
                            TextRendering.Render("ModelName: " + minionUnit.ModelName, Color.White, new Vector2(obj.Position.X, obj.Position.Y + 35));

                            var minionBuffs = minionUnit.GetActiveBuffs().ToArray();
                            if (minionBuffs.Length > 0)
                            {
                                TextRendering.Render("Buffs:", Color.Yellow, new Vector2(obj.Position.X, obj.Position.Y + 110));
                                TextRendering.Render("------", Color.Yellow, new Vector2(obj.Position.X, obj.Position.Y + 120));

                                for (var i = 0; i < minionBuffs.Length; i++)
                                {
                                    TextRendering.Render($"{minionUnit.GetBuffCount(minionBuffs[i].Name)}x {minionBuffs[i].Name}", Color.White, new Vector2(obj.Position.X, obj.Position.Y + 130 + 12 * i));
                                }
                            }
                            break;
                        }

                        if (!Menu["printobjects"].As<MenuBool>().Enabled)
                        {
                            return;
                        }

                        if (obj.Type.TypeID == GameObjectTypeID.obj_GeneralParticleEmitter)
                        {
                            Console.WriteLine("Name: " + obj.Name);
                        }
                        break;
                }
            }
        }

        #endregion
    }
}