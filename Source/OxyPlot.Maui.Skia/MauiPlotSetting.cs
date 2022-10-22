namespace OxyPlot.Maui.Skia
{
    /// <summary>
    /// Plot settings for Maui Skia
    /// </summary>
    public static class MauiPlotSetting
    {
        /// <summary>
        /// Custom Fonts Directory
        /// use for unicode fonts
        /// </summary>
        public static string CustomFontsDirectory { get; set; }

        /// <summary>
        /// Provide SKTypeface
        /// </summary>
        public static Func<string/* FontFamily */, SKTypeface> SKTypefaceProvider { get; set; }
    }
}