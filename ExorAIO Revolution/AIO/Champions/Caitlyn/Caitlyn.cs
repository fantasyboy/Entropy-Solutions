
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using Entropy.SDK.Orbwalking;
using AIO.Utilities;

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

            /// <summary>
            ///     Initializes the trap time check for each enemy.
            /// </summary>
            InitializeTrapTimeCheck();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Fired on spell cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SpellBookCastSpellEventArgs" /> instance containing the event data.</param>
        public void OnCastSpell(Obj_AI_Base sender, SpellBookCastSpellEventArgs args)
        {
            if (sender.IsMe)
            {
                switch (args.Slot)
                {
                    case SpellSlot.Q:
                        var safeQ = MenuClass.Spells["q"]["customization"]["safeq"];
                        if (safeQ != null &&
                            UtilityClass.Player.CountEnemyHeroesInRange(UtilityClass.Player.AttackRange) > safeQ.As<MenuSlider>().Value)
                        {
                            args.Process = false;
                        }
                        break;

                    case SpellSlot.W:
                        if (ObjectManager.Get<GameObject>().Any(m => m.Distance(args.End) <= SpellClass.W.Width && m.Name.Equals("caitlyn_Base_yordleTrap_idle_green.troy")))
                        {
                            args.Process = false;
                        }
                        break;

                    case SpellSlot.E:
                        var safeE = MenuClass.Spells["e"]["customization"]["safee"];
                        if (safeE != null &&
                            UtilityClass.Player.ServerPosition.Extend(args.End, -400f).CountEnemyHeroesInRange(UtilityClass.Player.AttackRange) > safeE.As<MenuSlider>().Value)
                        {
                            args.Process = false;
                        }

                        if (Game.TickCount - UtilityClass.LastTick >= 1000 &&
                            ImplementationClass.IOrbwalker.Mode == OrbwalkingMode.None &&
                            MenuClass.Miscellaneous["reversede"].As<MenuBool>().Enabled)
                        {
                            UtilityClass.LastTick = Game.TickCount;
                            SpellClass.E.Cast(UtilityClass.Player.ServerPosition.Extend(Game.CursorPos, -SpellClass.E.Range));
                        }
                        break;
                }
            }
        }

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PostAttackEventArgs" /> instance containing the event data.</param>
        public void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (ImplementationClass.IOrbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Weaving(sender, args);
                    break;
                case OrbwalkingMode.Laneclear:
                    Jungleclear(sender, args);
                    break;
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
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Obj_AI_BaseMissileClientDataEventArgs" /> instance containing the event data.</param>
        public void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender.IsMe)
            {
                /// <summary>
                ///     Initializes the orbwalkingmodes.
                /// </summary>
                switch (ImplementationClass.IOrbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                        switch (args.SpellData.Name)
                        {
                            //case "CaitlynEntrapment":
                            case "CaitlynEntrapmentMissile":
                                if (SpellClass.W.Ready &&
                                    MenuClass.Spells["w"]["triplecombo"].As<MenuBool>().Enabled)
                                {
                                    var bestTarget = GameObjects.EnemyHeroes
                                        .Where(t => !Invulnerable.Check(t) && t.IsValidTarget(SpellClass.W.Range))
                                        .MinBy(o => o.Distance(args.End));
                                    if (bestTarget != null &&
                                        CanTrap(bestTarget))
                                    {
                                        SpellClass.W.Cast(UtilityClass.Player.ServerPosition.Extend(bestTarget.ServerPosition, UtilityClass.Player.Distance(bestTarget) + bestTarget.BoundingRadius));
                                        UpdateEnemyTrapTime(bestTarget.NetworkId);
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
        public void OnGapcloser(Obj_AI_Hero sender, Gapcloser.GapcloserArgs args)
        {
            if (UtilityClass.Player.IsDead)
            {
                return;
            }
            
            if (sender == null || !sender.IsEnemy || Invulnerable.Check(sender))
            {
                return;
            }

            /// <summary>
            ///     The Anti-Gapcloser E.
            /// </summary>
            if (SpellClass.E.Ready)
            {
                var enabledOption2 = MenuClass.Gapcloser2["enabled"];
                if (enabledOption2 == null || !enabledOption2.As<MenuBool>().Enabled)
                {
                    return;
                }

                var spellOption2 = MenuClass.SubGapcloser2[$"{sender.ChampionName.ToLower()}.{args.SpellName.ToLower()}"];
                if (spellOption2 == null || !spellOption2.As<MenuBool>().Enabled)
                {
                    return;
                }

                switch (args.Type)
                {
                    case Gapcloser.Type.Targeted:
                        if (sender.IsMelee &&
                            args.Target.IsMe)
                        {
                            var targetPos = UtilityClass.Player.ServerPosition.Extend(args.StartPosition, -450);
                            if (targetPos.PointUnderEnemyTurret())
                            {
                                return;
                            }

                            SpellClass.E.Cast(args.StartPosition);
                        }
                        break;
                    default:
                        var targetPos2 = UtilityClass.Player.ServerPosition.Extend(args.EndPosition, -450);
                        if (targetPos2.PointUnderEnemyTurret())
                        {
                            return;
                        }

                        if (args.EndPosition.Distance(UtilityClass.Player.ServerPosition) <= SpellClass.E.Range)
                        {
                            SpellClass.E.Cast(args.EndPosition);
                        }
                        break;
                }
            }

            if (!CanTrap(sender))
            {
                return;
            }

            /// <summary>
            ///     The Anti-Gapcloser W.
            /// </summary>
            if (SpellClass.W.Ready &&
                args.EndPosition.Distance(UtilityClass.Player.ServerPosition) <= SpellClass.W.Range)
            {
                var enabledOption = MenuClass.Gapcloser2["enabled"];
                if (enabledOption == null || !enabledOption.As<MenuBool>().Enabled)
                {
                    return;
                }

                var spellOption = MenuClass.SubGapcloser[$"{sender.ChampionName.ToLower()}.{args.SpellName.ToLower()}"];
                if (spellOption == null || !spellOption.As<MenuBool>().Enabled)
                {
                    return;
                }

                SpellClass.W.Cast(args.EndPosition);
                UpdateEnemyTrapTime(sender.NetworkId);
            }
        }

        /*
        /// <summary>
        ///     Called on interruptable spell.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="args">The <see cref="Events.InterruptableTargetEventArgs" /> instance containing the event data.</param>
        public void OnInterruptableTarget(object sender, Events.InterruptableTargetEventArgs args)
        {
            if (UtilityClass.Player.IsDead || Invulnerable.Check(args.Sender, DamageType.Magical, false))
            {
                return;
            }

            if (SpellClass.E.State == SpellState.Ready && args.Sender.IsValidTarget(SpellClass.E.SpellData.Range)
                && MenuClass.Spells["e"]["interrupter"].As<MenuBool>().Enabled)
            {
                if (!SpellClass.E.GetPrediction(args.Sender).CollisionObjects.Any())
                {
                    SpellClass.E.Cast(SpellClass.E.GetPrediction(args.Sender).UnitPosition);
                }
            }

            if (SpellClass.W.State == SpellState.Ready && args.Sender.IsValidTarget(SpellClass.W.SpellData.Range)
                && MenuClass.Spells["w"]["interrupter"].As<MenuBool>().Enabled)
            {
                SpellClass.W.Cast(SpellClass.W.GetPrediction(args.Sender).CastPosition);
            }
        }
        */

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void OnUpdate()
        {
            if (UtilityClass.Player.IsDead)
            {
                return;
            }

            /// <summary>
            ///     Initializes the Killsteal events.
            /// </summary>
            Killsteal();

            /// <summary>
            ///     Initializes the Automatic actions.
            /// </summary>
            Automatic();

            if (ImplementationClass.IOrbwalker.IsWindingUp)
            {
                return;
            }

            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (ImplementationClass.IOrbwalker.Mode)
            {
                case OrbwalkingMode.Mixed:
                    Harass();
                    break;

                case OrbwalkingMode.Laneclear:
                    Laneclear();
                    break;
            }
        }

        #endregion
    }
}