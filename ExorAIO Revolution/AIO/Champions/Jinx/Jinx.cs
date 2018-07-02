
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
    internal partial class Jinx
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Jinx.
        /// </summary>
        public Jinx()
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
        ///     Called on orbwalker action.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="args">The <see cref="PreAttackEventArgs" /> instance containing the event data.</param>
        public void OnPreAttack(object sender, PreAttackEventArgs args)
        {
            switch (ImplementationClass.IOrbwalker.Mode)
            {
                case OrbwalkingMode.Laneclear:
                    Laneclear(sender, args);
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

            if (sender == null || !sender.IsEnemy || Invulnerable.Check(sender, DamageType.Magical, false))
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
            if (SpellClass.E.Ready &&
                args.EndPosition.Distance(UtilityClass.Player.ServerPosition) <= SpellClass.E.Range)
            {
                SpellClass.E.Cast(args.EndPosition);
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
            }
        }

        #endregion
    }
}