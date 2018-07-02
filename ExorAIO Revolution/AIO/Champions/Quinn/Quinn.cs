using System;
using System.Linq;
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
    internal partial class Quinn
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Quinn.
        /// </summary>
        public Quinn()
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
        ///     Called on pre attack.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PreAttackEventArgs" /> instance containing the event data.</param>
        public void OnPreAttack(object sender, PreAttackEventArgs args)
        {
            /// <summary>
            ///     Check for R Instance.
            /// </summary>
            if (IsUsingBehindEnemyLines())
            {
                args.Cancel = true;
            }

            /// <summary>
            ///     The Target Forcing Logic.
            /// </summary>
            if (MenuClass.Miscellaneous["focuspassive"].As<MenuBool>().Enabled)
            {
                var forceTarget = Extensions.GetBestEnemyHeroesTargets().FirstOrDefault(t =>
                    IsVulnerable(t) &&
                    t.IsValidTarget(UtilityClass.Player.GetFullAttackRange(t)));
                if (forceTarget != null)
                {
                    args.Target = forceTarget;
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
        ///     Fired upon unit leaving visibility.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnLeaveVisibility(AttackableUnit unit, EventArgs args)
        {
            /// <summary>
            ///     The Vision W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["vision"].As<MenuBool>().Value)
            {
                SpellClass.W.Cast();
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

            var enabledOption = MenuClass.Gapcloser["enabled"];
            if (enabledOption == null || !enabledOption.As<MenuBool>().Enabled)
            {
                return;
            }

            if (sender == null || !sender.IsEnemy)
            {
                return;
            }

            var spellOption = MenuClass.SubGapcloser[$"{sender.ChampionName.ToLower()}.{args.SpellName.ToLower()}"];
            if (spellOption == null || !spellOption.As<MenuBool>().Enabled)
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
                        if (args.Target.IsMe)
                        {
                            UtilityClass.CastOnUnit(SpellClass.E, sender);
                        }
                        break;
                    default:
                        if (args.EndPosition.PointUnderEnemyTurret())
                        {
                            return;
                        }

                        SpellClass.E.Cast(sender);
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
                case OrbwalkingMode.Lasthit:
                    Lasthit();
                    break;
                case OrbwalkingMode.Laneclear:
                    Laneclear();
                    break;
            }
        }

        #endregion
    }
}