
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;
using Entropy.SDK.UI.Components;
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
                MenuClass.Spells["e"]["customization"]["onlyeifmouseoutaarange"].As<MenuBool>().Enabled)
            {
                return;
            }

            var posAfterE = UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, 300f);
            var eRangeCheck = MenuClass.Spells["e"]["customization"]["erangecheck"];
            if (eRangeCheck != null)
            {
                if (eRangeCheck.As<MenuSliderBool>().Enabled &&
                    posAfterE.CountEnemyHeroesInRange(UtilityClass.Player.GetAutoAttackRange() + UtilityClass.Player.BoundingRadius) >= eRangeCheck.As<MenuSliderBool>().Value)
                {
                    return;
                }
            }

            if (posAfterE.Distance(target) >
                    UtilityClass.Player.GetAutoAttackRange(target) &&
                MenuClass.Spells["e"]["customization"]["noeoutaarange"].As<MenuBool>().Enabled)
            {
                return;
            }

            if (posAfterE.IsUnderEnemyTurret() &&
                MenuClass.Spells["e"]["customization"]["noeturret"].As<MenuBool>().Enabled)
            {
                return;
            }

            switch (MenuClass.Spells["e"]["mode"].As<MenuList>().Value)
            {
                case 0:
                    Vector3 point;
                    if (UtilityClass.Player.IsUnderEnemyTurret() ||
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

            switch (MenuClass.Spells["pattern"].As<MenuList>().Value)
            {
                case 0:
                case 2:
                    /// <summary>
                    ///     The E Combo Logic.
                    /// </summary>
                    if (SpellClass.E.Ready &&
                        MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
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
                        MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
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
                        MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
                    {
                        SpellClass.W.Cast(heroTarget);
                        return;
                    }
                    break;
            }

            switch (MenuClass.Spells["pattern"].As<MenuList>().Value)
            {
                case 0:
                    /// <summary>
                    ///     The Q Combo Logic.
                    /// </summary>
                    if (SpellClass.Q.Ready &&
                        MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
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
                        MenuClass.Spells["e"]["combo"].As<MenuBool>().Enabled)
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
                        MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
                    {
                        SpellClass.W.Cast(heroTarget);
                        return;
                    }
                    break;
            }

            switch (MenuClass.Spells["pattern"].As<MenuList>().Value)
            {
                case 0:
                case 1:
                    /// <summary>
                    ///     The W Combo Logic.
                    /// </summary>
                    if (SpellClass.W.Ready &&
                        MenuClass.Spells["w"]["combo"].As<MenuBool>().Enabled)
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
                        MenuClass.Spells["q"]["combo"].As<MenuBool>().Enabled)
                    {
                        SpellClass.Q.CastOnUnit(heroTarget);
                    }
                    break;
            }
        }

        #endregion
    }
}