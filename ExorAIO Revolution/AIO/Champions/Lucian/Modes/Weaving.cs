
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;
using SharpDX;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Lucian
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The E Combo Logic.
        /// </summary>
        public void ELogic(AIHeroClient target)
        {
            if (UtilityClass.Player.Distance(Hud.CursorPositionUnclipped) <= UtilityClass.Player.GetAutoAttackRange() &&
                MenuClass.Root["e"]["customization"]["onlyeifmouseoutaarange"].Enabled)
            {
                return;
            }

            var posAfterE = UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, 300f);
            var eRangeCheck = MenuClass.Root["e"]["customization"]["erangecheck"];
            if (eRangeCheck != null)
            {
                if (eRangeCheck.Enabled &&
                    posAfterE.EnemyHeroesCount(UtilityClass.Player.GetAutoAttackRange() + UtilityClass.Player.BoundingRadius) >= eRangeCheck.Value)
                {
                    return;
                }
            }

            if (posAfterE.Distance(target) >
                    UtilityClass.Player.GetAutoAttackRange(target) &&
                MenuClass.Root["e"]["customization"]["noeoutaarange"].Enabled)
            {
                return;
            }

            if (posAfterE.IsUnderEnemyTurret() &&
                MenuClass.Root["e"]["customization"]["noeturret"].Enabled)
            {
                return;
            }

            switch (MenuClass.Root["e"]["mode"].Value)
            {
                case 0:
                    Vector3 point;
                    if (UtilityClass.Player.Position.IsUnderEnemyTurret() ||
                        UtilityClass.Player.Distance(Hud.CursorPositionUnclipped) < UtilityClass.Player.GetAutoAttackRange())
                    {
                        point = UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, UtilityClass.Player.BoundingRadius);
                    }
                    else
                    {
                        point = UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, 475f);
                    }

                    SpellClass.E.Cast(point);
                    return;

                case 1:
                    SpellClass.E.Cast(UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, 425f));
                    return;

                case 2:
                    SpellClass.E.Cast(UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, UtilityClass.Player.BoundingRadius));
                    return;
            }
        }

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        
        /// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
        public void Weaving(OnPostAttackEventArgs args)
        {
            var heroTarget = args.Target as AIHeroClient;
            if (heroTarget == null)
            {
                return;
            }

            switch (MenuClass.Root["pattern"].Value)
            {
                case 0:
                case 2:
                    /// <summary>
                    ///     The E Combo Logic.
                    /// </summary>
                    if (SpellClass.E.Ready &&
                        MenuClass.Root["e"]["combo"].Enabled)
                    {
                        ELogic(heroTarget);
                        return;
                    }
                    break;

                case 1:
                    /// <summary>
                    ///     The Q Combo Logic.
                    /// </summary>
                    if (SpellClass.Q.Ready &&
                        MenuClass.Root["q"]["combo"].Enabled)
                    {
                        SpellClass.Q.CastOnUnit(heroTarget);
                        return;
                    }
                    break;

                case 3:
                    /// <summary>
                    ///     The W Combo Logic.
                    /// </summary>
                    if (SpellClass.W.Ready &&
                        MenuClass.Root["w"]["combo"].Enabled)
                    {
                        SpellClass.W.Cast(heroTarget);
                        return;
                    }
                    break;
            }

            switch (MenuClass.Root["pattern"].Value)
            {
                case 0:
                    /// <summary>
                    ///     The Q Combo Logic.
                    /// </summary>
                    if (SpellClass.Q.Ready &&
                        MenuClass.Root["q"]["combo"].Enabled)
                    {
                        SpellClass.Q.CastOnUnit(heroTarget);
                        return;
                    }
                    break;

                case 1:
                case 3:
                    /// <summary>
                    ///     The E Combo Logic.
                    /// </summary>
                    if (SpellClass.E.Ready &&
                        MenuClass.Root["e"]["combo"].Enabled)
                    {
                        ELogic(heroTarget);
                        return;
                    }
                    break;

                case 2:
                    /// <summary>
                    ///     The W Combo Logic.
                    /// </summary>
                    if (SpellClass.W.Ready &&
                        MenuClass.Root["w"]["combo"].Enabled)
                    {
                        SpellClass.W.Cast(heroTarget);
                        return;
                    }
                    break;
            }

            switch (MenuClass.Root["pattern"].Value)
            {
                case 0:
                case 1:
                    /// <summary>
                    ///     The W Combo Logic.
                    /// </summary>
                    if (SpellClass.W.Ready &&
                        MenuClass.Root["w"]["combo"].Enabled)
                    {
                        SpellClass.W.Cast(heroTarget);
                    }
                    break;

                case 2:
                case 3:
                    /// <summary>
                    ///     The Q Combo Logic.
                    /// </summary>
                    if (SpellClass.Q.Ready &&
                        MenuClass.Root["q"]["combo"].Enabled)
                    {
                        SpellClass.Q.CastOnUnit(heroTarget);
                    }
                    break;
            }
        }

        #endregion
    }
}