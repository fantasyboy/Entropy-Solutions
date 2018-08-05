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
	internal partial class Lucian
	{
		#region Public Methods and Operators

		/// <summary>
		///     Sets the menu.
		/// </summary>
		public void Menus()
		{
			/// <summary>
			///     Sets the combo pattern.
			/// </summary>
			MenuClass.Root.Add(new MenuList("pattern", "Combo Pattern",
				new[] {"E->Q->W", "Q->E->W", "E->W->Q", "W->E->Q"}));

			/// <summary>
			///     Sets the menu for the Q.
			/// </summary>
			MenuClass.Q = new Menu("q", "Use Q to:");
			{
				MenuClass.Q.Add(new MenuBool("combo", "Combo"));
				MenuClass.Q.Add(new MenuBool("killsteal", "Killsteal"));
				MenuClass.Q.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
				MenuClass.Q.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));

				/// <summary>
				///     Sets the customization menu for the Q spell.
				/// </summary>
				MenuClass.Q2 = new Menu("customization", "Customization:");
				{
					//MenuClass.Q2.Add(new MenuSeperator("separator1", "Laneclear settings:"));
					MenuClass.Q2.Add(new MenuSlider("laneclear", "Laneclear / if hittable minions >= x%", 3, 1, 10));
				}
				MenuClass.Q.Add(MenuClass.Q2);
			}
			MenuClass.Root.Add(MenuClass.Q);

			/// <summary>
			///     Sets the Extended Q menu.
			/// </summary>
			MenuClass.Q3 = new Menu("q2", "Use Extended Q in:");
			{
				MenuClass.Q3.Add(new MenuBool("killsteal", "Killsteal"));
				MenuClass.Q3.Add(new MenuSliderBool("mixed", "Harass / if Mana >= %", true, 50, 0, 99));
				MenuClass.Q3.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= %", true, 50, 0, 99));

				if (ObjectCache.EnemyHeroes.Any())
				{
					/// <summary>
					///     Sets the Whitelist menu for the Extended Q.
					/// </summary>
					MenuClass.WhiteList = new Menu("whitelist", "Extended Harass: Whitelist");
					{
						//MenuClass.WhiteList.Add(new MenuSeperator("extendedsep", "Note: The Whitelist only works for Mixed and Laneclear."));
						foreach (var target in ObjectCache.EnemyHeroes)
						{
							MenuClass.WhiteList.Add(new MenuBool(target.CharName.ToLower(),
								"Harass: " + target.CharName));
						}
					}
					MenuClass.Q3.Add(MenuClass.WhiteList);
				}
				else
				{
					MenuClass.Q3.Add(new MenuSeperator("exseparator", "Whitelist not needed"));
				}
			}
			MenuClass.Root.Add(MenuClass.Q3);

			/// <summary>
			///     Sets the menu for the W.
			/// </summary>
			MenuClass.W = new Menu("w", "Use W to:");
			{
				MenuClass.W.Add(new MenuBool("combo", "Combo"));
				MenuClass.W.Add(new MenuBool("killsteal", "Killsteal"));
				MenuClass.W.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
				MenuClass.W.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));
				MenuClass.W.Add(new MenuSliderBool("buildings", "To buildings / if Mana >= x%", true, 50, 0, 99));

				/// <summary>
				///     Sets the customization menu for the W spell.
				/// </summary>
				MenuClass.W2 = new Menu("customization", "Customization:");
				{
					//MenuClass.W2.Add(new MenuSeperator("separator1", "Laneclear settings:"));
					MenuClass.W2.Add(new MenuSlider("laneclear", "Laneclear / if hittable minions >= x%", 3, 1, 10));
				}
				MenuClass.W.Add(MenuClass.W2);
			}
			MenuClass.Root.Add(MenuClass.W);

			/// <summary>
			///     Sets the menu for the E.
			/// </summary>
			MenuClass.E = new Menu("e", "Use E to:");
			{
				MenuClass.E.Add(new MenuList("mode", "E Mode", new[] {"Dynamic", "Long", "Short"}));

				/// <summary>
				///     Sets the customization menu for the E spell.
				/// </summary>
				MenuClass.E2 = new Menu("customization", "Customization:");
				{
					MenuClass.E2.Add(new MenuBool("noeoutaarange", "Don't E out of AA range"));
					MenuClass.E2.Add(new MenuBool("onlyeifmouseoutaarange", "Only if mouse out AA Range", false));

					var count = ObjectCache.EnemyHeroes.Count();
					if (count > 1)
					{
						MenuClass.E2.Add(new MenuSliderBool("erangecheck", "Don't E if X enemies in range from pos",
							true, count >= 3 ? 3 : count, 2, count));
					}
					else
					{
						MenuClass.E2.Add(new MenuSeperator("exseparator",
							"Don't E if >= / No enemies found, no need for a position range check."));
					}

					MenuClass.E2.Add(new MenuBool("noeturret", "Don't use E under Enemy Turret"));
				}
				MenuClass.E.Add(MenuClass.E2);

				MenuClass.E.Add(new MenuSeperator("separator"));
				MenuClass.E.Add(new MenuBool("combo", "Combo"));
				MenuClass.E.Add(new MenuBool("engage", "Engage"));
				MenuClass.E.Add(new MenuSeperator("separator2"));

				/// <summary>
				///     Sets the menu for the Anti-Gapcloser E.
				/// </summary>
				MenuClass.Gapcloser = new Menu("gapcloser", "Anti-Gapcloser");
				{
					if (ObjectCache.EnemyHeroes.Any(x =>
						x.IsMelee && Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
					{
						/// <summary>
						///     Sets the menu for the Anti-Gapcloser E.
						/// </summary>
						MenuClass.Gapcloser = new Menu("gapcloser", "Anti-Gapcloser");
						{
							MenuClass.Gapcloser.Add(new MenuBool("enabled", "Enable"));
							MenuClass.Gapcloser.Add(new MenuSeperator(string.Empty));
							MenuClass.E.Add(MenuClass.Gapcloser);

							foreach (var enemy in ObjectCache.EnemyHeroes.Where(x =>
								x.IsMelee && Gapcloser.Spells.Any(spell => x.CharName == spell.ChampionName)))
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
						MenuClass.E.Add(new MenuSeperator(string.Empty, "Anti-Gapcloser not needed"));
					}
				}

				MenuClass.E.Add(new MenuSeperator("separator3"));
				MenuClass.E.Add(new MenuSliderBool("laneclear", "Laneclear / if Mana >= x%", true, 50, 0, 99));
				MenuClass.E.Add(new MenuSliderBool("jungleclear", "Jungleclear / if Mana >= x%", true, 50, 0, 99));
				MenuClass.E.Add(new MenuSliderBool("buildings", "To buildings / if Mana >= x%", true, 50, 0, 99));
			}
			MenuClass.Root.Add(MenuClass.E);

			/// <summary>
			///     Sets the menu for the R.
			/// </summary>
			MenuClass.R = new Menu("r", "Use R to:");
			{
				//MenuClass.R.Add(new MenuSeperator("separator", "How does it work:"));
				//MenuClass.R.Add(new MenuSeperator("separator2", "Keep the button pressed until you want to stop the ultimate."));
				//MenuClass.R.Add(new MenuSeperator("separator3", "You don't have to press both Spacebar and the Semi-Automatic key."));
				//MenuClass.R.Add(new MenuSeperator("separator4", "It automatically orbwalks while using his R, so just press the key."));

				MenuClass.R.Add(new MenuBool("bool", "Semi-Automatic R", false));
				MenuClass.R.Add(new MenuKeyBind("key", "Key:", WindowMessageWParam.T, KeybindType.Hold));

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
			///     Sets the drawings menu.
			/// </summary>
			MenuClass.Drawings = new Menu("drawings", "Drawings");
			{
				MenuClass.Drawings.Add(new MenuBool("q", "Q Range"));
				MenuClass.Drawings.Add(new MenuBool("qextended", "Extended Q Range"));
				MenuClass.Drawings.Add(new MenuBool("w", "W Range", false));
				MenuClass.Drawings.Add(new MenuBool("e", "E Range", false));
				MenuClass.Drawings.Add(new MenuBool("r", "R Range", false));
			}
			MenuClass.Root.Add(MenuClass.Drawings);
		}

		#endregion
	}
}