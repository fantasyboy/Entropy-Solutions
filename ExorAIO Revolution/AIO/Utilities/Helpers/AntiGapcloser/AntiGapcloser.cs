using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.UI;
using Entropy.SDK.Utils;
using SharpDX;

namespace AIO.Utilities
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Entropy;

	public static class Gapcloser
	{
		#region Static Fields

		public static Menu Menu;

		public static bool Initialized;

		public static readonly List<SpellData> Spells = new List<SpellData>();

		// ReSharper disable once InconsistentNaming
		private static readonly Dictionary<uint, GapcloserArgs> Gapclosers = new Dictionary<uint, GapcloserArgs>();

		#endregion

		#region Delegates

		public delegate void OnGapcloserEvent(AIHeroClient target, GapcloserArgs args);

		#endregion

		#region Events

		public static event OnGapcloserEvent OnGapcloser;

		#endregion

		#region Constructors and Destructors

		static Gapcloser()
		{
			Initialize();
		}

		#endregion

		#region Methods

		private static void Initialize()
		{
			InitSpells();

			Game.OnUpdate += OnUpdate;
			AIBaseClient.OnProcessSpellCast += OnProcessSpellCast;
		}

		private static void InitSpells()
		{
			#region Aatrox

			Spells.Add(
				new SpellData
				{
					ChampionName = "Aatrox",
					Slot = SpellSlot.Q,
					SpellName = "aatroxq",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Ahri

			Spells.Add(
				new SpellData
				{
					ChampionName = "Ahri",
					Slot = SpellSlot.R,
					SpellName = "ahritumble",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Akali

			Spells.Add(
				new SpellData
				{
					ChampionName = "Akali",
					Slot = SpellSlot.R,
					SpellName = "akalishadowdance",
					SpellType = Type.Targeted
				});

			#endregion

			#region Alistar

			Spells.Add(
				new SpellData
				{
					ChampionName = "Alistar",
					Slot = SpellSlot.W,
					SpellName = "headbutt",
					SpellType = Type.Targeted
				});

			#endregion

			#region Azir

			Spells.Add(
				new SpellData
				{
					ChampionName = "Azir",
					Slot = SpellSlot.E,
					SpellName = "azire",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Caitlyn

			Spells.Add(
				new SpellData
				{
					ChampionName = "Caitlyn",
					Slot = SpellSlot.E,
					IsReversedDash = true,
					Range = 400,
					SpellName = "caitlynentrapment",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Camille

			Spells.Add(
				new SpellData
				{
					ChampionName = "Camille",
					Slot = SpellSlot.E,
					SpellName = "camillee",
					SpellType = Type.SkillShot
				});

			Spells.Add(
				new SpellData
				{
					ChampionName = "Camille",
					Slot = SpellSlot.E,
					SpellName = "camilleedash2",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Corki

			Spells.Add(
				new SpellData
				{
					ChampionName = "Corki",
					Slot = SpellSlot.W,
					SpellName = "carpetbomb",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Diana

			Spells.Add(
				new SpellData
				{
					ChampionName = "Diana",
					Slot = SpellSlot.R,
					SpellName = "dianateleport",
					SpellType = Type.Targeted
				});

			#endregion

			#region Ekko

			Spells.Add(
				new SpellData
				{
					ChampionName = "Ekko",
					Slot = SpellSlot.E,
					SpellName = "ekkoeattack",
					SpellType = Type.Targeted
				});

			#endregion

			#region Elise

			Spells.Add(
				new SpellData
				{
					ChampionName = "Elise",
					Slot = SpellSlot.Q,
					SpellName = "elisespiderqcast",
					SpellType = Type.SkillShot
				});

			Spells.Add(
				new SpellData
				{
					ChampionName = "Elise",
					Slot = SpellSlot.E,
					SpellName = "elisespideredescent",
					SpellType = Type.Targeted
				});

			#endregion

			#region Ezreal

			Spells.Add(
				new SpellData
				{
					ChampionName = "Ezreal",
					Slot = SpellSlot.E,
					SpellName = "ezrealarcaneshift",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Fiora

			Spells.Add(
				new SpellData
				{
					ChampionName = "Fiora",
					Slot = SpellSlot.Q,
					SpellName = "fioraq",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Fizz

			Spells.Add(
				new SpellData
				{
					ChampionName = "Fizz",
					Slot = SpellSlot.Q,
					SpellName = "fizzpiercingstrike",
					SpellType = Type.Targeted
				});

			#endregion

			#region Galio

			Spells.Add(
				new SpellData
				{
					ChampionName = "Galio",
					Slot = SpellSlot.E,
					SpellName = "galioe",
					SpellType = Type.SkillShot,
					Delay = 500
				});

			#endregion

			#region Gnar

			Spells.Add(
				new SpellData
				{
					ChampionName = "Gnar",
					Slot = SpellSlot.E,
					SpellName = "gnare",
					SpellType = Type.SkillShot
				});

			Spells.Add(
				new SpellData
				{
					ChampionName = "Gnar",
					Slot = SpellSlot.E,
					SpellName = "gnarbige",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Gragas

			Spells.Add(
				new SpellData
				{
					ChampionName = "Gragas",
					Slot = SpellSlot.E,
					SpellName = "gragase",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Graves

			Spells.Add(
				new SpellData
				{
					ChampionName = "Graves",
					Slot = SpellSlot.E,
					SpellName = "gravesmove",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Hecarim

			Spells.Add(
				new SpellData
				{
					ChampionName = "Hecarim",
					Slot = SpellSlot.R,
					SpellName = "hecarimult",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Illaoi

			Spells.Add(
				new SpellData
				{
					ChampionName = "Illaoi",
					Slot = SpellSlot.W,
					SpellName = "illaoiwattack",
					SpellType = Type.Targeted
				});

			#endregion

			#region Irelia

			Spells.Add(
				new SpellData
				{
					ChampionName = "Irelia",
					Slot = SpellSlot.Q,
					SpellName = "ireliagatotsu",
					SpellType = Type.Targeted
				});

			#endregion

			/*
			#region JarvanIV

			Spells.Add(
			    new SpellData
			    {
			        ChampionName = "JarvanIV",
			        Slot = SpellSlot.Q,
			        SpellName = "jarvanivdragonstrike",
			        SpellType = Type.SkillShot
			    });

			Spells.Add(
			    new SpellData
			    {
			        ChampionName = "JarvanIV",
			        Slot = SpellSlot.R,
			        SpellName = "jarvanivcataclysm",
			        SpellType = Type.SkillShot
			    });

			#endregion
			*/

			#region Jax

			Spells.Add(
				new SpellData
				{
					ChampionName = "Jax",
					Slot = SpellSlot.Q,
					SpellName = "jaxleapstrike",
					SpellType = Type.Targeted
				});

			#endregion

			#region Jayce

			Spells.Add(
				new SpellData
				{
					ChampionName = "Jayce",
					Slot = SpellSlot.Q,
					SpellName = "jaycetotheskies",
					SpellType = Type.Targeted
				});

			#endregion

			#region Kassadin

			Spells.Add(
				new SpellData
				{
					ChampionName = "Kassadin",
					Slot = SpellSlot.R,
					SpellName = "riftwalk",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Katarina

			Spells.Add(
				new SpellData
				{
					ChampionName = "Katarina",
					Slot = SpellSlot.E,
					SpellName = "katarinae",
					SpellType = Type.Targeted
				});

			#endregion

			#region Kayn

			Spells.Add(
				new SpellData
				{
					ChampionName = "Kayn",
					Slot = SpellSlot.Q,
					SpellName = "kaynq",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Khazix

			Spells.Add(
				new SpellData
				{
					ChampionName = "Khazix",
					Slot = SpellSlot.E,
					SpellName = "khazixe",
					SpellType = Type.SkillShot
				});

			Spells.Add(
				new SpellData
				{
					ChampionName = "Khazix",
					Slot = SpellSlot.E,
					SpellName = "khazixelong",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Kindred

			Spells.Add(
				new SpellData
				{
					ChampionName = "Kindred",
					Slot = SpellSlot.Q,
					SpellName = "kindredq",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Leblanc

			Spells.Add(
				new SpellData
				{
					ChampionName = "Leblanc",
					Slot = SpellSlot.W,
					SpellName = "leblancslide",
					SpellType = Type.SkillShot
				});

			Spells.Add(
				new SpellData
				{
					ChampionName = "Leblanc",
					Slot = SpellSlot.W,
					SpellName = "leblancslidem",
					SpellType = Type.SkillShot
				});

			#endregion

			#region LeeSin

			Spells.Add(
				new SpellData
				{
					ChampionName = "LeeSin",
					Slot = SpellSlot.Q,
					SpellName = "blindmonkqtwo",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Lucian

			Spells.Add(
				new SpellData
				{
					ChampionName = "Lucian",
					Slot = SpellSlot.E,
					SpellName = "luciane",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Malphite

			Spells.Add(
				new SpellData
				{
					ChampionName = "Malphite",
					Slot = SpellSlot.R,
					SpellName = "ufslash",
					SpellType = Type.SkillShot
				});

			#endregion

			/*
			#region MasterYi

			Spells.Add(
			    new SpellData
			    {
			        ChampionName = "MasterYi",
			        Slot = SpellSlot.Q,
			        SpellName = "alphastrike",
			        SpellType = Type.Targeted
			    });

			#endregion
			*/

			#region MonkeyKing

			Spells.Add(
				new SpellData
				{
					ChampionName = "MonkeyKing",
					Slot = SpellSlot.E,
					SpellName = "monkeykingnimbus",
					SpellType = Type.Targeted
				});

			#endregion

			/*
			#region Nautilus

			Spells.Add(
			    new SpellData
			    {
			        ChampionName = "Nautilus",
			        Slot = SpellSlot.Q,
			        SpellName = "nautilusq",
			        SpellType = Type.SkillShot
			    });

			#endregion
			*/

			#region Nidalee

			Spells.Add(
				new SpellData
				{
					ChampionName = "Nidalee",
					Slot = SpellSlot.W,
					SpellName = "pounce",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Ornn

			Spells.Add(
				new SpellData
				{
					ChampionName = "Ornn",
					Slot = SpellSlot.E,
					SpellName = "ornne",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Pantheon

			Spells.Add(
				new SpellData
				{
					ChampionName = "Pantheon",
					Slot = SpellSlot.W,
					SpellName = "pantheonw",
					SpellType = Type.Targeted
				});

			#endregion

			#region Poppy

			Spells.Add(
				new SpellData
				{
					ChampionName = "Poppy",
					Slot = SpellSlot.E,
					SpellName = "poppyheroiccharge",
					SpellType = Type.Targeted
				});

			#endregion

			#region Quinn

			Spells.Add(
				new SpellData
				{
					ChampionName = "Quinn",
					Slot = SpellSlot.E,
					SpellName = "quinne",
					SpellType = Type.Targeted
				});

			#endregion

			#region Rakan

			Spells.Add(
				new SpellData
				{
					ChampionName = "Rakan",
					Slot = SpellSlot.W,
					SpellName = "rakanw",
					SpellType = Type.SkillShot
				});

			#endregion

			#region RekSai

			Spells.Add(
				new SpellData
				{
					ChampionName = "RekSai",
					Slot = SpellSlot.E,
					SpellName = "reksaieburrowed",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Renekton

			Spells.Add(
				new SpellData
				{
					ChampionName = "Renekton",
					Slot = SpellSlot.E,
					SpellName = "renektonsliceanddice",
					SpellType = Type.SkillShot
				});

			Spells.Add(
				new SpellData
				{
					ChampionName = "Renekton",
					Slot = SpellSlot.E,
					SpellName = "renektondice",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Rengar

			Spells.Add(
				new SpellData
				{
					ChampionName = "Rengar",
					Slot = SpellSlot.Unknown,
					SpellName = "rengarpassivebuffdash",
					SpellType = Type.SkillShot
				});

			Spells.Add(
				new SpellData
				{
					ChampionName = "Rengar",
					Slot = SpellSlot.Unknown,
					SpellName = "rengarpassivebuffdashaadummy",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Riven

			Spells.Add(
				new SpellData
				{
					ChampionName = "Riven",
					Slot = SpellSlot.Q,
					SpellName = "riventricleave",
					SpellType = Type.SkillShot
				});

			Spells.Add(
				new SpellData
				{
					ChampionName = "Riven",
					Slot = SpellSlot.E,
					SpellName = "rivenfeint",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Sejuani

			Spells.Add(
				new SpellData
				{
					ChampionName = "Sejuani",
					Slot = SpellSlot.Q,
					SpellName = "sejuaniarcticassault",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Shen

			Spells.Add(
				new SpellData
				{
					ChampionName = "Shen",
					Slot = SpellSlot.E,
					SpellName = "shene",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Shyvana

			Spells.Add(
				new SpellData
				{
					ChampionName = "Shyvana",
					Slot = SpellSlot.R,
					SpellName = "shyvanatransformcast",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Talon

			Spells.Add(
				new SpellData
				{
					ChampionName = "Talon",
					Slot = SpellSlot.Q,
					SpellName = "talonqdashattack",
					SpellType = Type.Targeted
				});

			#endregion

			#region Tristana

			Spells.Add(
				new SpellData
				{
					ChampionName = "Tristana",
					Slot = SpellSlot.W,
					SpellName = "rocketjump",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Tryndamere

			Spells.Add(
				new SpellData
				{
					ChampionName = "Tryndamere",
					Slot = SpellSlot.E,
					SpellName = "slashcast",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Vi

			Spells.Add(
				new SpellData
				{
					ChampionName = "Vi",
					Slot = SpellSlot.Q,
					SpellName = "viq",
					SpellType = Type.SkillShot
				});

			#endregion

			#region Vayne

			Spells.Add(
				new SpellData
				{
					ChampionName = "Vayne",
					Slot = SpellSlot.Q,
					SpellName = "vaynetumble",
					SpellType = Type.SkillShot,
					Range = 300f
				});

			#endregion

			/*
			#region Warwick

			Spells.Add(
			    new SpellData
			    {
			        ChampionName = "Warwick",
			        Slot = SpellSlot.R,
			        SpellName = "warwickr",
			        SpellType = Type.SkillShot
			    });

			#endregion
			*/

			#region XinZhao

			Spells.Add(
				new SpellData
				{
					ChampionName = "XinZhao",
					Slot = SpellSlot.E,
					SpellName = "xenzhaosweep",
					SpellType = Type.Targeted
				});

			#endregion

			#region Yasuo

			Spells.Add(
				new SpellData
				{
					ChampionName = "Yasuo",
					Slot = SpellSlot.E,
					SpellName = "yasuodashwrapper",
					SpellType = Type.Targeted
				});

			#endregion

			#region Zac

			Spells.Add(
				new SpellData
				{
					ChampionName = "Zac",
					Slot = SpellSlot.E,
					SpellName = "zace",
					SpellType = Type.SkillShot
				});

			#endregion

			/*
			#region Zed

			Spells.Add(
			    new SpellData
			    {
			        ChampionName = "Zed",
			        Slot = SpellSlot.R,
			        SpellName = "zedr",
			        SpellType = Type.Targeted
			    });

			#endregion
			*/

			/*
			#region Ziggs

			Spells.Add(
			    new SpellData
			    {
			        ChampionName = "Ziggs",
			        Slot = SpellSlot.W,
			        SpellName = "ziggswtoggle",
			        SpellType = Type.SkillShot
			    });

			#endregion
			*/
		}

		private static void OnUpdate(EntropyEventArgs args)
		{
			if (OnGapcloser == null)
			{
				return;
			}

			foreach (var needToDeleteValue in Gapclosers.Where(x => Game.TickCount - x.Value.StartTick > 1250).ToList())
			{
				Gapclosers.Remove(needToDeleteValue.Key);
			}

			foreach (var gapArgs in Gapclosers.Where(x => x.Value.Unit.IsValidTargetEx(allyIsValidTargetEx: true)))
			{
				OnGapcloser(gapArgs.Value.Unit, gapArgs.Value);
			}
		}

		private static void OnProcessSpellCast(AIBaseClientCastEventArgs args)
		{
			var sender = args.Caster as AIHeroClient;
			if (sender == null || !sender.IsValidTargetEx(allyIsValidTargetEx: true) ||
			    sender.Type.TypeID != GameObjectTypeID.AIHeroClient)
			{
				return;
			}

			var argsName = args.SpellData.Name.ToLower();
			if (Spells.All(x => !string.Equals(x.SpellName, argsName, StringComparison.CurrentCultureIgnoreCase)))
			{
				return;
			}
			
			var spell = Spells.FirstOrDefault(e => e.SpellName == argsName);
			var unit = Gapclosers[sender.NetworkID];
			DelayAction.Queue(() =>
				{
					if (!Gapclosers.ContainsKey(sender.NetworkID))
					{
						Gapclosers.Add(sender.NetworkID, new GapcloserArgs());
					}

					unit.Unit = sender;
					unit.Slot = args.Slot;
					unit.Target = args.Target;
					unit.Type = args.Target != null ? Type.Targeted : Type.SkillShot;
					unit.SpellName = args.SpellData.Name;
					unit.StartPosition = args.StartPosition;

					if (Spells.Any(e => e.SpellName == argsName))
					{
						if (spell.IsReversedDash)
						{
							unit.EndPosition = args.StartPosition.Extend(args.EndPosition, -spell.Range);
						}
						else if (Math.Abs(spell.Range) > 0)
						{
							unit.EndPosition = args.StartPosition.Extend(args.EndPosition, spell.Range);
						}
						else
						{
							unit.EndPosition = args.EndPosition;
						}
					}
					else
					{
						unit.EndPosition = args.EndPosition;
					}

					if (unit.EndPosition.IsWall())
					{
						for (var i = 25; i < args.StartPosition.Distance(unit.EndPosition); i += 25)
						{
							var endPos = args.StartPosition.Extend(unit.EndPosition, i);
							if (endPos.IsWall())
							{
								unit.EndPosition = endPos;
							}
						}
					}
				}, spell.Delay);

			unit.StartTick = Game.TickCount;
		}

		#endregion

		public enum Type
		{
			SkillShot = 0,

			Targeted = 1
		}

		public struct SpellData
		{
			#region Public Properties

			public string ChampionName { get; set; }

			public string SpellName { get; set; }

			public SpellSlot Slot { get; set; }

			public Type SpellType { get; set; }

			public bool IsReversedDash { get; set; }

			public int Delay { get; set; }

			public float Range { get; set; }

			#endregion
		}

		public class GapcloserArgs
		{
			#region Properties

			internal AIHeroClient Unit { get; set; }

			#endregion

			#region Public Properties

			public SpellSlot Slot { get; set; }

			public AttackableUnit Target { get; set; }

			public string SpellName { get; set; }

			public Type Type { get; set; }

			public Vector3 StartPosition { get; set; }

			public Vector3 EndPosition { get; set; }

			public int StartTick { get; set; }

			public int EndTick { get; set; }

			public int DurationTick { get; set; }

			#endregion
		}
	}
}