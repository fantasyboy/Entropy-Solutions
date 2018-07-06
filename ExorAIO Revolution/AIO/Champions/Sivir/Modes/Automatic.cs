using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.UI.Components;
using Entropy.SDK.Utils;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Sivir
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Automatic(args)
        {
            /// <summary>
            ///     Block Special AoE.
            /// </summary>
            if (SpellClass.E.Ready)
            {
                var buffList = new[] {"jaxcounterstrike", "kogmawicathiansurprise"};
                foreach (var buff in buffList)
                {
                    foreach (var target in GameObjects.EnemyHeroes)
                    {
                        var getBuff = target.GetBuff(buff);
                        if (getBuff != null)
                        {
                            if (getBuff.GetRemainingBuffTime() <= 350 &&
                                target.Distance(UtilityClass.Player) < 300 + UtilityClass.Player.BoundingRadius &&
                                MenuClass.Spells["e"]["whitelist"][$"{target.CharName.ToLower()}.{buff.ToLower()}"].As<MenuBool>().Enabled)
                            {
                                SpellClass.E.Cast();
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Called while processing Spellcasting operations.
        /// </summary>
        
        /// <param name="args">The <see cref="AIBaseClientMissileClientDataEventArgs" /> instance containing the event data.</param>
        public void Shield(AIBaseClient sender, AIBaseClientMissileClientDataEventArgs args)
        {
            if (sender == null || !sender.IsEnemy()())
            {
                return;
            }

            switch (sender.Type)
            {
                case GameObjectType.AIHeroClient:
                    if (Invulnerable.Check(UtilityClass.Player, DamageType.Magical, false))
                    {
                        return;
                    }

                    var hero = sender as AIHeroClient;
                    if (hero != null)
                    {
                        /// <summary>
                        ///     Block Gangplank's Barrels.
                        /// </summary>
                        if (hero.CharName.Equals("Gangplank"))
                        {
                            if (!(args.Target is AIMinionClient))
                            {
                                return;
                            }

                            if (Bools.IsAutoAttack(args.SpellData.Name) || args.SpellData.Name.Equals("GangplankQProceed"))
                            {
                                var target = (AIMinionClient)args.Target;
                                if ((int)target.GetRealHealth() <= 2 &&
                                    target.Distance(UtilityClass.Player) <= 450 &&
                                    target.CharName.Equals("gangplankbarrel"))
                                {
                                    SpellClass.E.Cast();
                                    return;
                                }
                            }
                        }

                        var spellMenu = MenuClass.Spells["e"]["whitelist"][$"{hero.CharName.ToLower()}.{args.SpellData.Name.ToLower()}"];
                        if (args.Target != null &&
                            args.Target.IsMe())
                        {
                            /// <summary>
                            ///     Check for Special On-Hit CC AutoAttacks.
                            /// </summary>
                            if (Bools.IsAutoAttack(args.SpellData.Name))
                            {
                                switch (args.SpellData.Name)
                                {
                                    case "UdyrBearAttack":
                                    case "GoldCardPreAttack":
                                    case "RedCardPreAttack":
                                    case "BlueCardPreAttack":
                                    case "NautilusRavageStrikeAttack":
                                        if (spellMenu != null &&
                                            spellMenu.As<MenuBool>().Value &&
                                            (!hero.CharName.Equals("Udyr") || !UtilityClass.Player.HasBuff("udyrbearstuncheck")))
                                        {
                                            SpellClass.E.Cast();
                                        }
                                        break;
                                }

                                /// <summary>
                                ///     Check for Melee AutoAttack Resets.
                                /// </summary>
                                var getReset = hero.GetActiveBuffs().FirstOrDefault(b => ImplementationClass.IOrbwalker.IsReset(b.Name));
                                var resetMenu = getReset != null
                                                    ? MenuClass.Spells["e"]["whitelist"][$"{hero.CharName.ToLower()}.{getReset.Name.ToLower()}"]
                                                    : null;
                                if (resetMenu != null &&
                                    resetMenu.As<MenuBool>().Value)
                                {
                                    SpellClass.E.Cast();
                                    return;
                                }

                                /// <summary>
                                ///     Check for Braum Passive.
                                /// </summary>
                                var braumMenu = MenuClass.Spells["e"]["whitelist"]["braum.passive"];
                                if (braumMenu != null &&
                                    braumMenu.As<MenuBool>().Value &&
                                    UtilityClass.Player.GetBuffCount("BraumMark") == 3)
                                {
                                    SpellClass.E.Cast();
                                    return;
                                }
                            }
                        }

                        /// <summary>
                        ///     Shield all the Targetted Spells.
                        /// </summary>
                        var getSpellName = SpellDatabase.GetByName(args.SpellData.Name);
                        if (spellMenu != null &&
                            getSpellName != null &&
                            spellMenu.As<MenuBool>().Value)
                        {
                            switch (getSpellName.SpellType)
                            {
                                /// <summary>
                                ///     Check for Targetted Spells.
                                /// </summary>
                                case SpellType.Targeted:
                                case SpellType.TargetedMissile:
                                    if (args.Target != null && args.Target.IsMe())
                                    {
                                        var delay = MenuClass.Spells["e"]["logical"].As<MenuSliderBool>().Value;
                                        switch (hero.CharName)
                                        {
                                            case "Caitlyn":
                                                delay = 1050;
                                                break;
                                            case "Nocturne":
                                                delay = 350;
                                                break;
                                            case "Zed":
                                                delay = 200;
                                                break;
                                            case "Nautilus":
                                                delay = (int)UtilityClass.Player.Distance(hero);
                                                break;
                                        }

                                        DelayAction.Queue(delay, () =>
                                            {
                                                SpellClass.E.Cast();
                                            });
                                    }
                                    break;

                                /// <summary>
                                ///     Check for AoE Spells.
                                /// </summary>
                                case SpellType.SkillshotCircle:
                                    switch (hero.CharName)
                                    {
                                        case "Alistar":
                                            if (hero.Distance(UtilityClass.Player) <= 300 + UtilityClass.Player.BoundingRadius)
                                            {
                                                SpellClass.E.Cast();
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                    break;

                case GameObjectType.AIMinionClient:
                    /// <summary>
                    ///     Block Dragon/Baron/RiftHerald's AutoAttacks.
                    /// </summary>
                    if (args.Target != null &&
                        args.Target.IsMe() &&
                        sender.CharName.Contains("SRU_Dragon") &&
                        MenuClass.Spells["e"]["whitelist"]["minions"].As<MenuBool>().Value)
                    {
                        SpellClass.E.Cast();
                    }
                    break;
            }
        }

        #endregion
    }
}