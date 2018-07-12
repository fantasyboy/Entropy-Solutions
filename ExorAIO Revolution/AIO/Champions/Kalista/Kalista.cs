
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;

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
        /// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
        public void OnPreAttack(OnPreAttackEventArgs args)
        {
            /// <summary>
            ///     The Target Forcing Logic.
            /// </summary>
            if (MenuClass.Miscellaneous["focusw"].As<MenuBool>().Enabled)
            {
                var forceTarget = Extensions.GetBestEnemyHeroesTargets().FirstOrDefault(t =>
                        t.HasBuff("kalistacoopstrikemarkally") &&
                        t.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(t)));
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
            RendKillsteal(EntropyEventArgs args);

            /// <summary>
            ///     Initializes the Automatic events.
            /// </summary>
            RendAutomatic(EntropyEventArgs args);

            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    RendCombo(EntropyEventArgs args);
                    break;
                case OrbwalkingMode.Harass:
                    RendHarass(EntropyEventArgs args);
                    break;
                case OrbwalkingMode.LaneClear:
                    RendLaneClear(EntropyEventArgs args);
                    RendJungleClear(EntropyEventArgs args);
                    break;
                case OrbwalkingMode.LastHit:
                    RendLastHit(EntropyEventArgs args);
                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        private void OnUpdate(EntropyEventArgs args)
        {
            if (UtilityClass.Player.IsDead)
            {
                return;
            }

            /// <summary>
            ///     Initializes the Automatic actions.
            /// </summary>
            Automatic(EntropyEventArgs args);

            /// <summary>
            ///     Initializes the Killsteal events.
            /// </summary>
            Killsteal(EntropyEventArgs args);

            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo(EntropyEventArgs args);
                    break;
                case OrbwalkingMode.Harass:
                    Harass(EntropyEventArgs args);
                    break;
                case OrbwalkingMode.LaneClear:
                    LaneClear(EntropyEventArgs args);
                    JungleClear(EntropyEventArgs args);
                    break;
                case OrbwalkingMode.LastHit:
                    LastHit(EntropyEventArgs args);
                    break;
            }
        }

        #endregion
    }
}