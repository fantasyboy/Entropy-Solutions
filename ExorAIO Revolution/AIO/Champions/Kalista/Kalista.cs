
using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Caching;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Orbwalking.EventArgs;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Kalista
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Kalista.
        /// </summary>
        public Kalista()
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
		///     Called on processing spellcast operations.
		/// </summary>
		/// <param name="args">The <see cref="AIBaseClientCastEventArgs" /> instance containing the event data.</param>
		public void OnProcessSpellCast(AIBaseClientCastEventArgs args)
		{
			var soulbound = args.Target as AIHeroClient;
			if (soulbound != null)
			{
				//Logging.Log($"SpellData.Name: {args.SpellData.Name}, ARGS.tARGET: {args.Target.CharName}");
				if (args.Caster.IsMe()                         &&
				    ObjectCache.AllyHeroes.Contains(soulbound) &&
				    args.SpellData.Name == "KalistaPSpellCast")
				{
					SoulBound = soulbound;
				}
			}
		}

        /// <summary>
        ///     Called on pre attack.
        /// </summary>
        /// <param name="args">The <see cref="OnPreAttackEventArgs" /> instance containing the event data.</param>
        public void OnPreAttack(OnPreAttackEventArgs args)
        {
			/// <summary>
			///     The Target Forcing Logic.
			/// </summary>
			if (MenuClass.Miscellaneous["focusw"].Enabled)
            {
	            if (Orbwalker.Mode != OrbwalkingMode.Combo &&
	                Orbwalker.Mode != OrbwalkingMode.Harass)
	            {
		            return;
	            }

				var forceTarget = Extensions.GetBestEnemyHeroesTargets().FirstOrDefault(t =>
                        t.HasBuff("kalistacoopstrikemarkally") &&
                        t.IsValidTarget(UtilityClass.Player.GetAutoAttackRange(t)));
                if (forceTarget != null)
                {
                    args.Target = forceTarget;
                }
            }
        }

	    #endregion

        #region Methods

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        private void OnUpdate(EntropyEventArgs args)
        {
            if (UtilityClass.Player.IsDead)
            {
                return;
            }

            /// <summary>
            ///     Initializes the Automatic actions.
            /// </summary>
            Automatic();

            /// <summary>
            ///     Initializes the Killsteal events.
            /// </summary>
            Killsteal();

            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo();
                    break;
                case OrbwalkingMode.Harass:
                    Harass();
                    break;
                case OrbwalkingMode.LaneClear:
                    LaneClear();
                    JungleClear();
                    break;
                case OrbwalkingMode.LastHit:
                    LastHit();
                    break;
            }
        }

        #endregion
    }
}