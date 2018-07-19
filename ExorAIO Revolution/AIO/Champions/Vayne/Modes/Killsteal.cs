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
	internal partial class Vayne
	{
		#region Public Methods and Operators

		/// <summary>
		///     Fired when the game is updated.
		/// </summary>
		public void Killsteal(EntropyEventArgs args)
		{
			/// <summary>
			///     The E KillSteal Logic.
			/// </summary>
			if (SpellClass.E.Ready &&
			    MenuClass.E["killsteal"].Enabled)
			{
				foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.E.Range)
					.Where(t => GetEDamage(t) + (t.GetBuffCount("vaynesilvereddebuff") == 2 ? GetWDamage(t) : 0) >=
					            t.GetRealHealth(DamageType.Physical)))
				{
					SpellClass.E.CastOnUnit(target);
					break;
				}
			}
		}

		#endregion
	}
}