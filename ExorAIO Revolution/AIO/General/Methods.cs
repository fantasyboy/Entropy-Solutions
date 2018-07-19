using Entropy;
using Entropy.SDK.Orbwalking;

#pragma warning disable 1587
namespace AIO
{
	/// <summary>
	///     The general class.
	/// </summary>
	internal partial class General
	{
		#region Public Methods and Operators

		/// <summary>
		///     Loads the methods.
		/// </summary>
		public static void Methods()
		{
			Orbwalker.OnPreAttack += OnPreAttack;
			Orbwalker.OnPostAttack += OnPostAttack;
			Spellbook.OnLocalCastSpell += OnLocalCastSpell;
		}

		#endregion
	}
}