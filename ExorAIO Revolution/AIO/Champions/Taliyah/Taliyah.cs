using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Events;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Utils;
using Gapcloser = AIO.Utilities.Gapcloser;

#pragma warning disable 1587

namespace AIO.Champions
{
	/// <summary>
	///     The champion class.
	/// </summary>
	internal partial class Taliyah
	{
		#region Constructors and Destructors

		/// <summary>
		///     Loads Taliyah.
		/// </summary>
		public Taliyah()
		{
			/// <summary>
			///     Initializes the menus.
			/// </summary>
			Menus();

			/// <summary>
			///     Initializes the methods.
			/// </summary>
			Methods();

			/// <summary>
			///     Initializes the spells.
			/// </summary>
			Spells();

			/// <summary>
			///     Reloads the MineField.
			/// </summary>
			ReloadMineField();

			/// <summary>
			///     Reloads the WorkedGrounds.
			/// </summary>
			ReloadWorkedGrounds();
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     Called on spell cast.
		/// </summary>
		/// <param name="args">The <see cref="SpellbookLocalCastSpellEventArgs" /> instance containing the event data.</param>
		public void OnLocalCastSpell(SpellbookLocalCastSpellEventArgs args)
		{
			switch (Orbwalker.Mode)
			{
				case OrbwalkingMode.Combo:
					switch (args.Slot)
					{
						case SpellSlot.W:
							if (SpellClass.E.Ready &&
							    UtilityClass.Player.MP <
							    SpellSlot.W.GetManaCost() +
							    SpellSlot.E.GetManaCost())
							{
								args.Execute = false;
							}
							else
							{
								LastWTime = Game.ClockTime;
							}

							break;
					}

					break;
			}
		}

		/// <summary>
		///     Fired upon GameObject creation.
		/// </summary>
		public void OnCreate(GameObjectCreateEventArgs args)
		{
			var obj = args.Sender;
			if (obj.IsValid)
			{
				switch (obj.Name)
				{
					case "Taliyah_Base_Q_aoe":
					case "Taliyah_Base_Q_aoe_river":
						WorkedGrounds.Add(obj, obj.Position);
						break;

					case "Taliyah_Base_E_Mines":
						MineField.Add(obj, obj.Position);
						break;
				}
			}
		}

		/// <summary>
		///     Fired upon GameObject creation.
		/// </summary>
		public void OnDelete(GameObjectDeleteEventArgs args)
		{
			var obj = args.Sender;
			if (obj.IsValid)
			{
				if (WorkedGrounds.Any(o => o.Key == obj))
				{
					WorkedGrounds.Remove(obj);
				}

				if (MineField.Any(o => o.Key == obj))
				{
					MineField.Remove(obj);
				}
			}
		}

		/// <summary>
		///     Fired on interruptable spells.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="Interrupter.InterruptableSpellEventArgs" /> instance containing the event data.</param>
		public void OnInterruptableSpell(AIBaseClient sender, Interrupter.InterruptableSpellEventArgs args)
		{
			if (UtilityClass.Player.IsDead)
			{
				return;
			}

			var heroSender = sender as AIHeroClient;
			if (heroSender == null || !heroSender.IsEnemy())
			{
				return;
			}

			/// <summary>
			///     The Interrupter W.
			/// </summary>
			if (SpellClass.W.Ready &&
			    !Invulnerable.Check(heroSender, DamageType.Magical, false))
			{
				var enabledOption = MenuClass.Interrupter["enabled"];
				if (enabledOption == null || !enabledOption.Enabled)
				{
					return;
				}

				var spellOption =
					MenuClass.SubInterrupter[$"{heroSender.CharName.ToLower()}.{args.Slot.ToString().ToLower()}"];
				if (spellOption == null || !spellOption.Enabled)
				{
					return;
				}

				if (heroSender.IsValidTargetEx(SpellClass.W.Range))
				{
					SpellClass.W.Cast(heroSender.Position, GetUnitPositionAfterPull(heroSender));
				}
			}
		}

		/// <summary>
		///     Fired on an incoming gapcloser.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="Utilities.Gapcloser.GapcloserArgs" /> instance containing the event data.</param>
		public void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserArgs args)
		{
			if (UtilityClass.Player.IsDead)
			{
				return;
			}

			if (sender == null || !sender.IsEnemy())
			{
				return;
			}

			/// <summary>
			///     The Anti-Gapcloser W Logic.
			/// </summary>
			if (SpellClass.W.Ready &&
			    !Invulnerable.Check(sender, DamageType.Magical, false))
			{
				var enabledOption = MenuClass.Gapcloser["enabled"];
				if (enabledOption == null || !enabledOption.Enabled)
				{
					return;
				}

				var spellOption = MenuClass.SubGapcloser[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"];
				if (spellOption == null || !spellOption.Enabled)
				{
					return;
				}

				switch (args.Type)
				{
					case Gapcloser.Type.Targeted:
						if (sender.IsMelee &&
						    args.Target.IsMe())
						{
							SpellClass.W.Cast(args.EndPosition, args.EndPosition.Extend(args.StartPosition, 200f));
						}

						break;
					default:
						if (args.EndPosition.DistanceToPlayer() <=
						    UtilityClass.Player.GetAutoAttackRange())
						{
							SpellClass.W.Cast(args.EndPosition,
								args.EndPosition.Extend(args.StartPosition, sender.IsMelee ? 200f : -200f));
						}

						break;
				}
			}

			/// <summary>
			///     The Anti-Gapcloser E Logic.
			/// </summary>
			if (SpellClass.E.Ready)
			{
				var enabledOption2 = MenuClass.Gapcloser2["enabled"];
				if (enabledOption2 == null || !enabledOption2.Enabled)
				{
					return;
				}

				var spellOption2 = MenuClass.SubGapcloser2[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"];
				if (spellOption2 == null || !spellOption2.Enabled)
				{
					return;
				}

				switch (args.Type)
				{
					case Gapcloser.Type.Targeted:
						if (sender.IsMelee &&
						    args.Target.DistanceToPlayer() <= SpellClass.E.Range)
						{
							SpellClass.E.Cast(UtilityClass.Player.Position.Extend(args.EndPosition, UtilityClass.Player.BoundingRadius));
						}

						break;
					default:
						if (args.EndPosition.DistanceToPlayer() <=
						    UtilityClass.Player.GetAutoAttackRange())
						{
							SpellClass.E.Cast(args.StartPosition);
						}

						break;
				}
			}
		}

		/// <summary>
		///     Called after processing spellcast operations.
		/// </summary>
		/// <param name="args">The <see cref="AIBaseClientCastEventArgs" /> instance containing the event data.</param>
		public void OnFinishCast(AIBaseClientCastEventArgs args)
		{
			if (args.Caster.IsMe())
			{
				switch (args.Slot)
				{
					case SpellSlot.W when MenuClass.Root["pattern"].Value != 1:
						SpellClass.E.Cast(args.EndPosition);
						break;

					/// <summary>
					///     Automatically Mount on R Logic.
					/// </summary>
					case SpellSlot.R:
						if (SpellClass.R.Ready &&
						    MenuClass.R["mountr"].Enabled)
						{
							DelayAction.Queue(() => { SpellClass.R.CastOnUnit(UtilityClass.Player); }, 500);
						}
						break;
				}
			}
		}

		private static void OnLevelUp(AIBaseClientLevelUpEventArgs args)
		{
			if (args.Owner.IsMe())
			{
				switch (args.ToLevel)
				{
					case 6:
						SpellClass.R.Range = 3000;
						break;
					case 11:
						SpellClass.R.Range = 4500;
						break;
					case 16:
						SpellClass.R.Range = 6000;
						break;
				}
			}
		}

		private static void OnTeleport(Teleports.TeleportEventArgs args)
		{
			if (args.Type != TeleportType.Recall || args.Status != TeleportStatus.Start)
			{
				return;
			}

			var sender = args.Sender;
			if (SpellClass.E.Ready &&
			    MenuClass.E["teleports"].Enabled &&
			    sender.DistanceToPlayer() <= SpellClass.E.Range)
			{
				SpellClass.E.Cast(sender.Position);
			}
		}

		/// <summary>
		///     Fired when the game is updated.
		/// </summary>
		public void OnUpdate(EntropyEventArgs args)
		{
			if (UtilityClass.Player.IsDead)
			{
				return;
			}

			/// <summary>
			///     Initializes the Killsteal events.
			/// </summary>
			Killsteal(args);

			/// <summary>
			///     Initializes the Automatic actions.
			/// </summary>
			Automatic(args);

			/// <summary>
			///     Initializes the orbwalkingmodes.
			/// </summary>
			switch (Orbwalker.Mode)
			{
				case OrbwalkingMode.Combo:
					Combo(args);
					break;
				case OrbwalkingMode.Harass:
					Harass(args);
					break;
				case OrbwalkingMode.LaneClear:
					LaneClear(args);
					JungleClear(args);
					break;
			}
		}

		#endregion
	}
}