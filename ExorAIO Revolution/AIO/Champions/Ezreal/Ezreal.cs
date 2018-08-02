using Entropy;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Orbwalking.EventArgs;

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
		/// <param name="args">The <see cref="AIBaseClientCastEventArgs" /> instance containing the event data.</param>
		public void OnProcessBasicAttack(AIBaseClientCastEventArgs args)
        {
            if (UtilityClass.Player.CharIntermediate.TotalAbilityDamage() >= GetMinimumApForApMode())
            {
                return;
            }

			var sender = args.Caster as AIHeroClient;
            var unitTarget = args.Target as AIBaseClient;
            if (unitTarget == null || sender == null || !sender.IsAlly() || sender.IsMe())
            {
                return;
            }

            var buffMenu = MenuClass.W["buff"];
            if (buffMenu == null ||
                !buffMenu["allywhitelist"][sender.CharName.ToLower()].Enabled)
            {
                return;
            }

            /// <summary>
            ///     The Ally W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                sender.IsValidTargetEx(SpellClass.W.Range, true) && 
                UtilityClass.Player.MPPercent()
                    > ManaManager.GetNeededMana(SpellClass.W.Slot, buffMenu["logical"]) &&
                buffMenu["logical"].Enabled)
            {
                var orbWhiteList = buffMenu["orbwhitelist"];
                switch (Orbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                        if (!(unitTarget is AIHeroClient) ||
                            !orbWhiteList["combo"].Enabled)
                        {
                            return;
                        }
                        break;

                    case OrbwalkingMode.Harass:
                        if (!(unitTarget is AIHeroClient) ||
                            !orbWhiteList["harass"].Enabled)
                        {
                            return;
                        }
                        break;

                    case OrbwalkingMode.LaneClear:
						var minionTarget = unitTarget as AIMinionClient;
                        if (minionTarget == null ||
							!unitTarget.IsStructure() &&
                            !Extensions.GetLegendaryJungleMinionsTargets().Contains(minionTarget) ||
                            !orbWhiteList["laneclear"].Enabled)
                        {
                            return;
                        }
                        break;
                    default:
                        return;
                }

                SpellClass.W.CastOnUnit(sender);
            }
        }

		/// <summary>
		///     Called on non killable minion.
		/// </summary>
		/// <param name="args">The <see cref="OnNonKillableMinionEventArgs" /> instance containing the event data.</param>
		public void OnNonKillableMinion(OnNonKillableMinionEventArgs args)
        {
            var minion = args.Target as AIMinionClient;
            if (minion == null)
            {
                return;
            }

            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (Orbwalker.Mode)
            {
                case OrbwalkingMode.LaneClear:
                case OrbwalkingMode.LastHit:
                case OrbwalkingMode.Harass:
                    if (SpellClass.Q.Ready &&
                        minion.GetRealHealth() < GetQDamage(minion) &&
                        UtilityClass.Player.MPPercent()
                            > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["farmhelper"]) &&
                        MenuClass.Q["farmhelper"].Enabled)
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
            if (enabledOption == null || !enabledOption.Enabled)
            {
                return;
            }

            if (sender == null || !sender.IsEnemy() || !sender.IsMelee)
            {
                return;
            }

            var spellOption = MenuClass.SubGapcloser[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"];
            if (spellOption == null || !spellOption.Enabled)
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