using System.Linq;
using AIO.Utilities;
using Entropy;
using Entropy.SDK.Caching;
using Entropy.SDK.UI;
using Entropy.SDK.UI.Components;

#pragma warning disable 1587

namespace AIO.Champions
{
	/// <summary>
	///     The menu class.
	/// </summary>
	internal partial class Caitlyn
	{
		#region Public Methods and Operators

		/// <summary>
		///     Initializes the menus.
		/// </summary>
		public void Menus()
		{
			/// <summary>
			///     Sets the menu for the Q.
			/// </summary>
			MenuClass.Q = new Menu("q", "Use Q to:");
			{
				MenuClass.Q.Add(new MenuBool("logical", "On Trapped Enemies"));
				MenuClass.Q.Add(new MenuBool("killsteal", "Killsteal"));
				MenuClass.Q.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
				MenuClass.Q.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
				MenuClass.Q.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

				/// <summary>
				///     Sets the customization menu for the Q spell.
				/// </summary>
				MenuClass.Q2 = new Menu("customization", "Customization:");
				{
					if (ObjectCache.EnemyHeroes.Any())
					{
						MenuClass.Q2.Add(new MenuSlider("safeq", "Combo: Only Q if enemies in AA Range <= x", 1, 0,
							ObjectCache.EnemyHeroes.Count()));
					}
					else
					{
						MenuClass.Q2.Add(
							new MenuSeperator("exseparator", "Combo: Only Q if enemies / No enemies found"));
					}

					//MenuClass.Q2.Add(new MenuSeperator("separator1", "Laneclear settings:"));
					MenuClass.Q2.Add(new MenuSlider("laneclear", "Laneclear / if hittable minions >= x%", 4, 1, 10));
				}
				MenuClass.Q.Add(MenuClass.Q2);

				if (ObjectCache.EnemyHeroes.Any())
				{
					/// <summary>
					///     Sets the menu for the Q Whitelist.
					/// </summary>
					MenuClass.WhiteList = new Menu("whitelist", "Harass: Whitelist");
					{
						foreach (var target in ObjectCache.EnemyHeroes)
						{
							MenuClass.WhiteList.Add(new MenuBool(target.CharName.ToLower(),
								"Harass: " + target.CharName));
						}
					}
					MenuClass.Q.Add(MenuClass.WhiteList);
				}
				else
				{
					MenuClass.Q.Add(new MenuSeperator("exseparator", "Whitelist not needed"));
				}
			}
			MenuClass.Root.Add(MenuClass.Q);

			/// <summary>
			///     Sets the menu for the W.
			/// </summary>
			MenuClass.W = new Menu("w", "Use W to:");
			{
				MenuClass.W.Add(new MenuBool("triplecombo", "Try Double/Triple HeadShot Combo"));
				MenuClass.W.Add(new MenuBool("logical", "On Hard-CC'd/Stasis Enemies"));
				MenuClass.W.Add(new MenuBool("teleport", "On Teleport"));
				MenuClass.W.Add(new MenuSeperator("separator"));

				if (ObjectCache.EnemyHeroes.Any(x => Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
				{
					/// <summary>
					///     Sets the menu for the Anti-Gapcloser W.
					/// </summary>
					MenuClass.Gapcloser = new Menu("gapcloser", "Anti-Gapcloser");
					{
						MenuClass.Gapcloser.Add(new MenuBool("enabled", "Enable"));
						MenuClass.Gapcloser.Add(new MenuSeperator(string.Empty));
						MenuClass.W.Add(MenuClass.Gapcloser);

						foreach (var enemy in ObjectCache.EnemyHeroes.Where(x =>
							Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
						{
							MenuClass.SubGapcloser = new Menu(enemy.CharName.ToLower(), enemy.CharName);
							{
								foreach (var spell in Gapcloser.Spells.Where(x => x.ChampionName == enemy.CharName))
								{
									MenuClass.SubGapcloser.Add(new MenuBool(
										$"{enemy.CharName.ToLower()}.{spell.SpellName.ToLower()}",
										$"Slot: {spell.Slot} ({spell.SpellName})"));
								}
							}
							MenuClass.Gapcloser.Add(MenuClass.SubGapcloser);
						}
					}
				}
				else
				{
					MenuClass.W.Add(new MenuSeperator(string.Empty, "Anti-Gapcloser not needed"));
				}
			}
			MenuClass.Root.Add(MenuClass.W);

			/// <summary>
			///     Sets the menu for the E.
			/// </summary>
			MenuClass.E = new Menu("e", "Use E to:");
			{
				MenuClass.E.Add(new MenuBool("combo", "Combo"));
				MenuClass.E.Add(new MenuSeperator("separator"));

				if (ObjectCache.EnemyHeroes.Any(x => Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
				{
					/// <summary>
					///     Sets the menu for the Anti-Gapcloser E.
					/// </summary>
					MenuClass.Gapcloser2 = new Menu("gapcloser", "Anti-Gapcloser");
					{
						MenuClass.Gapcloser2.Add(new MenuBool("enabled", "Enable"));
						MenuClass.Gapcloser2.Add(new MenuSeperator(string.Empty));
						MenuClass.E.Add(MenuClass.Gapcloser2);

						foreach (var enemy in ObjectCache.EnemyHeroes.Where(x =>
							Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
						{
							MenuClass.SubGapcloser2 = new Menu(enemy.CharName.ToLower(), enemy.CharName);
							{
								foreach (var spell in Gapcloser.Spells.Where(x => x.ChampionName == enemy.CharName))
								{
									MenuClass.SubGapcloser2.Add(new MenuBool(
										$"{enemy.CharName.ToLower()}.{spell.SpellName.ToLower()}",
										$"Slot: {spell.Slot} ({spell.SpellName})"));
								}
							}
							MenuClass.Gapcloser2.Add(MenuClass.SubGapcloser2);
						}
					}
				}
				else
				{
					MenuClass.E.Add(new MenuSeperator(string.Empty, "Anti-Gapcloser not needed"));
				}

				MenuClass.E.Add(new MenuSeperator("separator2"));
				MenuClass.E.Add(new MenuBool("interrupter", "Interrupt Channelling Targets"));
				MenuClass.E.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

				/// <summary>
				///     Sets the customization menu for the E spell.
				/// </summary>
				MenuClass.E2 = new Menu("customization", "Customization:");
				{
					if (ObjectCache.EnemyHeroes.Any())
					{
						MenuClass.E2.Add(new MenuSlider("safee", "Combo: Only E if enemies in Dash Position <= x", 1, 0,
							ObjectCache.EnemyHeroes.Count()));
					}
					else
					{
						MenuClass.E2.Add(
							new MenuSeperator("exseparator", "Combo: Only E if enemies / No enemies found"));
					}
				}
				MenuClass.E.Add(MenuClass.E2);
			}
			MenuClass.Root.Add(MenuClass.E);

			/// <summary>
			///     Sets the menu for the R.
			/// </summary>
			MenuClass.R = new Menu("r", "Use R to:");
			{
				//MenuClass.R.Add(new MenuSeperator("separator1", "It will ult the lowest on health,"));
				//MenuClass.R.Add(new MenuSeperator("separator2", "whitelisted and non-invulnerable enemy in range."));
				MenuClass.R.Add(new MenuBool("bool", "Semi-Automatic R"));
				MenuClass.R.Add(new MenuKeyBind("key", "Key:", WindowMessageWParam.U, KeybindType.Hold));

				if (ObjectCache.EnemyHeroes.Any())
				{
					/// <summary>
					///     Sets the menu for the R Whitelist.
					/// </summary>
					MenuClass.WhiteList2 = new Menu("whitelist", "Ultimate: Whitelist");
					{
						foreach (var target in ObjectCache.EnemyHeroes)
						{
							MenuClass.WhiteList2.Add(new MenuBool(target.CharName.ToLower(),
								"Use against: " + target.CharName));
						}
					}
					MenuClass.R.Add(MenuClass.WhiteList2);
				}
				else
				{
					MenuClass.R.Add(new MenuSeperator("exseparator", "Whitelist not needed"));
				}
			}
			MenuClass.Root.Add(MenuClass.R);

			/// <summary>
			///     Sets the miscellaneous menu.
			/// </summary>
			MenuClass.Miscellaneous = new Menu("miscellaneous", "Miscellaneous");
			{
				MenuClass.Miscellaneous.Add(new MenuBool("reversede",
					"Reverse E if manually casted (Dash to Cursor Position)", false));
			}
			MenuClass.Root.Add(MenuClass.Miscellaneous);

			/// <summary>
			///     Sets the menu for the drawings.
			/// </summary>
			MenuClass.Drawings = new Menu("drawings", "Drawings");
			{
				MenuClass.Drawings.Add(new MenuBool("q", "Q Range", false));
				MenuClass.Drawings.Add(new MenuBool("w", "W Range", false));
				MenuClass.Drawings.Add(new MenuBool("e", "E Range", false));
				MenuClass.Drawings.Add(new MenuBool("r", "R Range", false));
				MenuClass.Drawings.Add(new MenuBool("rmm", "R Minimap Range", false));
				MenuClass.Drawings.Add(new MenuBool("passivedmg", "Headshot Damage"));
				MenuClass.Drawings.Add(new MenuBool("rdmg", "R Damage"));
			}
			MenuClass.Root.Add(MenuClass.Drawings);
		}

		#endregion
	}
}