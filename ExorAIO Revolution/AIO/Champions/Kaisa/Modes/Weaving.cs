using AIO.Utilities;
using Entropy;
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
	    public void Weaving(OnPostAttackEventArgs args)
	    {
		    var heroTarget = args.Target as AIHeroClient;
		    if (heroTarget == null)
		    {
			    return;
		    }

		    /// <summary>
		    ///     The E Weaving Logic.
		    /// </summary>
		    if (SpellClass.E.Ready &&
		        MenuClass.Spells["e"]["combo"].Enabled)
		    {
			    SpellClass.E.Cast();
		    }
	    }

	    #endregion
    }
}