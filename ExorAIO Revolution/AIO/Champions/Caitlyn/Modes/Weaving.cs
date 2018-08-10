using AIO.Utilities;
using Entropy;
using Entropy.SDK.Extensions.Geometry;
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
		///     Called on post attack.
		/// </summary>
		/// <param name="args">The <see cref="OnPostAttackEventArgs" /> instance containing the event data.</param>
		public void Weaving(OnPostAttackEventArgs args)
		{
			var heroTarget = args.Target as AIHeroClient;
			if (heroTarget == null)
			{
				return;
			}

			/// <summary>
			///     The E Combo Logic.
			/// </summary>
			if (SpellClass.E.Ready &&
			    !Invulnerable.Check(heroTarget) &&
			    MenuClass.E["combo"].Enabled)
			{
				if (heroTarget.IsValidTarget(SpellClass.E.Range) &&
				    heroTarget.Distance(SpellClass.E.GetPrediction(heroTarget).CastPosition) < SpellClass.E.Range)
				{
					SpellClass.E.Cast(heroTarget);
				}
			}
		}

		#endregion
	}
}