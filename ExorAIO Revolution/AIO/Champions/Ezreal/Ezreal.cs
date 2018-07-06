using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Ezreal
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Ezreal.
        /// </summary>
        public Ezreal()
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
        ///     Called on process autoattack.
        /// </summary>
        
        /// <param name="args">The <see cref="AIBaseClientMissileClientDataEventArgs" /> instance containing the event data.</param>
        public void OnProcessAutoAttack(AIBaseClient sender, AIBaseClientMissileClientDataEventArgs args)
        {
            if (UtilityClass.Player.TotalAbilityDamage >= GetMinimumApForApMode())
            {
                return;
            }

            var senderAlly = sender as AIHeroClient;
            var unitTarget = args.Target as AIBaseClient;
            if (unitTarget == null || senderAlly == null || !senderAlly.IsAlly || senderAlly.IsMe())
            {
                return;
            }

            var buffMenu = MenuClass.Spells["w"]["buff"];
            if (buffMenu == null ||
                !buffMenu["allywhitelist"][senderAlly.CharName.ToLower()].As<MenuBool>().Enabled)
            {
                return;
            }

            /// <summary>
            ///     The Ally W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                senderAlly.IsValidTarget(SpellClass.W.Range, true) && 
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, buffMenu["logical"]) &&
                buffMenu["logical"].As<MenuSliderBool>().Enabled)
            {
                var orbWhiteList = buffMenu["orbwhitelist"];
                switch (ImplementationClass.IOrbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                        if (!(unitTarget is AIHeroClient) ||
                            !orbWhiteList["combo"].As<MenuBool>().Enabled)
                        {
                            return;
                        }
                        break;

                    case OrbwalkingMode.Harass:
                        if (!(unitTarget is AIHeroClient) ||
                            !orbWhiteList["harass"].As<MenuBool>().Enabled)
                        {
                            return;
                        }
                        break;

                    case OrbwalkingMode.LaneClear:
                        if (!unitTarget.IsBuilding() &&
                            !Extensions.GetLegendaryJungleMinionsTargets().Contains(unitTarget) ||
                            !orbWhiteList["laneclear"].As<MenuBool>().Enabled)
                        {
                            return;
                        }
                        break;
                    default:
                        return;
                }

                SpellClass.W.CastOnUnit(senderAlly);
            }
        }

        /// <summary>
        ///     Called on non killable minion.
        /// </summary>
        
        /// <param name="args">The <see cref="NonKillableMinionEventArgs" /> instance containing the event data.</param>
        public void OnNonKillableMinion(NonKillableMinionEventArgs args)
        {
            var minion = args.Target as AIMinionClient;
            if (minion == null)
            {
                return;
            }

            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (ImplementationClass.IOrbwalker.Mode)
            {
                case OrbwalkingMode.LaneClear:
                case OrbwalkingMode.LastHit:
                case OrbwalkingMode.Harass:
                    if (SpellClass.Q.Ready &&
                        minion.GetRealHealth() < UtilityClass.Player.GetSpellDamage(minion, SpellSlot.Q) &&
                        UtilityClass.Player.MPPercent()
                            > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["farmhelper"]) &&
                        MenuClass.Spells["q"]["farmhelper"].As<MenuSliderBool>().Enabled)
                    {
                        SpellClass.Q.Cast(minion);
                    }
                    break;
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
            switch (ImplementationClass.IOrbwalker.Mode)
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

            if (sender == null || !sender.IsEnemy()() || !sender.IsMelee)
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
                        if (args.Target.IsMe())
                        {
                            var targetPos = UtilityClass.Player.Position.Extend(args.StartPosition, -SpellClass.E.Range);
                            if (targetPos.IsUnderEnemyTurret())
                            {
                                return;
                            }

                            SpellClass.E.Cast(targetPos);
                        }
                        break;
                    default:
                        var targetPos2 = UtilityClass.Player.Position.Extend(args.EndPosition, -SpellClass.E.Range);
                        if (targetPos2.IsUnderEnemyTurret())
                        {
                            return;
                        }

                        if (args.EndPosition.Distance(UtilityClass.Player.Position) <= UtilityClass.Player.GetAutoAttackRange())
                        {
                            SpellClass.E.Cast(targetPos2);
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
                case OrbwalkingMode.LastHit:
                    LastHit(args);
                    break;
                case OrbwalkingMode.LaneClear:
                    LaneClear(args);
                    break;
            }
        }

        #endregion
    }
}