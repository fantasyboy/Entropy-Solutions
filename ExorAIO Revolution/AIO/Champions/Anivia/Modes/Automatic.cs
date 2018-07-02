
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using Entropy.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Anivia
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Automatic()
        {
            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The R Stacking Manager.
            /// </summary>
            if (SpellClass.R.ToggleState == 1 &&
                UtilityClass.Player.InFountain() &&
                UtilityClass.Player.IsTearLikeItemReady() &&
                MenuClass.Miscellaneous["tear"].As<MenuBool>().Value)
            {
                SpellClass.R.Cast(UtilityClass.Player.ServerPosition.Extend(Game.CursorPos, SpellClass.R.Range));
            }

            /// <summary>
            ///     The Automatic W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["logical"].As<MenuBool>().Value)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(t =>
                    t.IsImmobile(SpellClass.W.Delay) &&
                    t.Distance(UtilityClass.Player) < SpellClass.W.Range))
                {
                    SpellClass.W.Cast(
                        UtilityClass.Player.ServerPosition.Extend(
                            target.ServerPosition,
                            UtilityClass.Player.Distance(target) + target.BoundingRadius/2));
                }
            }
        }

        #endregion
    }
}