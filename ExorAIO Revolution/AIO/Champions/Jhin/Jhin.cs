
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
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
    internal partial class Jhin
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Jhin.
        /// </summary>
        public Jhin()
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
        ///     Fired on spell cast.
        /// </summary>
        
        /// <param name="args">The <see cref="SpellbookCastSpellEventArgs" /> instance containing the event data.</param>
        public void OnLocalCastSpell(SpellbookLocalCastSpellEventArgs args)
        {
            if (IsUltimateShooting() &&
                args.Slot != SpellSlot.R)
            {
                args.Execute = false;
            }

            switch (args.Slot)
            {
                case SpellSlot.E:
                    var target = Orbwalker.GetOrbwalkingTarget() as AIHeroClient;
                    if (target != null &&
                        target.HasBuff("jhinetrapslow"))
                    {
                        args.Execute = false;
                    }
                    break;
            }
        }

        /// <summary>
        ///     Called on post attack.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
        public void OnPostAttack(OnPostAttackEventArgs args)
        {
            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Weaving(args);
                    break;
                case OrbwalkingMode.LaneClear:
                    Jungleclear(args);
                    break;
            }
        }

        /// <summary>
        ///     Called on pre attack.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
        public void OnPreAttack(OnPreAttackEventArgs args)
        {
            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Weaving(args);
                    break;
                case OrbwalkingMode.LaneClear:
                    Jungleclear(args);
                    break;
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

            var enabledOption = MenuClass.Gapcloser["enabled"];
            if (enabledOption == null || !enabledOption.As<MenuBool>().Enabled)
            {
                return;
            }

            if (sender == null || !sender.IsEnemy() || Invulnerable.Check(sender))
            {
                return;
            }

            var spellOption = MenuClass.SubGapcloser[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"];
            if (spellOption == null || !spellOption.As<MenuBool>().Enabled)
            {
                return;
            }

            /// <summary>
            ///     The Anti-Gapcloser E.
            /// </summary>
            if (SpellClass.E.Ready &&
                args.EndPosition.Distance(UtilityClass.Player.Position) <= SpellClass.E.Range)
            {
                SpellClass.E.Cast(args.EndPosition);
            }
        }

		/// <summary>
		///     Handles the <see cref="E:ProcessSpell" /> event.
		/// </summary>
		/// <param name="args">The <see cref="AIBaseClientCastEventArgs" /> instance containing the event data.</param>
		public void OnProcessSpellCast(AIBaseClientCastEventArgs args)
        {
            if (args.Caster.IsMe())
            {
                switch (args.SpellData.Name)
                {
                    case "JhinR":
                        UltimateShotsCount = 0;
                        End = args.EndPosition;
                        break;

                    case "JhinRShot":
                        UltimateShotsCount++;
                        break;
                }
            }
        }

		/// <summary>
		///     Fired on issuing an order.
		/// </summary>
		/// <param name="args">The <see cref="LocalPlayerIssueOrderEventArgs" /> instance containing the event data.</param>
		public void OnIssueOrder(LocalPlayerIssueOrderEventArgs args)
        {
            if (IsUltimateShooting() &&
                args.Order == HeroOrder.MoveTo)
            {
                args.Execute = false;
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

            if (Orbwalker.IsWindingUp)
            {
                return;
            }

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

                case OrbwalkingMode.LastHit:
                    LastHit(args);
                    break;
            }
        }

        #endregion
    }
}