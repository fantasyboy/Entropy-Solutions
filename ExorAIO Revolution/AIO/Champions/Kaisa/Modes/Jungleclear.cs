using Entropy;
using AIO.Utilities;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;

#pragma warning disable 1587
namespace AIO.Champions
{
	/// <summary>
	///     The champion class.
	/// </summary>
	internal partial class Kaisa
	{
		#region Public Methods and Operators

		/// <summary>
		///     Called on post attack.
		/// </summary>
		/// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
		public void Jungleclear(OnPostAttackEventArgs args)
		{
			var jungleTarget = args.Target as AIMinionClient;
			if (jungleTarget == null ||
			    !Extensions.GetGenericJungleMinionsTargets().Contains(jungleTarget) ||
			    jungleTarget.GetRealHealth(DamageType.Physical) <
			    UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 2)
			{
				return;
			}

			/// <summary>
			///     The Q Jungleclear Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["jungleclear"]) &&
			    MenuClass.Q["jungleclear"].Enabled)
			{
				SpellClass.Q.Cast();
			}

			/// <summary>
			///     The W Jungleclear Logic.
			/// </summary>
			if (SpellClass.W.Ready &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.W["jungleclear"]) &&
			    MenuClass.W["laneclear"].Enabled)
			{
				SpellClass.W.Cast(jungleTarget);
			}

			/// <summary>
			///     The E Jungleclear Logic.
			/// </summary>
			if (SpellClass.E.Ready &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.E["jungleclear"]) &&
			    MenuClass.E["jungleclear"].Enabled)
			{
				SpellClass.E.Cast();
			}
		}

		#endregion
	}
}