
using Entropy;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    using Utilities;

	/// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Quinn
    {
        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Combo(EntropyEventArgs args)
        {
            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                !IsUsingBehindEnemyLines() &&
                MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range - 100f);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget))
                {
                    switch (MenuClass.Spells["q"]["modes"]["combo"].As<MenuList>().Value)
                    {
                        case 1:
                            SpellClass.Q.Cast(bestTarget);
                            break;
                    }
                }
            }

            /// <summary>
            ///     The E Combo Logic.
            /// </summary>
            if (SpellClass.E.Ready &&
                IsUsingBehindEnemyLines() &&
                MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
            {
                var bestTarget = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.E.Range);
                if (bestTarget != null &&
                    !Invulnerable.Check(bestTarget))
                {
                    SpellClass.E.CastOnUnit(bestTarget);
                }
            }
        }
    }
}