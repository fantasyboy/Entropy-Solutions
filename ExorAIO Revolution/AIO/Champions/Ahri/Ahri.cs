
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;
using Entropy.SDK.Utils;
using SharpDX;

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
		/// <param name="args">The <see cref="AIBaseClientCastEventArgs" /> instance containing the event data.</param>
		public void OnProcessSpellCast(AIBaseClientCastEventArgs args)
        {
            if (args.Caster.IsMe())
            {
                switch (args.Slot)
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
                                !MenuClass.Spells["r"]["whitelist"][heroTarget.CharName.ToLower()].As<MenuBool>().Enabled)
                            {
                                break;
                            }

                            var position = UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, SpellClass.R.Range);
                            if (heroTarget.IsValidTarget(rRadius, false, false, position))
                            {
                                DelayAction.Queue(() => SpellClass.R.Cast(position), 200 + EnetClient.Ping);
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
        
        /// <param name="args">The <see cref="Gapcloser.GapcloserArgs" /> instance containing the event data.</param>
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
                            SpellClass.E.Cast(args.StartPosition);
                            return;
                        }
                        break;
                    default:
                        if (args.EndPosition.Distance((Vector2)UtilityClass.Player.Position) <= SpellClass.E.Range)
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

                var spellOption2 = MenuClass.SubGapcloser2[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"];
                if (spellOption2 == null || !spellOption2.As<MenuBool>().Enabled)
                {
                    return;
                }

                switch (args.Type)
                {
                    case Gapcloser.Type.Targeted:
                        if (sender.IsMelee &&
                            args.Target.IsMe())
                        {
                            var targetPos = UtilityClass.Player.Position.Extend(args.StartPosition, -SpellClass.R.Range);
                            if (targetPos.IsUnderEnemyTurret())
                            {
                                return;
                            }

                            SpellClass.R.Cast(targetPos);
                        }
                        break;
                    default:
                        var targetPos2 = UtilityClass.Player.Position.Extend(args.EndPosition, -SpellClass.R.Range);
                        if (targetPos2.IsUnderEnemyTurret())
                        {
                            return;
                        }

                        if (args.EndPosition.Distance(UtilityClass.Player.Position) <= UtilityClass.Player.GetAutoAttackRange())
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