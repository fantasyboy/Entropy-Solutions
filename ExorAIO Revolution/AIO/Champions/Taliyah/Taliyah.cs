
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;
using Entropy.SDK.Utils;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Taliyah
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Taliyah.
        /// </summary>
        public Taliyah()
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
            ///     Initializes the spells.
            /// </summary>
            Spells();

            /// <summary>
            ///     Reloads the MineField.
            /// </summary>
            ReloadMineField();

            /// <summary>
            ///     Reloads the WorkedGrounds.
            /// </summary>
            ReloadWorkedGrounds();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Called on spell cast.
        /// </summary>
        /// <param name="sender">The SpellBook.</param>
        /// <param name="args">The <see cref="SpellBookCastSpellEventArgs" /> instance containing the event data.</param>
        public void OnCastSpell(SpellbookLocalCastSpellEventArgs args)
        {
            if (sender.IsMe())
            {
                switch (ImplementationClass.IOrbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                        switch (args.Slot)
                        {
                            case SpellSlot.Q:
                                if (Game.ClockTime - LastWTime < 0.75)
                                {
                                    args.Process = false;
                                }
                                break;

                            case SpellSlot.W:
                                if (SpellClass.E.Ready &&
                                    UtilityClass.Player.Mana <
                                        SpellSlot.W.GetManaCost() +
                                        SpellSlot.E.GetManaCost())
                                {
                                    args.Process = false;
                                }
                                else
                                {
                                    LastWTime = Game.ClockTime;
                                }
                                break;
                        }
                        break;
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
                    case "Taliyah_Base_Q_aoe.troy":
                    case "Taliyah_Base_Q_aoe_river.troy":
                        WorkedGrounds.Add(obj.NetworkID, screenPos);
                        break;

                    case "Taliyah_Base_E_Mines.troy":
                        MineField.Add(obj.NetworkID, screenPos);
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
                if (WorkedGrounds.Any(o => o.Key == obj.NetworkID))
                {
                    WorkedGrounds.Remove(obj.NetworkID);
                }

                if (MineField.Any(o => o.Key == obj.NetworkID))
                {
                    MineField.Remove(obj.NetworkID);
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
        ///     Fired on an incoming gapcloser.
        /// </summary>
        
        /// <param name="args">The <see cref="Gapcloser.GapcloserArgs" /> instance containing the event data.</param>
        public void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserArgs args)
        {
            if (UtilityClass.Player.IsDead)
            {
                return;
            }
            
            if (sender == null || !sender.IsEnemy()())
            {
                return;
            }

            /// <summary>
            ///     The Anti-Gapcloser W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
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
                        if (sender.IsMelee &&
                            args.Target.IsMe())
                        {
                            SpellClass.W.Cast(args.EndPosition, args.EndPosition.Extend(args.StartPosition, 200f));
                        }
                        break;
                    default:
                        if (args.EndPosition.Distance(UtilityClass.Player.Position) <= UtilityClass.Player.GetAutoAttackRange())
                        {
                            SpellClass.W.Cast(args.EndPosition, args.EndPosition.Extend(args.StartPosition, sender.IsMelee ? 200f : -200f));
                        }
                        break;
                }
            }

            /// <summary>
            ///     The Anti-Gapcloser E Logic.
            /// </summary>
            if (SpellClass.E.Ready)
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
                            SpellClass.E.Cast(args.StartPosition);
                        }
                        break;
                    default:
                        if (args.EndPosition.Distance(UtilityClass.Player.Position) <= UtilityClass.Player.GetAutoAttackRange())
                        {
                            SpellClass.E.Cast(args.StartPosition);
                        }
                        break;
                }
            }
        }

        /*
        /// <summary>
        ///     Called on interruptable spell.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="args">The <see cref="Events.InterruptableTargetEventArgs" /> instance containing the event data.</param>
        public void OnInterruptableTarget(Events.InterruptableTargetEventArgs args)
        {
            if (UtilityClass.Player.IsDead || Invulnerable.Check(args.Sender, DamageType.Magical, false))
            {
                return;
            }

            if (SpellClass.W.State == SpellState.Ready && args.Sender.IsValidTarget(SpellClass.W.SpellData.Range)
                && MenuClass.Spells["w"]["interrupter"].As<MenuBool>().Enabled)
            {
                SpellClass.W.Cast(args.Sender.Position, UtilityClass.Player.Position);
            }
        }
        */

        /// <summary>
        ///     Called while processing spellcast operations.
        /// </summary>
        
        /// <param name="args">The <see cref="AIBaseClientMissileClientDataEventArgs" /> instance containing the event data.</param>
        public void OnProcessSpellCast(AIBaseClientCastEventArgs args)
        {
            if (sender.IsMe())
            {
                switch (args.SpellSlot)
                {
                    /// <summary>
                    ///     The W->E Combo Logic.
                    /// </summary>
                    case SpellSlot.W:
                        if (SpellClass.E.Ready)
                        {
                            switch (ImplementationClass.IOrbwalker.Mode)
                            {
                                case OrbwalkingMode.Combo:
                                    if (MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
                                    {
                                        SpellClass.E.Cast(args.End);
                                    }
                                    break;
                            }
                        }
                        break;

                    /// <summary>
                    ///     Automatically Mount on R Logic.
                    /// </summary>
                    case SpellSlot.R:
                        if (SpellClass.R.Ready &&
                            MenuClass.Spells["r"]["mountr"].As<MenuBool>().Enabled)
                        {
                            DelayAction.Queue(500, () =>
                                {
                                    SpellClass.R.Cast();
                                });
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

            if (ImplementationClass.IOrbwalker.IsWindingUp)
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
            switch (ImplementationClass.IOrbwalker.Mode)
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