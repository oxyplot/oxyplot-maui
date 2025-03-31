namespace OxyPlot.Maui.Skia;

public class MauiOxyTouchEventArgs : OxyTouchEventArgs
{
    public int PointerCount { get; set; }

    public MauiOxyTouchEventArgs()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OxyTouchEventArgs" /> class.
    /// </summary>
    /// <param name="currentTouches">The current touches.</param>
    /// <param name="previousTouches">The previous touches.</param>
    public MauiOxyTouchEventArgs(ScreenPoint[] currentTouches, ScreenPoint[] previousTouches)
        : base(currentTouches, previousTouches)
    {
        PointerCount = currentTouches.Length;
    }
}
