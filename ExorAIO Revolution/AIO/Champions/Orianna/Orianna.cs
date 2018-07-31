
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Events;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Gapcloser = AIO.Utilities.Gapcloser;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Orianna
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Orianna.
        /// </summary>
        public Orianna()
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
		///     Fired on spell cast.
		/// </summary>
		/// <param name="args">The <see cref="SpellbookLocalCastSpellEventArgs" /> instance containing the event data.</param>
		public void OnLocalCastSpell(SpellbookLocalCastSpellEventArgs args)
        {
            if (!MenuClass.Miscellaneous["blockr"].Enabled)
            {
                return;
            }

	        switch (args.Slot)
	        {
				case SpellSlot.Q:
					LastECastTime = 0;
					break;

		        case SpellSlot.E:
			        LastECastTime = Game.TickCount;
			        break;

				case SpellSlot.W:
				case SpellSlot.R:
					if (Game.TickCount - LastECastTime < 1000)
					{
						args.Execute = false;
					}
					break;
	        }

            if (GetBall() != null &&
                args.Slot == SpellSlot.R)
            {
                var validTargets = GameObjects.EnemyHeroes.Where(t =>
                        !Invulnerable.Check(t, DamageType.Magical, false) &&
                        t.IsValidTargetEx(SpellClass.R.Width - t.BoundingRadius - SpellClass.R.Delay * t.BoundingRadius, false, false, GetBall().Position));
                if (!validTargets.Any())
                {
                    args.Execute = false;
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
            
            if (sender == null)
            {
                return;
            }

            /// <summary>
            ///     The On-Dash E Logics.
            /// </summary>
            if (SpellClass.E.Ready)
            {
                /// <summary>
                ///     The Anti-Gapcloser E.
                /// </summary>
                if (sender.IsEnemy() && sender.IsMelee)
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
                            if (args.Target.IsMe())
                            {
                                SpellClass.E.CastOnUnit(UtilityClass.Player);
                            }
                            break;
                        default:
                            if (args.EndPosition.Distance(UtilityClass.Player.Position) <= UtilityClass.Player.GetAutoAttackRange())
                            {
                                SpellClass.E.CastOnUnit(UtilityClass.Player);
                            }
                            else
                            {
                                var bestAlly = GameObjects.AllyHeroes
                                    .Where(a =>
                                        !a.IsMe() &&
                                        a.IsValidTargetEx(SpellClass.E.Range, true) &&
                                        args.EndPosition.Distance(a) <= a.GetAutoAttackRange() / 2)
                                    .MinBy(o => o.MaxHP);

                                if (bestAlly != null)
                                {
                                    SpellClass.E.CastOnUnit(bestAlly);
                                }
                            }
                            break;
                    }
                }
                else if (sender.IsAlly())
                {
                    if (GameObjects.EnemyHeroes.Count() < 2)
                    {
                        return;
                    }

                    /// <summary>
                    ///     The E Engager Logic.
                    /// </summary>
                    if (sender.IsValidTargetEx(SpellClass.E.Range, true) &&
                        MenuClass.R["aoe"].Enabled &&
						MenuClass.E["engager"].Enabled)
                    {
                        if (GameObjects.EnemyHeroes.Count(t =>
                                !Invulnerable.Check(t, DamageType.Magical, false) &&
                                t.IsValidTargetEx(SpellClass.R.Width - SpellClass.R.Delay * t.BoundingRadius, false, false, args.EndPosition)) >= MenuClass.R["aoe"].Value &&
                            MenuClass.E["engagerswhitelist"][sender.CharName.ToLower()].Enabled)
                        {
                            SpellClass.E.CastOnUnit(sender);
                        }
                    }
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
		    ///     The Interrupter R.
		    /// </summary>
		    if (SpellClass.R.Ready &&
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

			    if (heroSender.IsValidTargetEx(SpellClass.R.Width, false, false, GetBall().Position))
			    {
				    SpellClass.R.Cast();
			    }
		    }
	    }

		/// <summary>
		///     Called on process spell cast;
		/// </summary>
		/// <param name="args">The <see cref="AIBaseClientCastEventArgs" /> instance containing the event data.</param>
		public void OnProcessSpellCast(AIBaseClientCastEventArgs args)
        {
            var target = args.Target as AIHeroClient;
            if (target == null ||
                !Extensions.GetAllyHeroesTargetsInRange(SpellClass.E.Range).Contains(target))
            {
                return;
            }

            if (SpellClass.E.Ready &&
                Bools.ShouldShieldAgainstSender(args.Caster) &&
                MenuClass.E["protect"].Enabled &&
                MenuClass.E["protectwhitelist"][target.CharName.ToLower()].Enabled &&
                target.HPPercent() <= MenuClass.E["protectwhitelist"][target.CharName.ToLower()].Value)
            {
                SpellClass.E.CastOnUnit(target);
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
            ///     Initializes the Automatic actions.
            /// </summary>
            Automatic(args);

            /// <summary>
            ///     Initializes the Killsteal events.
            /// </summary>
            Killsteal(args);

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
                case OrbwalkingMode.LastHit:
                    LastHit(args);
                    break;
            }
        }

        #endregion
    }
}