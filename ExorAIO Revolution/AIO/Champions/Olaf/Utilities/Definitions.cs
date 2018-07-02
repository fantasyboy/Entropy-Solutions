// ReSharper disable LoopCanBeConvertedToQuery

using System.Collections.Generic;
using System.Linq;
using Entropy;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The definitions class.
    /// </summary>
    internal partial class Olaf
    {
        #region Fields

        /// <summary>
        ///     Gets the axes.
        /// </summary>
        public Dictionary<int, Vector3> Axes = new Dictionary<int, Vector3>();

        /// <summary>
        ///     Reloads the DarkSpheres.
        /// </summary>
        public void ReloadAxes()
        {
            foreach (var axe in ObjectManager.Get<GameObject>().Where(o => o != null && o.IsValid))
            {
                switch (axe.Name)
                {
                    case "Olaf_Base_Q_Axe_Ally.troy":
                    case "Olaf_Skin04_Q_Axe_Ally.troy":
                        Axes.Add(axe.NetworkId, axe.Position);
                        break;
                }
            }
        }

        #endregion
    }
}