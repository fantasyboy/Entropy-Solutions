
using System.Linq;
using Entropy;
using Entropy.SDK.Events;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;
using SharpDX;

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
                    Lasthit(args);
                    Laneclear(args);
                    Jungleclear(args);
                    Buildingclear(args);
                    break;
            }
        }

        /// <summary>
        ///     Called on pre attack.
        /// </summary>
        /// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
        public void OnPreAttack(OnPreAttackEventArgs args)
        {
            if (!UtilityClass.Player.Position.IsUnderEnemyTurret() &&
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
                        t.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(t))) >=
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
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    CondemnCombo(args);
                    break;
            }
        }

        /// <summary>
        ///     Fired on an incoming dash.
        /// </summary>
        
        /// <param name="args">The <see cref="Dash.DashArgs" /> instance containing the event data.</param>
        public void OnDash(Dash.DashArgs args)
        {
            var heroSender = args.Sender as AIHeroClient;
            if (heroSender == null || !heroSender.IsEnemy() || Invulnerable.Check(heroSender, DamageType.Magical, false))
            {
                return;
            }

            if (heroSender.CharName.Equals("Kalista"))
            {
                return;
            }

            var endPos = args.EndPosition;
            var playerPos = UtilityClass.Player.Position;

            if (!heroSender.IsValidTarget(SpellClass.E.Range) &&
                endPos.Distance((Vector2)playerPos) > SpellClass.E.Range)
            {
                return;
            }

            if (SpellClass.E.Ready &&
                MenuClass.Spells["e"]["emode"].As<MenuList>().Value != 2 &&
                MenuClass.Spells["e"]["whitelist"][heroSender.CharName.ToLower()].Enabled)
            {
                const int condemnPushDistance = 410;
                for (var i = UtilityClass.Player.BoundingRadius; i < condemnPushDistance; i += 10)
                {
                    if (!endPos.Extend((Vector2)playerPos, -i).IsWall(true))
                    {
                        continue;
                    }

                    SpellClass.E.CastOnUnit(heroSender);
                }
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

            if (sender == null || !sender.IsEnemy() || !sender.IsMelee)
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

                var spellOption = MenuClass.SubGapcloser[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"];
                if (spellOption == null || !spellOption.As<MenuBool>().Enabled)
                {
                    return;
                }

                switch (args.Type)
                {
                    case Gapcloser.Type.Targeted:
                        if (args.Target.IsMe())
                        {
                            Vector3 targetPos = UtilityClass.Player.Position.Extend(args.StartPosition, -SpellClass.Q.Range+UtilityClass.Player.GetAutoAttackRange());
                            if (targetPos..Position.IsUnderEnemyTurret())
                            {
                                return;
                            }

                            SpellClass.Q.Cast(targetPos);
                        }
                        break;
                    default:
                        Vector3 targetPos2 = UtilityClass.Player.Position.Extend(args.EndPosition, -SpellClass.Q.Range+UtilityClass.Player.GetAutoAttackRange());
                        if (targetPos2..Position.IsUnderEnemyTurret())
                        {
                            return;
                        }

                        if (args.EndPosition.Distance((Vector2)UtilityClass.Player.Position) <= UtilityClass.Player.GetAutoAttackRange())
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

                var spellOption2 = MenuClass.SubGapcloser2[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"];
                if (spellOption2 == null || !spellOption2.As<MenuBool>().Enabled)
                {
                    return;
                }

                switch (args.Type)
                {
                    case Gapcloser.Type.Targeted:
                        if (args.Target.IsMe())
                        {
                            SpellClass.E.CastOnUnit(sender);
                        }
                        break;
                    default:
                        if (args.EndPosition.Distance((Vector2)UtilityClass.Player.Position) <= UtilityClass.Player.GetAutoAttackRange())
                        {
                            SpellClass.E.CastOnUnit(sender);
                        }
                        break;
                }
            }
        }

        /*
        /// <summary>
        ///     Called on interruptable spell.
        /// </summary>
        
        /// <param name="args">The <see cref="Events.InterruptableTargetEventArgs" /> instance containing the event data.</param>
        public void OnInterruptableTarget(Events.InterruptableTargetEventArgs args)
        {
            if (UtilityClass.Player.IsDead || Invulnerable.Check(args.Sender, DamageType.Magical, false))
            {
                return;
            }

            if (SpellClass.E.State == SpellState.Ready && args.Sender.IsValidTarget(SpellClass.E.SpellData.Range)
                && MenuClass.Spells["e"]["interrupter"].As<MenuBool>().Enabled)
            {
                UtilityClass.Player.Spellbook.CastSpell(SpellSlot.E, args.Sender);
            }
        }
        */

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
            }
        }

        #endregion
    }
}