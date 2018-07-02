
using System.Linq;
using Aimtec;
using Aimtec.SDK.Events;
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
    internal partial class Vayne
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Vayne.
        /// </summary>
        public Vayne()
        {
            /// <summary>
            ///     Initializes the menus.
            /// </summary>
            Menus();

            /// <summary>
            ///     Updates the spells.
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
                    Lasthit(sender, args);
                    Laneclear(sender, args);
                    Jungleclear(sender, args);
                    Buildingclear(sender, args);
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
            if (!UtilityClass.Player.IsUnderEnemyTurret() &&
                UtilityClass.Player.HasBuff("vaynetumblefade"))
            {
                var invisibilityBuff = UtilityClass.Player.GetBuff("vaynetumblefade");
                if (MenuClass.Miscellaneous["stealthtime"].As<MenuSliderBool>().Enabled &&
                    invisibilityBuff.GetRemainingBuffTime() >
                    invisibilityBuff.EndTime - invisibilityBuff.StartTime - MenuClass.Miscellaneous["stealthtime"].As<MenuSliderBool>().Value / 1000f)
                {
                    args.Cancel = true;
                }

                if (UtilityClass.Player.HasBuff("summonerexhaust"))
                {
                    args.Cancel = true;
                }

                if (MenuClass.Miscellaneous["stealthcheck"].As<MenuSliderBool>().Enabled &&
                    GameObjects.EnemyHeroes.Count(t =>
                        t.IsValidTarget(UtilityClass.Player.GetFullAttackRange(t))) >=
                    MenuClass.Miscellaneous["stealthcheck"].As<MenuSliderBool>().Value)
                {
                    args.Cancel = true;
                }
            }

            /// <summary>
            ///     The Target Forcing Logic.
            /// </summary>
            if (MenuClass.Miscellaneous["focusw"].As<MenuBool>().Enabled)
            {
                var forceTarget = Extensions.GetBestEnemyHeroesTargets().FirstOrDefault(t =>
                        t.GetBuffCount("vaynesilvereddebuff") == 2 &&
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
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (ImplementationClass.IOrbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    CondemnCombo();
                    break;
            }
        }

        /// <summary>
        ///     Fired on an incoming dash.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Dash.DashArgs" /> instance containing the event data.</param>
        public void OnDash(object sender, Dash.DashArgs args)
        {
            var heroSender = args.Unit as Obj_AI_Hero;
            if (heroSender == null || !heroSender.IsEnemy || Invulnerable.Check(heroSender, DamageType.Magical, false))
            {
                return;
            }

            if (heroSender.ChampionName.Equals("Kalista"))
            {
                return;
            }

            var endPos = (Vector3)args.EndPos;
            var playerPos = UtilityClass.Player.ServerPosition;

            if (!heroSender.IsValidTarget(SpellClass.E.Range) &&
                endPos.Distance(playerPos) > SpellClass.E.Range)
            {
                return;
            }

            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["emode"].As<MenuList>().Value != 2 &&
                MenuClass.Spells["e"]["whitelist"][heroSender.ChampionName.ToLower()].Enabled)
            {
                const int condemnPushDistance = 410;
                for (var i = UtilityClass.Player.BoundingRadius; i < condemnPushDistance; i += 10)
                {
                    if (!endPos.Extend(playerPos, -i).IsWall(true))
                    {
                        continue;
                    }

                    UtilityClass.CastOnUnit(SpellClass.E, heroSender);
                }
            }
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

            if (sender == null || !sender.IsEnemy || !sender.IsMelee)
            {
                return;
            }

            /// <summary>
            ///     The Anti-Gapcloser Q.
            /// </summary>
            if (SpellClass.Q.Ready)
            {
                var enabledOption = MenuClass.Gapcloser["enabled"];
                if (enabledOption == null || !enabledOption.As<MenuBool>().Enabled)
                {
                    return;
                }

                var spellOption = MenuClass.SubGapcloser[$"{sender.ChampionName.ToLower()}.{args.SpellName.ToLower()}"];
                if (spellOption == null || !spellOption.As<MenuBool>().Enabled)
                {
                    return;
                }

                switch (args.Type)
                {
                    case Gapcloser.Type.Targeted:
                        if (args.Target.IsMe)
                        {
                            var targetPos = UtilityClass.Player.ServerPosition.Extend(args.StartPosition, -SpellClass.Q.Range+UtilityClass.Player.AttackRange);
                            if (targetPos.PointUnderEnemyTurret())
                            {
                                return;
                            }

                            SpellClass.Q.Cast(targetPos);
                        }
                        break;
                    default:
                        var targetPos2 = UtilityClass.Player.ServerPosition.Extend(args.EndPosition, -SpellClass.Q.Range+UtilityClass.Player.AttackRange);
                        if (targetPos2.PointUnderEnemyTurret())
                        {
                            return;
                        }

                        if (args.EndPosition.Distance(UtilityClass.Player.ServerPosition) <= UtilityClass.Player.AttackRange)
                        {
                            SpellClass.Q.Cast(targetPos2);
                        }
                        break;
                }
            }

            /// <summary>
            ///     The Anti-Gapcloser E.
            /// </summary>
            if (SpellClass.E.Ready &&
                !Invulnerable.Check(sender, DamageType.Magical, false))
            {
                var enabledOption2 = MenuClass.Gapcloser2["enabled"];
                if (enabledOption2 == null || !enabledOption2.As<MenuBool>().Enabled)
                {
                    return;
                }

                var spellOption2 = MenuClass.SubGapcloser2[$"{sender.ChampionName.ToLower()}.{args.SpellName.ToLower()}"];
                if (spellOption2 == null || !spellOption2.As<MenuBool>().Enabled)
                {
                    return;
                }

                switch (args.Type)
                {
                    case Gapcloser.Type.Targeted:
                        if (args.Target.IsMe)
                        {
                            UtilityClass.CastOnUnit(SpellClass.E, sender);
                        }
                        break;
                    default:
                        if (args.EndPosition.Distance(UtilityClass.Player.ServerPosition) <= UtilityClass.Player.AttackRange)
                        {
                            UtilityClass.CastOnUnit(SpellClass.E, sender);
                        }
                        break;
                }
            }
        }

        /*
        /// <summary>
        ///     Called on interruptable spell.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Events.InterruptableTargetEventArgs" /> instance containing the event data.</param>
        public void OnInterruptableTarget(object sender, Events.InterruptableTargetEventArgs args)
        {
            if (UtilityClass.Player.IsDead || Invulnerable.Check(args.Sender, DamageType.Magical, false))
            {
                return;
            }

            if (SpellClass.E.State == SpellState.Ready && args.Sender.IsValidTarget(SpellClass.E.SpellData.Range)
                && MenuClass.Spells["e"]["interrupter"].As<MenuBool>().Enabled)
            {
                UtilityClass.Player.SpellBook.CastSpell(SpellSlot.E, args.Sender);
            }
        }
        */

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
            }
        }

        #endregion
    }
}