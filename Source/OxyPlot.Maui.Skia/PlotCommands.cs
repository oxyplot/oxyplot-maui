namespace OxyPlot.Maui.Skia
{
    public class PlotCommands
    {
        /// <summary>
        /// Gets the pan/zoom touch command.
        /// </summary>
        public static IViewCommand<OxyTouchEventArgs> PanZoomByTouch { get; private set; }

        /// <summary>
        /// Gets the pan(axis only)/zoom touch command.
        /// </summary>
        public static IViewCommand<OxyTouchEventArgs> PanZoomByTouchAxisOnly { get; private set; }

        /// <summary>
        /// Gets the pan(by two finger)/zoom touch command.
        /// </summary>
        public static IViewCommand<OxyTouchEventArgs> PanZoomByTouchTwoFinger { get; private set; }

        /// <summary>
        /// Gets the snap tracker command.
        /// </summary>
        public static IViewCommand<OxyTouchEventArgs> SnapTrackTouch { get; private set; }

        static PlotCommands()
        {
            PanZoomByTouch = new DelegatePlotCommand<OxyTouchEventArgs>((view, controller, args) => controller.AddTouchManipulator(view, new Manipulators.TouchManipulator(view), args));
            PanZoomByTouchAxisOnly = new DelegatePlotCommand<OxyTouchEventArgs>((view, controller, args) => controller.AddTouchManipulator(view, new Manipulators.TouchManipulator(view) { IsOnlyAcceptAxisPan = true }, args));
            PanZoomByTouchTwoFinger = new DelegatePlotCommand<OxyTouchEventArgs>((view, controller, args) => controller.AddTouchManipulator(view, new Manipulators.TouchManipulator(view) { IsPanByTowFinger = true }, args));
            SnapTrackTouch = new DelegatePlotCommand<OxyTouchEventArgs>((view, controller, args) => controller.AddTouchManipulator(view, new Manipulators.TouchTrackerManipulator(view) { Snap = true, PointsOnly = true, LockToInitialSeries = false }, args));
        }
    }
}