
using System.Linq;
using Entropy;
using Entropy.SDK.Extensions;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Ashe
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(args)
        {
            if (UtilityClass.Player.IsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The Semi-Automatic R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["bool"].As<MenuBool>().Enabled &&
                MenuClass.Spells["r"]["key"].As<MenuKeyBind>().Enabled)
            {
                var bestTarget = GameObjects.EnemyHeroes.Where(t =>
                        t.IsValidTarget(2000f) &&
                        !Invulnerable.Check(t, DamageType.Magical, false) &&
                        MenuClass.Spells["r"]["whitelist"][t.CharName.ToLower()].As<MenuBool>().Enabled)
                    .MinBy(o => o.GetRealHealth());
                if (bestTarget != null)
                {
                    if (SpellClass.E.Ready &&
                        MenuClass.Spells["e"]["logical"].As<MenuBool>().Enabled)
                    {
                        SpellClass.E.Cast(bestTarget);
                    }

                    SpellClass.R.Cast(bestTarget);
                }
            }
        }

        #endregion
    }
}