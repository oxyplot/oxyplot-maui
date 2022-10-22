using ExampleLibrary;
using OxyPlot;
using OxyPlot.XF.Skia.Core;
using Xamarin.Forms;
using PlotCommands = OxyPlot.XF.Skia.PlotCommands;

namespace SimpleDemo
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
            var viewCmd = PlotCommands.PanZoomByTouch;
            switch (rb.Value)
            {
                case "2":
                    viewCmd = PlotCommands.PanZoomByTouchTwoFinger;
                    break;
                case "3":
                    viewCmd = PlotCommands.PanZoomByTouchAxisOnly;
                    break;
            }

            var cmd = new CompositeDelegateViewCommand<OxyTouchEventArgs>(
                PlotCommands.SnapTrackTouch,
                viewCmd
            );

            PlotView.Controller.BindTouchDown(cmd);
        }
    }
}