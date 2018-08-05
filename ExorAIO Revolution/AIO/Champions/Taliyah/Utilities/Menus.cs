using System.Linq;
using AIO.Utilities;
using Entropy.SDK.Caching;
using Entropy.SDK.Events;
using Entropy.SDK.UI;
using Entropy.SDK.UI.Components;
using Gapcloser = AIO.Utilities.Gapcloser;

#pragma warning disable 1587

namespace AIO.Champions
{
	/// <summary>
	///     The menu class.
	/// </summary>
	internal partial class Taliyah
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
				MenuClass.Q.Add(new MenuBool("combo", "Combo"));
				MenuClass.Q.Add(new MenuBool("killsteal", "KillSteal"));
				MenuClass.Q.Add(new MenuSeperator("qseparator", "ManaManager ignored when using single Q"));
				MenuClass.Q.Add(new MenuSliderBool("harass", "Harass / if Mana >= x%", true, 50, 0, 99));
				MenuClass.Q.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 75, 0, 99));
				MenuClass.Q.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

				/// <summary>
				///     Sets the customization menu for the Q spell.
				/// </summary>
				MenuClass.Q2 = new Menu("customization", "Customization:");
				{
					MenuClass.Q2.Add(new MenuSlider("laneclear", "Laneclear / if hittable minions >= x%", 3, 1, 10));
				}
				MenuClass.Q.Add(MenuClass.Q2);

				/// <summary>
				///     Sets the modes menu for the Q spell.
				/// </summary>
				MenuClass.Q3 = new Menu("modes", "Modes:");
				{
					MenuClass.Q3.Add(new MenuList("combo", "Combo Mode:", new[] {"Only Full", "Full + Partial"}, 1));
					MenuClass.Q3.Add(new MenuList("harass", "Harass Mode:", new[] {"Only Full", "Full + Partial"}, 1));
					MenuClass.Q3.Add(new MenuList("laneclear", "Laneclear Mode:", new[] {"Only Full", "Full + Partial"},
						1));
					MenuClass.Q3.Add(new MenuList("jungleclear", "Jungleclear Mode:",
						new[] {"Only Full", "Full + Partial"}, 1));
				}
				MenuClass.Q.Add(MenuClass.Q3);

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
				/// <summary>
				///     Sets the customization menu for the W spell.
				/// </summary>
				MenuClass.W2 = new Menu("customization", "Customization:");
				{
					//MenuClass.W2.Add(new MenuSeperator("separator1", "General settings:"));
					MenuClass.W2.Add(
						new MenuBool("onlyeready", "Combo: Only W to W->E combo").SetToolTip(
							"Unless to Hit boulders on the ground if any"));
					//MenuClass.W2.Add(new MenuSeperator("separator2"));
					//MenuClass.W2.Add(new MenuSeperator("separator3", "Laneclear settings:"));
					MenuClass.W2.Add(new MenuSlider("laneclear", "Laneclear / if hittable minions >= x%", 3, 1, 10));
				}
				MenuClass.W.Add(MenuClass.W2);

				if (ObjectCache.EnemyHeroes.Any())
				{
					/// <summary>
					///     Sets the menu for the selection.
					/// </summary>
					MenuClass.WhiteList = new Menu("selection", "Combo: Pull / Push Selection");
					{
						foreach (var enemy in ObjectCache.EnemyHeroes)
						{
							MenuClass.WhiteList.Add(
								new MenuList(
									enemy.CharName.ToLower(),
									enemy.CharName,
									new[]
									{
										"Always Pull", "Always Push", "Pull if Killable else Push",
										"Pull if not near else Push", "Ignore if possible"
									}, 3));
						}
					}
					MenuClass.W.Add(MenuClass.WhiteList);
				}
				else
				{
					MenuClass.W.Add(new MenuSeperator("exseparator", "Combo: Pull / Push Selection not needed."));
				}

				MenuClass.W.Add(new MenuBool("combo", "Combo"));
				MenuClass.W.Add(new MenuBool("boulders", "To boulders"));
				MenuClass.W.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 75, 0, 99));
				MenuClass.W.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));
				MenuClass.W.Add(new MenuSeperator("separator4"));

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

				if (ObjectCache.EnemyHeroes.Any(t => Interrupter.SpellDatabase.Keys.Contains(t.CharName)))
				{
					/// <summary>
					///     Sets the menu for the Interrupter W.
					/// </summary>
					MenuClass.Interrupter = new Menu("interrupter", "Interrupter");
					{
						MenuClass.Interrupter.Add(new MenuBool("enabled", "Enable"));
						MenuClass.Interrupter.Add(new MenuSeperator(string.Empty));
						MenuClass.W.Add(MenuClass.Interrupter);

						foreach (var enemy in ObjectCache.EnemyHeroes.Where(t =>
							Interrupter.SpellDatabase.Keys.Contains(t.CharName)))
						{
							MenuClass.SubInterrupter = new Menu(enemy.CharName.ToLower(), enemy.CharName);
							{
								foreach (var list in Interrupter.SpellDatabase.Where(x => x.Key == enemy.CharName))
								{
									foreach (var spell in list.Value)
									{
										MenuClass.SubInterrupter.Add(new MenuBool(
											$"{enemy.CharName.ToLower()}.{spell.SpellSlot.ToString().ToLower()}",
											$"Interrupt: {spell.SpellSlot}"));
									}
								}
							}
							MenuClass.Interrupter.Add(MenuClass.SubInterrupter);
						}
					}
				}
				else
				{
					MenuClass.W.Add(new MenuSeperator("interrupternotneeded", "Interrupter not needed"));
				}

				MenuClass.W.Add(new MenuSeperator("separator5"));
				MenuClass.W.Add(new MenuBool("interrupter", "Interrupt Enemy Channels"));
			}
			MenuClass.Root.Add(MenuClass.W);

			/// <summary>
			///     Sets the menu for the E.
			/// </summary>
			MenuClass.E = new Menu("e", "Use E to:");
			{
				/// <summary>
				///     Sets the customization menu for the E spell.
				/// </summary>
				MenuClass.E2 = new Menu("customization", "Customization:");
				{
					MenuClass.E2.Add(new MenuBool("onlywready", "Combo: Don't Cast E if W on cooldown"));
					//MenuClass.E2.Add(new MenuSeperator("separator3", "Laneclear settings:"));
					MenuClass.E2.Add(new MenuSlider("laneclear", "Laneclear / if hittable minions >= x%", 3, 1, 10));
				}
				MenuClass.E.Add(MenuClass.E2);

				MenuClass.E.Add(new MenuBool("combo", "Combo"));
				MenuClass.E.Add(new MenuBool("teleports", "On Teleports"));
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
				MenuClass.E.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
				MenuClass.E.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));
			}
			MenuClass.Root.Add(MenuClass.E);

			/// <summary>
			///     Sets the menu for the R.
			/// </summary>
			MenuClass.R = new Menu("r", "Use R to:");
			{
				MenuClass.R.Add(new MenuBool("mountr", "Automatically Mount on R"));
			}
			MenuClass.Root.Add(MenuClass.R);

			/// <summary>
			///     Sets the menu for the drawings.
			/// </summary>
			MenuClass.Drawings = new Menu("drawings", "Drawings");
			{
				MenuClass.Drawings.Add(new MenuBool("q", "Q Range"));
				MenuClass.Drawings.Add(new MenuBool("w", "W Range", false));
				MenuClass.Drawings.Add(new MenuBool("e", "E Range", false));
				MenuClass.Drawings.Add(new MenuBool("r", "R Range", false));
				MenuClass.Drawings.Add(new MenuBool("rmm", "R Minimap Range"));
				MenuClass.Drawings.Add(new MenuBool("boulders", "Draw Boulders", false));
				MenuClass.Drawings.Add(new MenuBool("grounds", "Draw Worked Grounds", false));
			}
			MenuClass.Root.Add(MenuClass.Drawings);

			MenuClass.Root.Add(new MenuList("pattern", "Combo Pattern", new[] { "W->E", "E->W" }));
		}

		#endregion
	}
}