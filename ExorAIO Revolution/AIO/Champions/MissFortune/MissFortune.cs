
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Damage;
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
    internal partial class MissFortune
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Miss Fortune.
        /// </summary>
        public MissFortune()
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
        ///     Fired on spell cast.
        /// </summary>
        
        /// <param name="args">The <see cref="SpellbookCastSpellEventArgs" /> instance containing the event data.</param>
        public void OnLocalCastSpell(SpellbookLocalCastSpellEventArgs args)
        {
            if (sender.IsMe())
            {
                if (args.Slot == SpellSlot.R &&
                    !IsUltimateShooting())
                {
                    Orbwalker.MovingEnabled = false;
                }
                else if (IsUltimateShooting())
                {
                    args.Process = false;
                }
            }
        }

        /// <summary>
        ///     Fired on spell cast.
        /// </summary>
        
        /// <param name="buff">The buff.</param>
        public void OnRemoveBuff(AIBaseClient sender, Buff buff)
        {
            if (sender.IsMe() &&
                buff.Name == "missfortunebulletsound")
            {
                Orbwalker.MovingEnabled = true;
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
            ///     The Anti-Gapcloser E.
            /// </summary>
            if (SpellClass.E.Ready)
            {
                switch (args.Type)
                {
                    case Gapcloser.Type.Targeted:
                        if (sender.IsMelee &&
                            args.Target.IsMe())
                        {
                            SpellClass.E.Cast(args.EndPosition);
                        }
                        break;
                    default:
                        if (args.EndPosition.Distance(UtilityClass.Player.Position) <= SpellClass.E.Range)
                        {
                            SpellClass.E.Cast(args.EndPosition);
                        }
                        break;
                }
            }
        }

        /// <summary>
        ///     Called on do-cast.
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
                    Buildingclear(sender, args);
                    break;
            }
        }

        /// <summary>
        ///     Called on pre attack.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
        public void OnPreAttack(OnPreAttackEventArgs args)
        {
            if (IsUltimateShooting())
            {
                args.Cancel = true;
            }

            /// <summary>
            ///     The Target Forcing Logic.
            /// </summary>
            if (MenuClass.Miscellaneous["focusp"].As<MenuSliderBool>().Enabled)
            {
                var orbTarget = args.Target as AIHeroClient;
                var forceTarget = Extensions.GetBestEnemyHeroesTargets().FirstOrDefault(t =>
                        t.NetworkID != LoveTapTargetNetworkId &&
                        t.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(t)));
                if (forceTarget != null &&
                    orbTarget != null &&
                    orbTarget.GetRealHealth() >
                        UtilityClass.Player.GetAutoAttackDamage(orbTarget) *
                        MenuClass.Miscellaneous["focusp"].As<MenuSliderBool>().Value)
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
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo(EntropyEventArgs args);
                    break;

                case OrbwalkingMode.LaneClear:
                    LaneClear(EntropyEventArgs args);
                    break;

                case OrbwalkingMode.Harass:
                    Harass(EntropyEventArgs args);
                    break;
            }
        }

        #endregion
    }
}