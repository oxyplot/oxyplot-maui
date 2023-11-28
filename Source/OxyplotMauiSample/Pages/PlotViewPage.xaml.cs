using ExampleLibrary;
using ExampleLibrary.Utilities;
using OxyPlot.Maui.Skia.Core;
using OxyPlot;

namespace OxyplotMauiSample
{
    public partial class PlotViewPage
    {
        private const string DefaultFont = "NotoSansCJKsc";

        public ExampleInfo ExampleInfo { get; set; }

        public PlotViewPage()
        {
            InitializeComponent();
            this.Loaded += PlotViewPage_Loaded;
            this.Unloaded += PlotViewPage_Unloaded;
        }

        private IPlotController CreateAnnotationTrackController()
        {
            var controller = new OxyPlot.Maui.Skia.PlotController();
            controller.UnbindTouchDown();

            var snapTrackTouch = new DelegatePlotCommand<OxyTouchEventArgs>((view, c, args) =>
                c.AddTouchManipulator(view, new OxyPlot.Maui.Skia.Manipulators.TouchTrackerManipulator(view)
                {
                    Snap = true,
                    PointsOnly = true,
                    LockToInitialSeries = false,
                    IsTrackAnnotations = true
                }, args));

            var cmd = new CompositeDelegateViewCommand<OxyTouchEventArgs>(
                snapTrackTouch,
                OxyPlot.Maui.Skia.PlotCommands.PanZoomByTouch
            );
            controller.BindTouchDown(cmd);

            return controller;
        }

        private void PlotViewPage_Loaded(object sender, EventArgs e)
        {
            this.Title = ExampleInfo.Category + " - " + ExampleInfo.Title;
            TbTranspose.IsEnabled = ExampleInfo.IsTransposable;
            TryAddReverseMenuItem();

            PlotView.Model = ExampleInfo.PlotModel;
            PlotView.Model.DefaultFont = DefaultFont;

            if (ExampleInfo.PlotController != null)
            {
                PlotView.Controller = ExampleInfo.PlotController;
            }
            else if (ExampleInfo.Tags.Contains("Annotations"))
            {
                // make the annotation tooltip work
                PlotView.Controller = CreateAnnotationTrackController();
            }
        }

        private void PlotViewPage_Unloaded(object sender, EventArgs e)
        {
            PlotView.Model = null;
        }

        private void TbReverse_OnClicked(object sender, EventArgs e)
        {
            var mi = sender as ToolbarItem;
            var action = mi.CommandParameter as string;
            switch (action)
            {
                case "ReverseXAxis":
                    PlotView.Model.ReverseXAxis();
                    break;
                case "ReverseYAxis":
                    PlotView.Model.ReverseYAxis();
                    break;
                case "ReverseAllAxes":
                    PlotView.Model.ReverseAllAxes();
                    break;
            }
            PlotView.Model.InvalidatePlot(false);
        }

        private void TryAddReverseMenuItem()
        {
            if (!ExampleInfo.PlotModel.IsReversible())
            {
                return;
            }

            var ti = new ToolbarItem
            {
                Text = "ReverseXAxis",
                CommandParameter = "ReverseXAxis",
                Order = ToolbarItemOrder.Secondary,
            };
            ti.Clicked += TbReverse_OnClicked;
            this.ToolbarItems.Add(ti);

            ti = new ToolbarItem
            {
                Text = "ReverseYAxis",
                CommandParameter = "ReverseYAxis",
                Order = ToolbarItemOrder.Secondary,
            };
            ti.Clicked += TbReverse_OnClicked;
            this.ToolbarItems.Add(ti);

            ti = new ToolbarItem
            {
                Text = "ReverseAllAxes",
                CommandParameter = "ReverseAllAxes",
                Order = ToolbarItemOrder.Secondary,
            };
            ti.Clicked += TbReverse_OnClicked;
            this.ToolbarItems.Add(ti);
        }

        private void TbTranspose_OnClicked(object sender, EventArgs e)
        {
            PlotView.Model.Transpose();
            PlotView.Model.InvalidatePlot(false);
        }
    }
}
