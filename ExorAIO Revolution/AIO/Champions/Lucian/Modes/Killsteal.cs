using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.Extensions.Objects;

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
		///     Fired when the game is updated.
		/// </summary>
		public void Killsteal(EntropyEventArgs args)
		{
			/// <summary>
			///     The Q Killsteal Logics.
			/// </summary>
			if (SpellClass.Q.Ready)
			{
				/// <summary>
				///     Normal.
				/// </summary>
				if (MenuClass.Q["killsteal"].Enabled)
				{
					foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range).Where(t =>
						GetQDamage(t) >= t.GetRealHealth()))
					{
						SpellClass.Q.CastOnUnit(target);
						break;
					}
				}

				/// <summary>
				///     Extended.
				/// </summary>
				if (MenuClass.Q3["killsteal"].Enabled)
				{
					foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q2.Range).Where(t =>
						!t.IsValidTargetEx(SpellClass.Q.Range) &&
						GetQDamage(t) >= t.GetRealHealth()))
					{
						foreach (var minion in Extensions.GetAllGenericUnitTargetsInRange(SpellClass.Q.Range))
						{
							if (QRectangle(minion).IsInsidePolygon(SpellClass.Q.GetPrediction(target).CastPosition))
							{
								SpellClass.Q.CastOnUnit(minion);
								break;
							}
						}
					}
				}
			}

			/// <summary>
			///     The KillSteal W Logic.
			/// </summary>
			if (SpellClass.W.Ready &&
			    MenuClass.W["killsteal"].Enabled)
			{
				foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.W.Range).Where(t =>
					GetWDamage(t) >= t.GetRealHealth()))
				{
					SpellClass.W.Cast(target);
					break;
				}
			}
		}

		#endregion
	}
}