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
	internal partial class Kaisa
	{
		#region Constructors and Destructors

		/// <summary>
		///     Loads Kai'Sa.
		/// </summary>
		public Kaisa()
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
					Jungleclear(args);
					break;
			}
		}

		/// <summary>
		///     Called on pre attack.
		/// </summary>
		/// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
		public void OnPreAttack(OnPreAttackEventArgs args)
		{
			/// <summary>
			///     The Target Forcing Logic.
			/// </summary>
			if (MenuClass.Miscellaneous["focusmark"].Enabled)
			{
				if (Orbwalker.Mode != OrbwalkingMode.Combo &&
				    Orbwalker.Mode != OrbwalkingMode.Harass)
				{
					return;
				}

				var forceTarget = Extensions.GetBestEnemyHeroesTargets().FirstOrDefault(t =>
					t.HasBuff("kaisapassivemarker") &&
					t.IsValidTargetEx(UtilityClass.Player.GetAutoAttackRange(t)));
				if (forceTarget != null)
				{
					args.Target = forceTarget;
				}
			}

			var heroTarget = args.Target as AIHeroClient;
			if (heroTarget == null)
			{
				return;
			}

			/// <summary>
			///     The W Combo Logic.
			/// </summary>
			if (SpellClass.W.Ready &&
			    MenuClass.W["combo"].Enabled &&
			    heroTarget.GetBuffCount("kaisapassivemarker") >= MenuClass.W["combo"].Value)
			{
				SpellClass.W.Cast(heroTarget);
			}
		}

		private static void OnTeleport(Teleports.TeleportEventArgs args)
		{
			if (args.Type != TeleportType.Recall || args.Status != TeleportStatus.Start)
			{
				return;
			}

			var sender = args.Sender;
			if (SpellClass.W.Ready &&
			    MenuClass.W["teleports"].Enabled &&
			    sender.DistanceToPlayer() <= SpellClass.W.Range)
			{
				SpellClass.W.Cast(sender.Position);
			}
		}

		private static void OnLevelUp(AIBaseClientLevelUpEventArgs args)
		{
			if (args.Owner.IsMe())
			{
				switch (args.ToLevel)
				{
					case 6:
						SpellClass.R.Range = 1500;
						break;
					case 11:
						SpellClass.R.Range = 2000;
						break;
					case 16:
						SpellClass.R.Range = 2500;
						break;
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
			///     The Anti-Gapcloser E.
			/// </summary>
			if (SpellClass.E.Ready &&
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
							SpellClass.E.Cast();
						}
						break;

					default:
						if (args.EndPosition.Distance((Vector2) UtilityClass.Player.Position) <=
						    UtilityClass.Player.GetAutoAttackRange())
						{
							SpellClass.E.Cast();
						}
						break;
				}
			}
		}

		public void OnNonKillableMinion(OnNonKillableMinionEventArgs args)
		{
			if (SpellClass.Q.Ready &&
			    args.Target.DistanceToPlayer() < UtilityClass.Player.GetAutoAttackRange() &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["nonkillable"]) &&
			    MenuClass.Q["nonkillable"].Enabled)
			{
				SpellClass.Q.Cast();
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
				case OrbwalkingMode.LaneClear:
					Laneclear(args);
					break;
			}
		}

		#endregion
	}
}