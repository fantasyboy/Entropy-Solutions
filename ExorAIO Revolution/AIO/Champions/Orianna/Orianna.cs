
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;
using SharpDX;

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
        
        /// <param name="args">The <see cref="SpellBookCastSpellEventArgs" /> instance containing the event data.</param>
        public void OnCastSpell(SpellbookLocalCastSpellEventArgs args)
        {
            if (!MenuClass.Miscellaneous["blockr"].As<MenuBool>().Enabled)
            {
                return;
            }

            if (sender.IsMe() &&
                BallPosition != null &&
                args.Slot == SpellSlot.R)
            {
                var validTargets = GameObjects.EnemyHeroes.Where(t =>
                        !Invulnerable.Check(t, DamageType.Magical, false) &&
                        t.IsValidTarget(SpellClass.R.Width - t.BoundingRadius - SpellClass.R.Delay * t.BoundingRadius, false, false, (Vector3)BallPosition));
                if (!validTargets.Any())
                {
                    args.Process = false;
                }
            }
        }

        /// <summary>
        ///     Fired on present.
        /// </summary>
        public void OnPresent()
        {
            /// <summary>
            ///     Initializes the drawings.
            /// </summary>
            Drawings();
        }

        /// <summary>
        ///     Fired on an incoming gapcloser.
        /// </summary>
        
        /// <param name="args">The <see cref="Gapcloser.GapcloserArgs" /> instance containing the event data.</param>
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
                if (sender.IsEnemy()() && sender.IsMelee)
                {
                    var enabledOption = MenuClass.Gapcloser["enabled"];
                    if (enabledOption == null || !enabledOption.As<MenuBool>().Enabled)
                    {
                        return;
                    }

                    var spellOption = MenuClass.SubGapcloser[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"];
                    if (spellOption == null || !spellOption.As<MenuBool>().Enabled)
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
                                        a.IsValidTarget(SpellClass.E.Range, true) &&
                                        args.EndPosition.Distance(a) <= a.AttackRange / 2)
                                    .MinBy(o => o.MaxHP);

                                if (bestAlly != null)
                                {
                                    SpellClass.E.CastOnUnit(bestAlly);
                                }
                            }
                            break;
                    }
                }
                else if (sender.IsAlly)
                {
                    if (MenuClass.Spells["r"]["aoe"] == null ||
                        !MenuClass.Spells["r"]["aoe"].As<MenuSliderBool>().Enabled)
                    {
                        return;
                    }

                    /// <summary>
                    ///     The E Engager Logic.
                    /// </summary>
                    if (sender.IsValidTarget(SpellClass.E.Range, true) &&
                        MenuClass.Spells["e"]["engager"].As<MenuBool>().Enabled)
                    {
                        if (GameObjects.EnemyHeroes.Count(t =>
                                !Invulnerable.Check(t, DamageType.Magical, false) &&
                                t.IsValidTarget(SpellClass.R.Width - SpellClass.R.Delay * t.BoundingRadius, false, false, args.EndPosition)) >= MenuClass.Spells["r"]["aoe"].As<MenuSliderBool>().Value &&
                            MenuClass.Spells["e"]["engagerswhitelist"][sender.CharName.ToLower()].As<MenuBool>().Enabled)
                        {
                            SpellClass.E.CastOnUnit(sender);
                        }
                    }
                }
            }
        }

        /*
        /// <summary>
        ///     Called on interruptable spell.
        /// </summary>
        
        /// <param name="args">The <see cref="Events.InterruptableTargetEventArgs" /> instance containing the event data.</param>
        public void OnInterruptableTarget(Events.InterruptableTargetEventArgs args)
        {
            if (UtilityClass.Player.IsDead || Invulnerable.Check(args.Sender, DamageType.Magical, false))
            {
                return;
            }

            if (SpellClass.R.State == SpellState.Ready && ((Vector2)GetBallPosition).Distance(args.Sender.Position) < SpellClass.R.SpellData.Range
                && MenuClass.Spells["r"]["interrupter"].As<MenuBool>().Enabled)
            {
                UtilityClass.Player.Spellbook.CastSpell(SpellSlot.R);
            }
        }
        */

        /// <summary>
        ///     Called on process spell cast;
        /// </summary>
        
        /// <param name="args">The <see cref="AIBaseClientMissileClientDataEventArgs" /> instance containing the event data.</param>
        public void OnProcessSpellCast(AIBaseClient sender, AIBaseClientMissileClientDataEventArgs args)
        {
            var target = args.Target as AIHeroClient;
            if (target == null ||
                !Extensions.GetAllyHeroesTargetsInRange(SpellClass.E.Range).Contains(target))
            {
                return;
            }

            if (SpellClass.E.Ready &&
                Bools.ShouldShieldAgainstSender(sender) &&
                MenuClass.Spells["e"]["protect"].As<MenuBool>().Enabled &&
                MenuClass.Spells["e"]["protectwhitelist"][target.CharName.ToLower()].As<MenuSliderBool>().Enabled &&
                target.HPPercent() <= MenuClass.Spells["e"]["protectwhitelist"][target.CharName.ToLower()].As<MenuSliderBool>().Value)
            {
                SpellClass.E.CastOnUnit(target);
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
            ///     Updates the position of the ball.
            /// </summary>
            UpdateBallPosition();

            /// <summary>
            ///     Updates the drawing position of the ball.
            /// </summary>
            UpdateDrawingBallPosition();

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
            switch (ImplementationClass.IOrbwalker.Mode)
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