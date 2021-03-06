using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Caching;
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
	internal partial class Kalista
	{
		#region Public Methods and Operators

		/// <summary>
		///     Fired when the game is updated.
		/// </summary>
		public void Automatic()
		{
			if (SoulBound == null)
			{
				var possibleSoulBound = ObjectCache.AllyHeroes.FirstOrDefault(a => a.HasBuff("kalistacoopstrikeally"));
				if (possibleSoulBound != null)
				{
					SoulBound = possibleSoulBound;
				}
			}

			if (UtilityClass.Player.IsRecalling())
			{
				return;
			}

			/// <summary>
			///     The Automatic E Logics.
			/// </summary>
			if (SpellClass.E.Ready &&
			    MenuClass.E["beforedeath"].Enabled &&
			    LocalPlayer.Instance.HPPercent() <= MenuClass.E["beforedeath"].Value)
			{
				SpellClass.E.Cast();
			}

			/// <summary>
			///     The R Logics.
			/// </summary>
			if (SpellClass.R.Ready &&
			    SoulBound != null)
			{
				/// <summary>
				///     The Lifesaver R Logic.
				/// </summary>
				if (SoulBound.EnemyHeroesCount(800f) > 0 &&
				    SoulBound.HPPercent() <=
				    MenuClass.R["lifesaver"].Value &&
				    MenuClass.R["lifesaver"].Enabled)
				{
					SpellClass.R.Cast();
				}

				/// <summary>
				///     The Offensive R Logics.
				/// </summary>
				if (RLogics.ContainsKey(SoulBound.CharName))
				{
					var option = RLogics.FirstOrDefault(k => k.Key == SoulBound.CharName).Value;
					var buffName = option.Item1;
					var menuOption = option.Item2;

					var target = ObjectCache.EnemyHeroes.FirstOrDefault(t => t.HasBuff(buffName));
					if (target != null &&
					    MenuClass.R[menuOption].Enabled)
					{
						var buff = target.GetBuff(buffName);
						if (buff.Caster == SoulBound &&
						    target.DistanceToPlayer() >
						    UtilityClass.Player.GetAutoAttackRange(target))
						{
							SpellClass.R.Cast();
						}
					}
				}
			}

			/// <summary>
			///     The Spot W Logic.
			/// </summary>
			if (SpellClass.W.Ready &&
			    !UtilityClass.Player.Position.IsUnderEnemyTurret() &&
			    Orbwalker.Mode == OrbwalkingMode.None &&
			    UtilityClass.Player.EnemyHeroesCount(1500f) == 0 &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.W["logical"]) &&
			    MenuClass.W["logical"].Enabled)
			{
				foreach (var loc in Locations.Where(l =>
					UtilityClass.Player.Distance(l) <= SpellClass.W.Range &&
					!ObjectCache.AllMinions.Any(m => m.Distance(l) <= 1000f && m.ModelName.Equals("KalistaSpawn"))))
				{
					SpellClass.W.Cast(loc);
				}
			}
		}

		#endregion
	}
}