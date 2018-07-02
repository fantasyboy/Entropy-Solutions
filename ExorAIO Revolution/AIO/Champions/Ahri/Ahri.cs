
using Entropy;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Util;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Ahri
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Ahri.
        /// </summary>
        public Ahri()
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
            ///     Updates the spells.
            /// </summary>
            Spells();
        }

        #endregion

        #region Public Methods and Operators

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
        ///     Called while processing spellcast operations.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Obj_AI_BaseMissileClientDataEventArgs" /> instance containing the event data.</param>
        public void OnPerformCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender.IsMe)
            {
                switch (args.SpellSlot)
                {
                    case SpellSlot.Q:
                    case SpellSlot.W:
                    case SpellSlot.E:
                        if (UtilityClass.Player.HasBuff("AhriTumble") &&
                            MenuClass.Spells["r"]["customization"]["onlyrfirst"].As<MenuBool>().Enabled)
                        {
                            break;
                        }

                        if (!UtilityClass.Player.HasBuff("AhriTumble") &&
                            MenuClass.Spells["r"]["customization"]["onlyrstarted"].As<MenuBool>().Enabled)
                        {
                            break;
                        }

                        if (SpellClass.R.Ready &&
                            MenuClass.Spells["r"]["combo"].As<MenuBool>().Enabled)
                        {
                            const float rRadius = 500f;
                            var heroTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.R.Range + rRadius);
                            if (heroTarget == null ||
                                    Invulnerable.Check(heroTarget, DamageType.Magical) ||
                                !MenuClass.Spells["r"]["whitelist"][heroTarget.ChampionName.ToLower()].As<MenuBool>().Enabled)
                            {
                                break;
                            }

                            var position = UtilityClass.Player.ServerPosition.Extend(Game.CursorPos, SpellClass.R.Range);
                            if (heroTarget.IsValidTarget(rRadius, false, false, position))
                            {
                                DelayAction.Queue(200+Game.Ping, ()=> SpellClass.R.Cast(position));
                            }
                        }
                        break;

                    case SpellSlot.R:
                        if (SpellClass.W.Ready &&
                            MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
                        {
                            SpellClass.W.Cast();
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

            if (sender == null || !sender.IsEnemy)
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
                if (enabledOption == null || !enabledOption.As<MenuBool>().Enabled)
                {
                    return;
                }

                var spellOption = MenuClass.SubGapcloser[$"{sender.ChampionName.ToLower()}.{args.SpellName.ToLower()}"];
                if (spellOption == null || !spellOption.As<MenuBool>().Enabled)
                {
                    return;
                }

                switch (args.Type)
                {
                    case Gapcloser.Type.Targeted:
                        if (args.Target.IsMe)
                        {
                            SpellClass.E.Cast(args.StartPosition);
                            return;
                        }
                        break;
                    default:
                        if (args.EndPosition.Distance(UtilityClass.Player.ServerPosition) <= SpellClass.E.Range)
                        {
                            SpellClass.E.Cast(args.EndPosition);
                            return;
                        }
                        break;
                }
            }

            /// <summary>
            ///     The Anti-Gapcloser R.
            /// </summary>
            if (SpellClass.R.Ready && sender.IsMelee)
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
                            var targetPos = UtilityClass.Player.ServerPosition.Extend(args.StartPosition, -SpellClass.R.Range);
                            if (targetPos.PointUnderEnemyTurret())
                            {
                                return;
                            }

                            SpellClass.R.Cast(targetPos);
                        }
                        break;
                    default:
                        var targetPos2 = UtilityClass.Player.ServerPosition.Extend(args.EndPosition, -SpellClass.R.Range);
                        if (targetPos2.PointUnderEnemyTurret())
                        {
                            return;
                        }

                        if (args.EndPosition.Distance(UtilityClass.Player.ServerPosition) <= UtilityClass.Player.AttackRange)
                        {
                            SpellClass.R.Cast(targetPos2);
                        }
                        break;
                }
            }
        }

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

            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (ImplementationClass.IOrbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo();
                    break;
                case OrbwalkingMode.Mixed:
                    Harass();
                    break;
                case OrbwalkingMode.Laneclear:
                    Laneclear();
                    Jungleclear();
                    break;
            }
        }

        #endregion
    }
}