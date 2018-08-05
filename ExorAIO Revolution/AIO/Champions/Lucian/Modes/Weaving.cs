using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;
using SharpDX;
using Entropy.SDK.Caching;

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
			    MenuClass.E2["onlyeifmouseoutaarange"].Enabled)
			{
				return;
			}

			var posAfterE = UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, 300f);
			if (ObjectCache.EnemyHeroes.Count() > 1)
			{
				if (MenuClass.E2["erangecheck"].Enabled &&
				    posAfterE.EnemyHeroesCount(UtilityClass.Player.GetAutoAttackRange() +
				                               UtilityClass.Player.BoundingRadius) >= MenuClass.E2["erangecheck"].Value)
				{
					return;
				}
			}

			if (posAfterE.Distance(target) >
			    UtilityClass.Player.GetAutoAttackRange(target) &&
			    MenuClass.E2["noeoutaarange"].Enabled)
			{
				return;
			}

			if (posAfterE.IsUnderEnemyTurret() &&
			    MenuClass.E2["noeturret"].Enabled)
			{
				return;
			}

			switch (MenuClass.E["mode"].Value)
			{
				case 0:
					Vector3 point;
					if (UtilityClass.Player.Position.IsUnderEnemyTurret() ||
					    UtilityClass.Player.Distance(Hud.CursorPositionUnclipped) <
					    UtilityClass.Player.GetAutoAttackRange())
					{
						point = UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped,
							UtilityClass.Player.BoundingRadius);
					}
					else
					{
						point = UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, 475f);
					}

					SpellClass.E.Cast(point);
					break;

				case 1:
					SpellClass.E.Cast(UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, 425f));
					break;

				case 2:
					SpellClass.E.Cast(UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped,
						UtilityClass.Player.BoundingRadius));
					break;
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
					    MenuClass.E["combo"].Enabled)
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
					    MenuClass.Q["combo"].Enabled)
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
					    MenuClass.W["combo"].Enabled)
					{
						SpellClass.W.Cast(heroTarget.Position);
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
					    MenuClass.Q["combo"].Enabled)
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
					    MenuClass.E["combo"].Enabled)
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
					    MenuClass.W["combo"].Enabled)
					{
						SpellClass.W.Cast(heroTarget.Position);
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
					    MenuClass.W["combo"].Enabled)
					{
						SpellClass.W.Cast(heroTarget.Position);
					}
					break;

				case 2:
				case 3:
					/// <summary>
					///     The Q Combo Logic.
					/// </summary>
					if (SpellClass.Q.Ready &&
					    MenuClass.Q["combo"].Enabled)
					{
						SpellClass.Q.CastOnUnit(heroTarget);
					}
					break;
			}
		}

		#endregion
	}
}