
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Caching;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Events;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.Predictions.RecallPrediction;
using Entropy.SDK.Utils;
using Gapcloser = AIO.Utilities.Gapcloser;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Caitlyn
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Caitlyn.
        /// </summary>
        public Caitlyn()
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
        }

		#endregion

		#region Public Methods and Operators

	    private static void OnLevelUp(AIBaseClientLevelUpEventArgs args)
	    {
		    if (args.Owner.IsMe())
		    {
			    switch (args.ToLevel)
			    {
					case 6:
						SpellClass.R.Range = 2000;
						break;
				    case 11:
					    SpellClass.R.Range = 2500;
					    break;
				    case 16:
					    SpellClass.R.Range = 3000;
					    break;
			    }
		    }
	    }

	    private static void OnGainBuff(BuffManagerGainBuffEventArgs args)
	    {
		    if (!args.Sender.IsMe())
		    {
			    return;
		    }

			var heroTarget = args.BuffInstance.Owner;
		    if (heroTarget != null)
		    {
				var buff = args.BuffInstance;
			    if (buff.Name.ToLower() == "caitlynyordletrapinternal" &&
					heroTarget.DistanceToPlayer() >= 650 + UtilityClass.Player.BoundingRadius &&
			        heroTarget.IsValidTarget(1250))
			    {
					LocalPlayer.IssueOrder(HeroOrder.AttackUnit, heroTarget);
				    LocalPlayer.IssueOrder(HeroOrder.AttackUnit, heroTarget);
				    LocalPlayer.IssueOrder(HeroOrder.AttackUnit, heroTarget);
				    LocalPlayer.IssueOrder(HeroOrder.AttackUnit, heroTarget);
				}
			}
	    }

	    private static void OnTeleport(Teleports.TeleportEventArgs args)
	    {
		    if (args.Type != TeleportType.Recall || args.Status != TeleportStatus.Start)
		    {
			    return;
		    }

		    /// <summary>
		    ///     The Automatic W on Teleport Logic. 
		    /// </summary>
		    if (SpellClass.W.Ready &&
		        MenuClass.Spells["w"]["teleport"].Enabled)
		    {
			    DelayAction.Queue(() =>
				    {
					    var predictedPos = RecallPrediction.GetPrediction();
					    if (predictedPos.IsZero || predictedPos.DistanceToPlayer() > SpellClass.W.Range)
					    {
						    return;
					    }

					    SpellClass.W.Cast(predictedPos);
				    },
				    250); // <- Gotta let SDK run first
		    }
		}

		/// <summary>
		///     Fired on spell cast.
		/// </summary>
		/// <param name="args">The <see cref="SpellbookLocalCastSpellEventArgs" /> instance containing the event data.</param>
		public void OnLocalCastSpell(SpellbookLocalCastSpellEventArgs args)
        {
            switch (args.Slot)
            {
                case SpellSlot.Q:
                    var safeQ = MenuClass.Spells["q"]["customization"]["safeq"];
                    if (safeQ != null &&
                        UtilityClass.Player.EnemyHeroesCount(UtilityClass.Player.GetAutoAttackRange()) > safeQ.Value)
                    {
                        args.Execute = false;
                    }
                    break;

                case SpellSlot.W:
                    if (ObjectCache.AllGameObjects.Any(m => m.Distance(args.End) <= SpellClass.W.Width && m.Name.Equals("caitlyn_Base_yordleTrap_idle_green.troy")))
                    {
						args.Execute = false;
					}
                    break;

                case SpellSlot.E:
                    var safeE = MenuClass.Spells["e"]["customization"]["safee"];
                    if (safeE != null &&
                        UtilityClass.Player.Position.Extend(args.End, -400f).EnemyHeroesCount(UtilityClass.Player.GetAutoAttackRange()) > safeE.Value)
                    {
						args.Execute = false;
					}

                    if (Game.TickCount - UtilityClass.LastTick >= 1000 &&
                        Orbwalker.Mode == OrbwalkingMode.None &&
                        MenuClass.Miscellaneous["reversede"].Enabled)
                    {
                        UtilityClass.LastTick = Game.TickCount;
                        SpellClass.E.Cast(UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, -SpellClass.E.Range));
                    }
                    break;
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
                    Jungleclear(args);
                    break;
            }
        }

		/// <summary>
		///     Called on do-cast.
		/// </summary>
		/// <param name="args">The <see cref="AIBaseClientCastEventArgs" /> instance containing the event data.</param>
		public void OnProcessSpellCast(AIBaseClientCastEventArgs args)
        {
            if (args.Caster.IsMe())
            {
                /// <summary>
                ///     Initializes the orbwalkingmodes.
                /// </summary>
                switch (Orbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                        switch (args.SpellData.Name)
                        {
                            //case "CaitlynEntrapment":
                            case "CaitlynEntrapmentMissile":
                                if (SpellClass.W.Ready &&
                                    MenuClass.Spells["w"]["triplecombo"].Enabled)
                                {
                                    var bestTarget = GameObjects.EnemyHeroes
                                        .Where(t => !Invulnerable.Check(t) && t.IsValidTarget(SpellClass.W.Range))
                                        .MinBy(o => o.Distance(args.EndPosition));
                                    if (bestTarget != null &&
                                        CanTrap(bestTarget))
                                    {
                                        SpellClass.W.Cast(UtilityClass.Player.Position.Extend(bestTarget.Position, UtilityClass.Player.Distance(bestTarget) + bestTarget.BoundingRadius));
                                        //UpdateEnemyTrapTime(bestTarget.NetworkID);
                                    }
                                }
                                break;
                        }
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
            
            if (sender == null || !sender.IsEnemy() || Invulnerable.Check(sender))
            {
                return;
            }

            /// <summary>
            ///     The Anti-Gapcloser E.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Gapcloser2["enabled"].Enabled &&
                MenuClass.SubGapcloser2[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"].Enabled)
            {

                switch (args.Type)
                {
                    case Gapcloser.Type.Targeted:
                        if (sender.IsMelee &&
                            args.Target.IsMe())
                        {
                            var targetPos = UtilityClass.Player.Position.Extend(args.StartPosition, -400);
                            if (targetPos.IsUnderEnemyTurret())
                            {
                                return;
                            }

                            SpellClass.E.Cast(args.StartPosition);
                        }
                        break;
                    default:
                        var targetPos2 = UtilityClass.Player.Position.Extend(args.EndPosition, -400);
                        if (targetPos2.IsUnderEnemyTurret())
                        {
                            return;
                        }

                        if (args.EndPosition.Distance(UtilityClass.Player.Position) <= SpellClass.E.Range)
                        {
                            SpellClass.E.Cast(args.EndPosition);
                        }
                        break;
                }
            }

			/// <summary>
			///     The Anti-Gapcloser W.
			/// </summary>
			if (SpellClass.W.Ready &&
				!Invulnerable.Check(sender, DamageType.Magical, false) &&
			    MenuClass.Gapcloser["enabled"].Enabled &&
			    MenuClass.SubGapcloser[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"].Enabled &&
				args.EndPosition.Distance(UtilityClass.Player.Position) <= SpellClass.W.Range)
            {
				SpellClass.W.Cast(args.EndPosition);
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

            if (Orbwalker.IsWindingUp)
            {
                return;
            }

            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (Orbwalker.Mode)
            {
                case OrbwalkingMode.Harass:
                    Harass(args);
                    break;

                case OrbwalkingMode.LaneClear:
                    LaneClear(args);
                    break;
            }
        }

        #endregion
    }
}