using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Objects;

#pragma warning disable 1587

namespace AIO.Champions
{
	/// <summary>
	///     The logics class.
	/// </summary>
	internal partial class Taliyah
	{
		#region Public Methods and Operators

		/// <summary>
		///     Fired when the game is updated.
		/// </summary>
		public void LaneClear(EntropyEventArgs args)
		{
			/// <summary>
			///     The Laneclear W Logic.
			/// </summary>
			if (SpellClass.W.Ready &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.W.Slot, MenuClass.W["laneclear"]) &&
			    MenuClass.W["laneclear"].Enabled)
			{
				/*
				var farmLocation = SpellClass.W.GetCircularFarmLocation(Extensions.GetEnemyLaneMinionsTargets(), SpellClass.W.Width);
				if (farmLocation.MinionsHit >= MenuClass.W["customization"]["laneclear"].Value)
				{
				    SpellClass.W.Cast(farmLocation.Position);
				}
				*/
			}

			/// <summary>
			///     The Laneclear E Logic.
			/// </summary>
			if (SpellClass.E.Ready &&
			    UtilityClass.Player.MPPercent()
			    > ManaManager.GetNeededMana(SpellClass.E.Slot, MenuClass.E["laneclear"]) &&
			    MenuClass.E["laneclear"].Enabled)
			{
				/*
				var farmLocation = SpellClass.W.GetCircularFarmLocation(Extensions.GetEnemyLaneMinionsTargets(), SpellClass.W.Width);
				if (farmLocation.MinionsHit >= MenuClass.W["customization"]["laneclear"].Value)
				{
				    SpellClass.E.Cast(farmLocation.Position);
				}
				*/
			}

			/// <summary>
			///     The Laneclear Q Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    (IsNearWorkedGround() ||
			     UtilityClass.Player.MPPercent()
			     > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Q["laneclear"])) &&
			    MenuClass.Q["laneclear"].Enabled)
			{
				/*
				var farmLocation = SpellClass.Q.GetCircularFarmLocation(Extensions.GetEnemyLaneMinionsTargets(), SpellClass.Q.Width*2);
				switch (MenuClass.Q["modes"]["laneclear"].Value)
				{
				    case 0:
				        if (!this.IsNearWorkedGround())
				        {
				            SpellClass.Q.Cast(farmLocation.Position);
				        }
				        break;
				    case 1:
				        SpellClass.Q.Cast(farmLocation.Position);
				        break;
				}
				*/
			}
		}

		#endregion
	}
}