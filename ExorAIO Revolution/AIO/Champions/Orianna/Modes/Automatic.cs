
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;
using SharpDX;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Orianna
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Automatic(args)
        {
            if (BallPosition == null)
            {
                return;
            }

            /// <summary>
            ///     The Automatic R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["aoe"] != null &&
                MenuClass.Spells["r"]["aoe"].As<MenuSliderBool>().Enabled)
            {
                var countValidTargets = GameObjects.EnemyHeroes.Count(t =>
                        !Invulnerable.Check(t, DamageType.Magical, false) &&
                        t.IsValidTarget(SpellClass.R.Width - t.BoundingRadius - SpellClass.R.Delay * t.BoundingRadius, false, false, (Vector3)BallPosition));
            
                if (countValidTargets >= MenuClass.Spells["r"]["aoe"].As<MenuSliderBool>().Value)
                {
                    SpellClass.R.Cast();
                }
            }
        }

        #endregion
    }
}