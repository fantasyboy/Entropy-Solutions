
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
            if (sender.IsMe())
            {
                if (IsUltimateShooting() &&
                    args.Slot != SpellSlot.R)
                {
                    args.Process = false;
                }

                switch (args.Slot)
                {
                    case SpellSlot.E:
                        var target = Orbwalker.GetOrbwalkingTarget() as AIHeroClient;
                        if (target != null &&
                            target.HasBuff("jhinetrapslow"))
                        {
                            args.Process = false;
                        }
                        break;
                }
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
                    Weaving(sender, args);
                    break;
                case OrbwalkingMode.LaneClear:
                    Jungleclear(sender, args);
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
                    Weaving(sender, args);
                    break;
                case OrbwalkingMode.LaneClear:
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
        
        /// <param name="args">The <see cref="AIBaseClientMissileClientDataEventArgs" /> instance containing the event data.</param>
        public void OnProcessSpellCast(AIBaseClient sender, AIBaseClientMissileClientDataEventArgs args)
        {
            if (sender.IsMe())
            {
                switch (args.SpellData.Name)
                {
                    case "JhinR":
                        UltimateShotsCount = 0;
                        End = args.End;
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
        /// <param name="sender">The object.</param>
        /// <param name="args">The <see cref="AIBaseClientIssueOrderEventArgs" /> instance containing the event data.</param>
        public void OnIssueOrder(AIBaseClient sender, AIBaseClientIssueOrderEventArgs args)
        {
            if (sender.IsMe() &&
                IsUltimateShooting() &&
                args.OrderType == OrderType.MoveTo)
            {
                args.ProcessEvent = false;
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
            Killsteal(EntropyEventArgs args);

            if (Orbwalker.IsWindingUp)
            {
                return;
            }

            /// <summary>
            ///     Initializes the Automatic actions.
            /// </summary>
            Automatic(EntropyEventArgs args);

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