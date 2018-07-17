using System.Collections.Generic;
using System.Linq;

namespace NabbTracker.Utilities
{
    /// <summary>
    ///     The Color convert class.
    /// </summary>
    internal class Colors
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Converts a System.Drawing.Color type into the same color as seen by a colorblind person
        ///     for each type of colorblindness, using the 'FromArgb' function.
        /// </summary>
        public static SharpDX.Color GetRealColor(SharpDX.Color color)
        {
            var colorConvert = new Dictionary<SharpDX.Color, int[,]>
            {
                {SharpDX.Color.Gray,        new [,] {{ 128, 128, 128 }, { 127, 127, 129 }, { 128, 126, 130 }, { 128, 128, 128 }, { 127, 127, 127 }}},
                {SharpDX.Color.Yellow,      new [,] {{ 255, 255, 0   }, { 255, 232, 2   }, { 255, 242, 5   }, { 255, 214, 208 }, { 225, 225, 225 }}},
                {SharpDX.Color.Red,         new [,] {{ 255, 0,   0   }, { 109, 86,  0   }, { 51,  40,  2   }, { 247, 5,   0   }, { 76,  76,  76  }}},
                {SharpDX.Color.Cyan,        new [,] {{ 0,   255, 255 }, { 145, 168, 255 }, { 204, 211, 255 }, { 7,   249, 255 }, { 178, 178, 178 }}},
                {SharpDX.Color.LightGreen,  new [,] {{ 102, 255, 0   }, { 211, 197, 148 }, { 237, 216, 147 }, { 154, 220, 226 }, { 199, 199, 199 }}},
                {SharpDX.Color.Purple,      new [,] {{ 143, 0,   255 }, { 35,  55,  125 }, { 1,   19,  81  }, { 71,  17,  15  }, { 34,  34,  34  }}},
                {SharpDX.Color.Black,       new [,] {{ 0,   0,   0   }, { 0,   0,   0   }, { 0,   0,   0   }, { 0,   0,   0   }, { 0,   0,   0   }}}
            };

            var item = colorConvert.FirstOrDefault(i => i.Key == color);
            var menuListValue = MenuClass.ColorblindMenu["mode"].Value;

            var r = item.Value[menuListValue, 0];
            var g = item.Value[menuListValue, 1];
            var b = item.Value[menuListValue, 2];

            return new SharpDX.Color(r, g, b);
        }

        #endregion
    }
}