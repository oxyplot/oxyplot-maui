using ExampleLibrary;

namespace SimpleDemo
{
    public partial class CustomTrackerPage
    {
        public CustomTrackerPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            PlotView.Model = ShowCases.CreateNormalDistributionModel();
        }
    }
}