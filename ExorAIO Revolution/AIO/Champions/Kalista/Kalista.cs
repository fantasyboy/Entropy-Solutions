
using System.Linq;
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
    internal partial class Kalista
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Kalista.
        /// </summary>
        public Kalista()
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
        /// <param name="sender">The object.</param>
        /// <param name="args">The <see cref="PreAttackEventArgs" /> instance containing the event data.</param>
        public void OnPreAttack(object sender, PreAttackEventArgs args)
        {
            /// <summary>
            ///     The Target Forcing Logic.
            /// </summary>
            if (MenuClass.Miscellaneous["focusw"].As<MenuBool>().Enabled)
            {
                var forceTarget = Extensions.GetBestEnemyHeroesTargets().FirstOrDefault(t =>
                        t.HasBuff("kalistacoopstrikemarkally") &&
                        t.IsValidTarget(UtilityClass.Player.GetFullAttackRange(t)));
                if (forceTarget != null)
                {
                    args.Target = forceTarget;
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

            /// <summary>
            ///     Initializes the Killsteal events.
            /// </summary>
            RendKillsteal();

            /// <summary>
            ///     Initializes the Automatic events.
            /// </summary>
            RendAutomatic();

            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (ImplementationClass.IOrbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    RendCombo();
                    break;
                case OrbwalkingMode.Mixed:
                    RendHarass();
                    break;
                case OrbwalkingMode.Laneclear:
                    RendLaneclear();
                    RendJungleclear();
                    break;
                case OrbwalkingMode.Lasthit:
                    RendLasthit();
                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        private void OnUpdate()
        {
            if (UtilityClass.Player.IsDead)
            {
                return;
            }

            /// <summary>
            ///     Initializes the Automatic actions.
            /// </summary>
            Automatic();

            /// <summary>
            ///     Initializes the Killsteal events.
            /// </summary>
            Killsteal();

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
                case OrbwalkingMode.Lasthit:
                    Lasthit();
                    break;
            }
        }

        #endregion
    }
}