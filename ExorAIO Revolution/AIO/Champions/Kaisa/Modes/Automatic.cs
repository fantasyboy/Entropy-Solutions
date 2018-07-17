
using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Kaisa
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Semi-Automatic R Management.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Root["r"]["bool"].Enabled &&
                MenuClass.Root["r"]["key"].Enabled)
            {
                var bestTarget = GameObjects.EnemyHeroes.FirstOrDefault(t =>
						!Invulnerable.Check(t) &&
						t.IsValidTarget(SpellClass.R.Range) &&
						t.HasBuff("kaisapassivemarkerr"));
                if (bestTarget != null)
                {
                    SpellClass.R.CastOnUnit(bestTarget);
                }
            }
        }

        #endregion
    }
}