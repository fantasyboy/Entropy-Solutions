
using System.Linq;
using Entropy;
using Entropy.SDK.Events;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Syndra
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Syndra.
        /// </summary>
        public Syndra()
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
            if (sender.IsMe() &&
                args.Slot == SpellSlot.W &&
                !IsHoldingForceOfWillObject())
            {
                if (Game.TickCount - UtilityClass.LastTick >= 300)
                {
                    UtilityClass.LastTick = Game.TickCount;
                    HoldedSphere = args.Target;
                }
                else
                {
                    args.Process = false;
                }
            }
        }

        /// <summary>
        ///     Fired upon GameObject creation.
        /// </summary>
        public void OnCreate(GameObject obj)
        {
            if (obj.IsValid)
            {
                switch (obj.Name)
                {
                    case "Syndra_Base_Q_idle.troy":
                    case "Syndra_Base_Q_Lv5_idle.troy":
                        DarkSpheres.Add(obj.NetworkID, screenPos);
                        break;
                }
            }
        }

        /// <summary>
        ///     Fired upon GameObject creation.
        /// </summary>
        public void OnDestroy(GameObject obj)
        {
            if (obj.IsValid)
            {
                if (DarkSpheres.Any(o => o.Key == obj.NetworkID))
                {
                    DarkSpheres.Remove(obj.NetworkID);
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
        }

        /// <summary>
        ///     Fired on an incoming dash.
        /// </summary>
        
        /// <param name="args">The <see cref="Dash.DashArgs" /> instance containing the event data.</param>
        public void OnDash(Dash.DashArgs args)
        {
            var heroSender = args.Unit as AIHeroClient;
            if (heroSender == null || !heroSender.IsEnemy() || Invulnerable.Check(heroSender, DamageType.Magical))
            {
                return;
            }

            if (SpellClass.Q.Ready &&
                UtilityClass.Player.Distance(args.EndPos) <= SpellClass.Q.Range &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                SpellClass.Q.Cast(args.EndPos);
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

            if (sender == null || !sender.IsEnemy() || Invulnerable.Check(sender, DamageType.Magical, false))
            {
                return;
            }

            var spellOption = MenuClass.SubGapcloser[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"];
            if (spellOption == null || !spellOption.As<MenuBool>().Enabled)
            {
                return;
            }

            /// <summary>
            ///     The Anti-Gapcloser E Logic.
            /// </summary>
            if (SpellClass.E.Ready)
            {
                switch (args.Type)
                {
                    case Gapcloser.Type.Targeted:
                        if (sender.IsMelee &&
                            args.Target.IsMe())
                        {
                            if (SpellClass.Q.Ready)
                            {
                                SpellClass.Q.Cast(args.EndPosition);
                            }

                            SpellClass.E.Cast(args.StartPosition);
                        }
                        break;
                    default:
                        if (args.EndPosition.Distance(UtilityClass.Player.Position) <= UtilityClass.Player.GetAutoAttackRange())
                        {
                            if (SpellClass.Q.Ready)
                            {
                                SpellClass.Q.Cast(args.EndPosition);
                            }

                            SpellClass.E.Cast(args.EndPosition);
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
            Killsteal(EntropyEventArgs args);

            /// <summary>
            ///     Initializes the Automatic actions.
            /// </summary>
            Automatic(EntropyEventArgs args);

            /// <summary>
            ///     Reloads the DarkSpheres.
            /// </summary>
            ReloadDarkSpheres();

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
            }
        }

        #endregion
    }
}