using OxyPlot.Maui.Skia.Core;

namespace OxyPlot.Maui.Skia
{
    public class PlotController : ControllerBase, IPlotController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotController" /> class.
        /// </summary>
        public PlotController()
        {
            var cmd = new CompositeDelegateViewCommand<OxyTouchEventArgs>(
                PlotCommands.SnapTrackTouch,
                PlotCommands.PanZoomByTouch
                );

            this.BindTouchDown(cmd);

#if WINDOWS
            this.BindMouseWheel(OxyPlot.PlotCommands.ZoomWheel);
            this.BindMouseWheel(OxyModifierKeys.Control, OxyPlot.PlotCommands.ZoomWheelFine);
#endif
        }
    }
}