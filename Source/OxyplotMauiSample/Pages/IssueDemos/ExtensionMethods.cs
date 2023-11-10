// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods related to OxyPlot and Maui.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using OxyPlot;

namespace OxyplotMauiSample
{
    /// <summary>
    /// Provides extension methods related to OxyPlot and Maui.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts from <see cref="T:OxyColor"/> to <see cref="T:Color"/>.
        /// </summary>
        /// <param name="c">The color to convert.</param>
        /// <returns>The converted color.</returns>
        public static Color ToMauiForms(this OxyColor c)
        {
            return Color.FromRgba(c.R, c.G, c.B, c.A);
        }

        /// <summary>
        /// Converts from <see cref="T:Color"/> to <see cref="T:OxyColor"/>.
        /// </summary>
        /// <param name="c">The color to convert.</param>
        /// <returns>The converted color.</returns>
        public static OxyColor ToOxyColor(this Color c)
        {
            return OxyColor.FromArgb((byte)(c.Alpha * 255), (byte)(c.Red * 255), (byte)(c.Green * 255), (byte)(c.Blue * 255));
        }
    }
}
