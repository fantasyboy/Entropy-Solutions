using System.Linq;
using Entropy;
using AIO.Utilities;

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
		///     Called on tick update.
		/// </summary>
		public void Killsteal(EntropyEventArgs args)
		{
			/// <summary>
			///     The KillSteal Q Logic.
			/// </summary>
			if (SpellClass.Q.Ready &&
			    MenuClass.Q["killsteal"].Enabled)
			{
				foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q.Range).Where(t =>
					GetQDamage(t, 3) >= t.GetRealHealth()))
				{
					SpellClass.Q.Cast(target);
					break;
				}
			}
		}

		#endregion
	}
}