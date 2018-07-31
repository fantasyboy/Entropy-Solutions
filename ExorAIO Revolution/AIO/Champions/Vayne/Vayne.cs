using System.Linq;
using Entropy;
using Entropy.SDK.Events;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Orbwalking.EventArgs;
using SharpDX;
using Gapcloser = AIO.Utilities.Gapcloser;

#pragma warning disable 1587

namespace AIO.Champions
{
	/// <summary>
	///     The champion class.
	/// </summary>
	internal partial class Vayne
	{
		#region Constructors and Destructors

		/// <summary>
		///     Loads Vayne.
		/// </summary>
		public Vayne()
		{
			/// <summary>
			///     Initializes the menus.
			/// </summary>
			Menus();

			/// <summary>
			///     Updates the spells.
			/// </summary>
			Spells();

			/// <summary>
			///     Initializes the methods.
			/// </summary>
			Methods();
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     Called on post attack.
		/// </summary>
		/// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
		public void OnPostAttack(OnPostAttackEventArgs args)
		{
			/// <summary>
			///     Initializes the orbwalkingmodes.
			/// </summary>
			switch (Orbwalker.Mode)
			{
				case OrbwalkingMode.Combo:
					Weaving(args);
					break;
				case OrbwalkingMode.LaneClear:
					Lasthit(args);
					Laneclear(args);
					Jungleclear(args);
					Buildingclear(args);
					break;
			}
		}

		/// <summary>
		///     Called on pre attack.
		/// </summary>
		/// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
		public void OnPreAttack(OnPreAttackEventArgs args)
		{
			if (!UtilityClass.Player.Position.IsUnderEnemyTurret() &&
			    UtilityClass.Player.HasBuff("vaynetumblefade"))
			{
				var invisibilityBuff = UtilityClass.Player.GetBuff("vaynetumblefade");
				if (MenuClass.Miscellaneous["stealthtime"].Enabled &&
				    invisibilityBuff.GetRemainingBuffTime() >
				    invisibilityBuff.ExpireTime - invisibilityBuff.StartTime -
				    MenuClass.Miscellaneous["stealthtime"].Value / 1000f)
				{
					args.Cancel = true;
				}

				if (UtilityClass.Player.HasBuff("summonerexhaust"))
				{
					args.Cancel = true;
				}

				if (MenuClass.Miscellaneous["stealthcheck"].Enabled &&
				    GameObjects.EnemyHeroes.Count(t =>
					    t.IsValidTargetEx(UtilityClass.Player.GetAutoAttackRange(t))) >=
				    MenuClass.Miscellaneous["stealthcheck"].Value)
				{
					args.Cancel = true;
				}
			}

			/// <summary>
			///     The Target Forcing Logic.
			/// </summary>
			if (MenuClass.Miscellaneous["focusw"].Enabled)
			{
				if (Orbwalker.Mode != OrbwalkingMode.Combo &&
				    Orbwalker.Mode != OrbwalkingMode.Harass)
				{
					return;
				}

				var forceTarget = Extensions.GetBestEnemyHeroesTargets().FirstOrDefault(t =>
					t.GetBuffCount("vaynesilvereddebuff") == 2 &&
					t.IsValidTargetEx(UtilityClass.Player.GetAutoAttackRange(t)));
				if (forceTarget != null)
				{
					args.Target = forceTarget;
				}
			}
		}

		/// <summary>
		///     Fired on an incoming dash.
		/// </summary>
		/// <param name="args">The <see cref="Dash.DashArgs" /> instance containing the event data.</param>
		public void OnDash(Dash.DashArgs args)
		{
			var heroSender = args.Sender as AIHeroClient;
			if (heroSender == null || !heroSender.IsEnemy() ||
			    Invulnerable.Check(heroSender, DamageType.Magical, false))
			{
				return;
			}

			if (heroSender.CharName.Equals("Kalista"))
			{
				return;
			}

			var endPos = args.EndPosition;
			var playerPos = UtilityClass.Player.Position;

			if (!heroSender.IsValidTargetEx(SpellClass.E.Range) &&
			    endPos.Distance((Vector2) playerPos) > SpellClass.E.Range)
			{
				return;
			}

			if (SpellClass.E.Ready &&
			    MenuClass.E["emode"].Value != 2 &&
			    MenuClass.E["whitelist"][heroSender.CharName.ToLower()].Enabled)
			{
				const int condemnPushDistance = 410;
				for (var i = UtilityClass.Player.BoundingRadius; i < condemnPushDistance; i += 10)
				{
					if (!endPos.Extend((Vector2) playerPos, -i).IsWall())
					{
						continue;
					}

					SpellClass.E.CastOnUnit(heroSender);
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
			///     The Interrupter E.
			/// </summary>
			if (SpellClass.E.Ready &&
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

				if (heroSender.IsValidTargetEx(SpellClass.E.Range))
				{
					SpellClass.E.CastOnUnit(heroSender);
				}
			}
		}

		/// <summary>
		///     Fired on an incoming gapcloser.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="Gapcloser.GapcloserArgs" /> instance containing the event data.</param>
		public void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserArgs args)
		{
			if (UtilityClass.Player.IsDead)
			{
				return;
			}

			if (sender == null || !sender.IsEnemy() || !sender.IsMelee)
			{
				return;
			}

			/// <summary>
			///     The Anti-Gapcloser Q.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    MenuClass.Gapcloser["enabled"].Enabled &&
			    MenuClass.SubGapcloser[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"].Enabled)
			{
				switch (args.Type)
				{
					case Gapcloser.Type.Targeted:
						if (sender.IsMelee &&
							args.Target.IsMe())
						{
							Vector3 targetPos =
								UtilityClass.Player.Position.Extend(args.StartPosition, -SpellClass.Q.Range);
							if (targetPos.IsUnderEnemyTurret())
							{
								return;
							}

							SpellClass.Q.Cast(targetPos);
						}

						break;
					default:
						Vector3 targetPos2 = UtilityClass.Player.Position.Extend(args.EndPosition, -SpellClass.Q.Range);
						if (targetPos2.IsUnderEnemyTurret())
						{
							return;
						}

						if (args.EndPosition.DistanceToPlayer() <= UtilityClass.Player.GetAutoAttackRange())
						{
							SpellClass.Q.Cast(targetPos2);
						}

						break;
				}
			}

			/// <summary>
			///     The Anti-Gapcloser E.
			/// </summary>
			if (SpellClass.E.Ready &&
			    MenuClass.Gapcloser2["enabled"].Enabled &&
			    MenuClass.SubGapcloser2[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"].Enabled &&
			    !Invulnerable.Check(sender, DamageType.Magical, false))
			{
				switch (args.Type)
				{
					case Gapcloser.Type.Targeted:
						if (args.Target.IsMe())
						{
							SpellClass.E.CastOnUnit(sender);
						}

						break;
					default:
						if (args.EndPosition.Distance((Vector2) UtilityClass.Player.Position) <=
						    UtilityClass.Player.GetAutoAttackRange())
						{
							SpellClass.E.CastOnUnit(sender);
						}

						break;
				}
			}
		}

		/// <summary>
		///     Fired when the game is updated.
		/// </summary>
		public void OnTick(EntropyEventArgs args)
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
			}
		}

		#endregion
	}
}