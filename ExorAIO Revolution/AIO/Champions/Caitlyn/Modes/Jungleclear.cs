using AIO.Utilities;
using Entropy;
using Entropy.SDK.Damage;
using Entropy.SDK.Extensions.Objects;
using Entropy.SDK.Orbwalking.EventArgs;

#pragma warning disable 1587

namespace AIO.Champions
{
	/// <summary>
	///     The champion class.
	/// </summary>
	internal partial class Caitlyn
	{
		#region Public Methods and Operators

		/// <summary>
		///     Called on do-cast.
		/// </summary>
		/// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
		public void Jungleclear(OnPostAttackEventArgs args)
		{
			var jungleTarget = args.Target as AIMinionClient;
			if (jungleTarget == null ||
			    !Extensions.GetGenericJungleMinionsTargets().Contains(jungleTarget) ||
			    jungleTarget.HP < UtilityClass.Player.GetAutoAttackDamage(jungleTarget) * 2)
			{
				return;
			}

			/// <summary>
			///     The Jungleclear E Logics.
			/// </summary>
			if (SpellClass.E.Ready &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.E["jungleclear"]) &&
			    MenuClass.E["jungleclear"].Enabled)
			{
				SpellClass.E.Cast(jungleTarget);
				return;
			}

			/// <summary>
			///     The Jungleclear Q Logics.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["jungleclear"]) &&
			    MenuClass.Q["jungleclear"].Enabled)
			{
				SpellClass.Q.Cast(jungleTarget);
			}
		}

		#endregion
	}
}