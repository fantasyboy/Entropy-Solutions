using System.Linq;
using Entropy;
using AIO.Utilities;
using Entropy.SDK.UI.Components;
using SharpDX;
using Color = System.Drawing.Color;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The drawings class.
    /// </summary>
    internal partial class MissFortune
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the drawings.
        /// </summary>
        public void Drawings()
        {
            /// <summary>
            ///     Loads the Q drawing.
            /// </summary>
            if (SpellClass.Q.Ready)
            {
                /// <summary>
                ///     Loads the Q drawing.
                /// </summary>
                if (MenuClass.Drawings["q"].As<MenuBool>().Enabled)
                {
                    Render.Circle(UtilityClass.Player.Position, SpellClass.Q.Range, 30, Color.LightGreen);
                }

                /// <summary>
                ///     Loads the Extended Q drawing.
                /// </summary>
                if (MenuClass.Drawings["qcone"].As<MenuBool>().Enabled)
                {
                    foreach (var target in Extensions.GetBestSortedTargetsInRange(SpellClass.Q2.Range))
                    {
                        var unitsToIterate = Extensions.GetAllGenericUnitTargetsInRange(SpellClass.Q.Range)
                            .Where(m => !m.IsMoving && QCone(m).IsInside((Vector2)target.Position))
                            .OrderBy(m => m.HP)
                            .ToList();
                        foreach (var minion in unitsToIterate)
                        {
                            DrawQCone(minion).Draw(QCone(minion).IsInside((Vector2)target.Position) && MenuClass.Spells["q2"]["whitelist"][target.CharName.ToLower()].Enabled
                                ? Color.Green
                                : Color.Red);
                        }
                    }
                }
            }

            /// <summary>
            ///     Loads the E drawing.
            /// </summary>
            if (SpellClass.E.Ready &&
                MenuClass.Drawings["e"].As<MenuBool>().Enabled)
            {
                Render.Circle(UtilityClass.Player.Position, SpellClass.E.Range, 30, Color.Cyan);
            }

            /// <summary>
            ///     Loads the R drawing.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Drawings["r"].As<MenuBool>().Enabled)
            {
                Render.Circle(UtilityClass.Player.Position, SpellClass.R.Range, 30, Color.Red);
            }

            /// <summary>
            ///     Loads the Passive drawing.
            /// </summary>
            if (MenuClass.Drawings["passivetarget"].As<MenuBool>().Enabled)
            {
                var target = ObjectManager.Get<AIBaseClient>().FirstOrDefault(u => u.NetworkID == LoveTapTargetNetworkId);
                if (target != null)
                {
                    Render.Circle(target.Position, target.BoundingRadius, 30, Color.Black);
                }
            }
        }

        #endregion
    }
}