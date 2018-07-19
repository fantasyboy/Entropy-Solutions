
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Enumerations;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Caitlyn
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(EntropyEventArgs args)
        {
			if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The Automatic W Logic. 
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.W["logical"].Enabled)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(t =>
					CanTrap(t) &&
					t.IsImmobile(SpellClass.W.Delay) &&
                    t.DistanceToPlayer() < SpellClass.W.Range))
                {
                    SpellClass.W.Cast(UtilityClass.Player.Position.Extend(target.Position, UtilityClass.Player.Distance(target)+target.BoundingRadius/2));
                    //UpdateEnemyTrapTime(target.NetworkID);
                }
            }

            /// <summary>
            ///     The Automatic Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Q["logical"].Enabled)
            {
                switch (Orbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                    case OrbwalkingMode.Harass:
                        foreach (var target in GameObjects.EnemyHeroes.Where(t =>
                            !Invulnerable.Check(t) &&
                            t.IsImmobile(SpellClass.Q.Delay) &&
                            t.IsValidTarget(SpellClass.Q.Range) &&
                            t.HasBuff("caitlynyordletrapdebuff")))
                        {
                            SpellClass.Q.Cast(target.Position);
                        }
                        break;
                }
            }

            /// <summary>
            ///     The Semi-Automatic R Management.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.R["bool"].Enabled &&
                MenuClass.R["key"].Enabled)
            {
                var bestTarget = GameObjects.EnemyHeroes
                    .Where(t =>
                        !Invulnerable.Check(t) &&
                        t.IsValidTarget(SpellClass.R.Range) &&
                        MenuClass.R["whitelist"][t.CharName.ToLower()].Enabled)
                    .MinBy(o => o.GetRealHealth());
                if (bestTarget != null)
                {
                    SpellClass.R.CastOnUnit(bestTarget);
                }
            }
        }

        #endregion
    }
}