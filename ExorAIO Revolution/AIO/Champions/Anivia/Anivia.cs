
using Entropy;
using Entropy.SDK.Orbwalking;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Anivia
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Anivia.
        /// </summary>
        public Anivia()
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
        
        /// <param name="args">The <see cref="NonKillableMinionEventArgs" /> instance containing the event data.</param>
        public void OnNonKillableMinion(NonKillableMinionEventArgs args)
        {
            var minion = (AIMinionClient)args.Target;
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
                    if (SpellClass.E.Ready &&
                        minion.IsValidTarget(SpellClass.E.Range) &&
                        UtilityClass.Player.MPPercent()
                            > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.Spells["e"]["lasthitunk"]) &&
                        MenuClass.Spells["e"]["lasthitunk"].As<MenuSliderBool>().Enabled)
                    {
                        if (minion.GetRealHealth() <= GetFrostBiteDamage(minion))
                        {
                            SpellClass.E.CastOnUnit(minion);
                        }
                    }
                    break;
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
                    case "Anivia_Base_Q_AOE_Mis.troy":
                        FlashFrost = obj;
                        break;
                    case "Anivia_Base_R_indicator_ring.troy":
                        GlacialStorm = obj;
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
                switch (obj.Name)
                {
                    case "Anivia_Base_Q_AOE_Mis.troy":
                        FlashFrost = null;
                        break;
                    case "Anivia_Base_R_indicator_ring.troy":
                        GlacialStorm = null;
                        break;
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

            var enabledOption = MenuClass.Gapcloser["enabled"];
            if (enabledOption == null || !enabledOption.As<MenuBool>().Enabled)
            {
                return;
            }

            if (sender == null || !sender.IsEnemy())
            {
                return;
            }

            var spellOption = MenuClass.SubGapcloser[$"{sender.CharName.ToLower()}.{args.SpellName.ToLower()}"];
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
                        if (sender.IsMelee &&
                            args.Target.IsMe())
                        {
                            SpellClass.W.Cast(UtilityClass.Player.Position.Extend(args.StartPosition, UtilityClass.Player.BoundingRadius));
                        }
                        break;
                    default:
                        if (args.EndPosition.Distance(UtilityClass.Player.Position) <= SpellClass.W.Range)
                        {
                            SpellClass.W.Cast(sender.Position.Extend(args.EndPosition, sender.BoundingRadius));
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
            Killsteal(EntropyEventArgs args);

            /// <summary>
            ///     Initializes the Automatic actions.
            /// </summary>
            Automatic(EntropyEventArgs args);

            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (Orbwalker.Implementation.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo(EntropyEventArgs args);
                    break;
                case OrbwalkingMode.Harass:
                    Harass(EntropyEventArgs args);
                    break;
                case OrbwalkingMode.LaneClear:
                    LaneClear(EntropyEventArgs args);
                    break;
                case OrbwalkingMode.LastHit:
                    LastHit(EntropyEventArgs args);
                    break;
            }
        }

        #endregion
    }
}