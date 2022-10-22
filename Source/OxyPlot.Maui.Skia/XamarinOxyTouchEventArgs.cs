namespace OxyPlot.Maui.Skia
{
    internal class XamarinOxyTouchEventArgs : OxyTouchEventArgs
    {
        public int PointerCount { get; set; }

        public XamarinOxyTouchEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyTouchEventArgs" /> class.
        /// </summary>
        /// <param name="currentTouches">The current touches.</param>
        /// <param name="previousTouches">The previous touches.</param>
        public XamarinOxyTouchEventArgs(ScreenPoint[] currentTouches, ScreenPoint[] previousTouches)
        : base(currentTouches, previousTouches)
        {
            PointerCount = currentTouches.Length;
        }
    }
}