using ExampleLibrary;
using OxyPlot;
using OxyPlot.Maui.Skia.Core;
using PlotCommands = OxyPlot.Maui.Skia.PlotCommands;
namespace OxyplotMauiSample
{
    public partial class PanModePage
    {
        public PanModePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            PlotView.Model = ShowCases.CreateNormalDistributionModel();
        }

        private void PanMode_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!e.Value)
                return;

            var rb = sender as RadioButton;
            if (rb?.Value == null)
                return;

            var viewCmd = rb.Value switch
            {
                "2" => PlotCommands.PanZoomByTouchTwoFinger,
                "3" => PlotCommands.PanZoomByTouchAxisOnly,
                _ => PlotCommands.PanZoomByTouch
            };

            var cmd = new CompositeDelegateViewCommand<OxyTouchEventArgs>(
                PlotCommands.SnapTrackTouch,
                viewCmd
            );

            PlotView.Controller.BindTouchDown(cmd);
        }
    }
}