using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
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
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Obj_AI_BaseMissileClientDataEventArgs" /> instance containing the event data.</param>
        public void OnProcessAutoAttack(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (UtilityClass.Player.TotalAbilityDamage >= GetMinimumApForApMode())
            {
                return;
            }

            var senderAlly = sender as Obj_AI_Hero;
            var unitTarget = args.Target as Obj_AI_Base;
            if (unitTarget == null || senderAlly == null || !senderAlly.IsAlly || senderAlly.IsMe)
            {
                return;
            }

            var buffMenu = MenuClass.Spells["w"]["buff"];
            if (buffMenu == null ||
                !buffMenu["allywhitelist"][senderAlly.ChampionName.ToLower()].As<MenuBool>().Enabled)
            {
                return;
            }

            /// <summary>
            ///     The Ally W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                senderAlly.IsValidTarget(SpellClass.W.Range, true) && 
                UtilityClass.Player.ManaPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, buffMenu["logical"]) &&
                buffMenu["logical"].As<MenuSliderBool>().Enabled)
            {
                var orbWhiteList = buffMenu["orbwhitelist"];
                switch (ImplementationClass.IOrbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                        if (!(unitTarget is Obj_AI_Hero) ||
                            !orbWhiteList["combo"].As<MenuBool>().Enabled)
                        {
                            return;
                        }
                        break;

                    case OrbwalkingMode.Mixed:
                        if (!(unitTarget is Obj_AI_Hero) ||
                            !orbWhiteList["harass"].As<MenuBool>().Enabled)
                        {
                            return;
                        }
                        break;

                    case OrbwalkingMode.Laneclear:
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

                UtilityClass.CastOnUnit(SpellClass.W, senderAlly);
            }
        }

        /// <summary>
        ///     Called on non killable minion.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="NonKillableMinionEventArgs" /> instance containing the event data.</param>
        public void OnNonKillableMinion(object sender, NonKillableMinionEventArgs args)
        {
            var minion = args.Target as Obj_AI_Minion;
            if (minion == null)
            {
                return;
            }

            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (ImplementationClass.IOrbwalker.Mode)
            {
                case OrbwalkingMode.Laneclear:
                case OrbwalkingMode.Lasthit:
                case OrbwalkingMode.Mixed:
                    if (SpellClass.Q.Ready &&
                        minion.GetRealHealth() < UtilityClass.Player.GetSpellDamage(minion, SpellSlot.Q) &&
                        UtilityClass.Player.ManaPercent()
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

            if (sender == null || !sender.IsEnemy || !sender.IsMelee)
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
                            var targetPos = UtilityClass.Player.ServerPosition.Extend(args.StartPosition, -SpellClass.E.Range);
                            if (targetPos.PointUnderEnemyTurret())
                            {
                                return;
                            }

                            SpellClass.E.Cast(targetPos);
                        }
                        break;
                    default:
                        var targetPos2 = UtilityClass.Player.ServerPosition.Extend(args.EndPosition, -SpellClass.E.Range);
                        if (targetPos2.PointUnderEnemyTurret())
                        {
                            return;
                        }

                        if (args.EndPosition.Distance(UtilityClass.Player.ServerPosition) <= UtilityClass.Player.AttackRange)
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