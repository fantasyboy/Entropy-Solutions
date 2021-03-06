using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Geometry;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking;
using Entropy.SDK.Orbwalking.EventArgs;

#pragma warning disable 1587
namespace AIO.Champions
{
	/// <summary>
	///     The champion class.
	/// </summary>
	internal partial class Vayne
	{
		#region Public Methods and Operators

		/// <summary>
		///     Called on do-cast.
		/// </summary>
		/// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
		public void Lasthit(OnPostAttackEventArgs args)
		{
			/// <summary>
			///     The Q FarmHelper Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["farmhelper"]) &&
			    MenuClass.Q["farmhelper"].Enabled)
			{
				var posAfterQ = UtilityClass.Player.Position.Extend(Hud.CursorPositionUnclipped, 300f);
				if (Extensions.GetEnemyLaneMinionsTargetsInRange(SpellClass.Q.Range)
					.Any(m => m.Distance(posAfterQ) < UtilityClass.Player.GetAutoAttackRange() &&
					          m != Orbwalker.GetOrbwalkingTarget() &&
					          posAfterQ.EnemyHeroesCount(UtilityClass.Player.GetAutoAttackRange(m)) <= 2 &&
					          m.HP <= UtilityClass.Player.GetAutoAttackDamage(m) + GetQBonusDamage(m)))
				{
					SpellClass.Q.Cast(Hud.CursorPositionUnclipped);
				}
			}
		}

		#endregion
	}
}