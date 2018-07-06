
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression

using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Taliyah
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Automatic(args)
        {
            SpellClass.R.Range = 1500 + 1500 * SpellClass.R.Level();

            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The Automatic W Logic. 
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["logical"].As<MenuBool>().Enabled)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(t =>
                    t.IsImmobile(SpellClass.W.Delay) &&
                    t.IsValidTarget(SpellClass.W.Range) &&
                    !Invulnerable.Check(t, DamageType.Magical, false)))
                {
                    SpellClass.W.Cast(GetUnitPositionAfterPull(target), SpellClass.W.GetPrediction(target).CastPosition);
                }
            }
        }

        #endregion
    }
}