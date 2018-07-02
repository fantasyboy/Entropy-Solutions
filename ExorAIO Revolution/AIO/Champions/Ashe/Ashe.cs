
using System;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Ashe
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Ashe.
        /// </summary>
        public Ashe()
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
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnLeaveVisibility(AttackableUnit sender, EventArgs args)
        {
            if (sender.IsEnemy)
            {
                if (SpellClass.E.Ready &&
                    SpellClass.E.Ammo >= (MenuClass.Spells["e"]["logical"].As<MenuBool>().Enabled ? 2 : 1) &&
                    MenuClass.Spells["e"]["vision"].As<MenuBool>().Enabled)
                {
                    SpellClass.E.Cast(sender.ServerPosition);
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
                    Buildingclear(sender, args);
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

            var enabledOption = MenuClass.Gapcloser["enabled"];
            if (enabledOption == null || !enabledOption.As<MenuBool>().Enabled)
            {
                return;
            }

            if (sender == null || !sender.IsEnemy || Invulnerable.Check(sender))
            {
                return;
            }

            var spellOption = MenuClass.SubGapcloser[$"{sender.ChampionName.ToLower()}.{args.SpellName.ToLower()}"];
            if (spellOption == null || !spellOption.As<MenuBool>().Enabled)
            {
                return;
            }

            /// <summary>
            ///     The Anti-Gapcloser R.
            /// </summary>
            if (SpellClass.R.Ready)
            {
                switch (args.Type)
                {
                    case Gapcloser.Type.Targeted:
                        if (sender.IsMelee &&
                            args.Target.IsMe)
                        {
                            SpellClass.R.Cast(args.StartPosition);
                        }
                        break;
                    default:
                        if (args.EndPosition.Distance(UtilityClass.Player.ServerPosition) <= UtilityClass.Player.AttackRange)
                        {
                            SpellClass.R.Cast(args.EndPosition);
                        }
                        break;
                }
            }
        }

        /*
        /// <summary>
        ///     Called on interruptable spell.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Events.InterruptableTargetEventArgs" /> instance containing the event data.</param>
        public void OnInterruptableTarget(object sender, Events.InterruptableTargetEventArgs args)
        {
            if (UtilityClass.Player.IsDead || Invulnerable.Check(args.Sender, DamageType.Magical, false))
            {
                return;
            }

            if (SpellClass.R.State == SpellState.Ready && args.Sender.IsValidTarget(SpellClass.R.SpellData.Range)
                && MenuClass.Spells["r"]["interrupter"].As<MenuBool>().Enabled)
            {
                SpellClass.R.Cast(args.Sender.ServerPosition);
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

            if (ImplementationClass.IOrbwalker.IsWindingUp)
            {
                return;
            }

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
                    break;
            }
        }

        #endregion
    }
}