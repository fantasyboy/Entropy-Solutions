using AIO.Utilities;
using Entropy;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Orbwalking.EventArgs;

#pragma warning disable 1587

namespace AIO.Champions
{
	/// <summary>
	///     The champion class.
	/// </summary>
	internal partial class Lucian
	{
		#region Constructors and Destructors

		/// <summary>
		///     Loads Lucian.
		/// </summary>
		public Lucian()
		{
			/// <summary>
			///     Initializes the menus.
			/// </summary>
			Menus();

			/// <summary>
			///     Initializes the spells.
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

			var enabledOption = MenuClass.Gapcloser["enabled"];
			if (enabledOption == null || !enabledOption.Enabled)
			{
				return;
			}

			if (sender == null || !sender.IsEnemy() || !sender.IsMelee)
			{
				return;
			}

			var spellOption = MenuClass.SubGapcloser[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"];
			if (spellOption == null || !spellOption.Enabled)
			{
				return;
			}

			/// <summary>
			///     The Anti-Gapcloser E.
			/// </summary>
			if (SpellClass.E.Ready)
			{
				switch (args.Type)
				{
					case Gapcloser.Type.Targeted:
						if (args.Target.IsMe())
						{
							var targetPos =
								UtilityClass.Player.Position.Extend(args.StartPosition, -SpellClass.E.Range);
							if (targetPos.IsUnderEnemyTurret())
							{
								return;
							}

							SpellClass.E.Cast(targetPos);
						}

						break;
					default:
						var targetPos2 = UtilityClass.Player.Position.Extend(args.EndPosition, -SpellClass.E.Range);
						if (targetPos2.IsUnderEnemyTurret())
						{
							return;
						}

						if (args.EndPosition.Distance(UtilityClass.Player.Position) <=
						    UtilityClass.Player.GetAutoAttackRange())
						{
							SpellClass.E.Cast(targetPos2);
						}

						break;
				}
			}
		}

		/// <summary>
		///     Called on do-cast.
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
					Laneclear(args);
					Jungleclear(args);
					Buildingclear(args);
					break;
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

			if (Orbwalker.IsWindingUp)
			{
				return;
			}

			/// <summary>
			///     Initializes the Automatic actions.
			/// </summary>
			Automatic(args);

			if (IsCulling())
			{
				return;
			}

			/// <summary>
			///     Initializes the orbwalkingmodes.
			/// </summary>
			switch (Orbwalker.Mode)
			{
				case OrbwalkingMode.Combo:
					Combo(args);
					break;

				case OrbwalkingMode.LaneClear:
					LaneClear(args);
					break;

				case OrbwalkingMode.Harass:
					Harass(args);
					break;
			}
		}

		#endregion
	}
}