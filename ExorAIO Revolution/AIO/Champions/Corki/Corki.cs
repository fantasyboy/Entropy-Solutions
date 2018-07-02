
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
    internal partial class Corki
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Corki.
        /// </summary>
        public Corki()
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
        ///     Called on non killable minion.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="NonKillableMinionEventArgs" /> instance containing the event data.</param>
        public void OnNonKillableMinion(object sender, NonKillableMinionEventArgs args)
        {
            var minion = (Obj_AI_Minion)args.Target;
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
                    if (SpellClass.R.Ready &&
                        minion.IsValidTarget(SpellClass.R.Range) &&
                        UtilityClass.Player.ManaPercent()
                            > ManaManager.GetNeededMana(SpellClass.R.Slot, MenuClass.Spells["r"]["lasthitunk"]) &&
                        MenuClass.Spells["r"]["lasthitunk"].As<MenuSliderBool>().Enabled)
                    {
                        if (minion.GetRealHealth() <= GetMissileDamage(minion))
                        {
                            SpellClass.R.Cast(minion);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        ///     Called on post attack.
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
        ///     Called on pre attack.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PreAttackEventArgs" /> instance containing the event data.</param>
        public void OnPreAttack(object sender, PreAttackEventArgs args)
        {
            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (ImplementationClass.IOrbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Weaving(sender, args);
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

            if (sender == null || !sender.IsEnemy || !sender.IsMelee || HasPackage())
            {
                return;
            }

            var spellOption = MenuClass.SubGapcloser[$"{sender.ChampionName.ToLower()}.{args.SpellName.ToLower()}"];
            if (spellOption == null || !spellOption.As<MenuBool>().Enabled)
            {
                return;
            }

            /// <summary>
            ///     The Anti-Gapcloser W.
            /// </summary>
            if (SpellClass.W.Ready)
            {
                switch (args.Type)
                {
                    case Gapcloser.Type.Targeted:
                        if (args.Target.IsMe)
                        {
                            var targetPos = UtilityClass.Player.ServerPosition.Extend(args.StartPosition, -SpellClass.W.Range);
                            if (targetPos.PointUnderEnemyTurret())
                            {
                                return;
                            }

                            SpellClass.W.Cast(targetPos);
                        }
                        break;
                    default:
                        var targetPos2 = UtilityClass.Player.ServerPosition.Extend(args.EndPosition, -SpellClass.W.Range);
                        if (targetPos2.PointUnderEnemyTurret())
                        {
                            return;
                        }

                        if (args.EndPosition.Distance(UtilityClass.Player.ServerPosition) <= UtilityClass.Player.AttackRange)
                        {
                            SpellClass.W.Cast(targetPos2);
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
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (Orbwalker.Implementation.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo();
                    break;
                case OrbwalkingMode.Mixed:
                    Harass();
                    break;
                case OrbwalkingMode.Laneclear:
                    Laneclear();
                    break;
                case OrbwalkingMode.Lasthit:
                    Lasthit();
                    break;
            }
        }

        #endregion
    }
}