
using System.Linq;
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
    internal partial class Xayah
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Xayah.
        /// </summary>
        public Xayah()
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

            /// <summary>
            ///     Reloads the WorkedGrounds.
            /// </summary>
            ReloadFeathers();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Fired upon GameObject creation.
        /// </summary>
        public void OnCreate(GameObjectCreateEventArgs args)
        {
			var obj = args.Sender;
			if (obj.IsValid)
			{
                switch (obj.Name)
                {
                    case "Xayah_Base_Passive_Dagger_Mark8s":
                        Feathers.Add(obj.NetworkID, obj.Position);
                        break;
                }
            }
        }

        /// <summary>
        ///     Fired upon GameObject creation.
        /// </summary>
        public void OnDelete(GameObjectDeleteEventArgs args)
        {
			var obj = args.Sender;
			if (obj.IsValid)
			{
                if (Feathers.Any(o => o.Key == obj.NetworkID))
                {
                    Feathers.Remove(obj.NetworkID);
                }
            }
        }

        /// <summary>
        ///     Called on do-cast.
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
                    Weaving(args);
                    break;
                case OrbwalkingMode.LaneClear:
                    Buildingclear(args);
                    Jungleclear(args);
                    break;
            }
        }

        /// <summary>
        ///     Called while processing spellcast operations.
        /// </summary>
        /// <param name="args">The <see cref="AIBaseClientCastEventArgs" /> instance containing the event data.</param>
        public void OnProcessSpellCast(AIBaseClientCastEventArgs args)
        {
            if (args.Caster.IsMe())
            {
                switch (args.Slot)
                {
                    case SpellSlot.E:
                        LastCastedETime = Game.ClockTime;
                        break;
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

            var enabledOption = MenuClass.Gapcloser["enabled"];
            if (enabledOption == null || !enabledOption.As<MenuBool>().Enabled)
            {
                return;
            }

            if (sender == null || !sender.IsEnemy() || !sender.IsMelee)
            {
                return;
            }

            var spellOption = MenuClass.SubGapcloser[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"];
            if (spellOption == null || !spellOption.As<MenuBool>().Enabled)
            {
                return;
            }

            /// <summary>
            ///     The Anti-Gapcloser R.
            /// </summary>
            if (SpellClass.R.Ready)
            {
                switch (args.Type)
                {
                    case Gapcloser.Type.Targeted:
                        if (args.Target.IsMe())
                        {
                            SpellClass.R.Cast(args.StartPosition);
                        }
                        break;
                    default:
                        if (args.EndPosition.Distance(UtilityClass.Player.Position) <= UtilityClass.Player.GetAutoAttackRange())
                        {
                            SpellClass.R.Cast(args.StartPosition);
                        }
                        break;
                }
            }
        }

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void OnTick(EntropyEventArgs args)
        {
            if (UtilityClass.Player.IsDead)
            {
                return;
            }

            /// <summary>
            ///     Initializes the Automatic actions.
            /// </summary>
            Automatic(args);

			/// <summary>
			///     Initializes the Killsteal events.
			/// </summary>
			Killsteal(args);

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

                case OrbwalkingMode.LaneClear:
                    LaneClear(args);
                    break;
			}
        }

        #endregion
    }
}