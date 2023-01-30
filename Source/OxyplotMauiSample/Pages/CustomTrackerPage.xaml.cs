using ExampleLibrary;

namespace OxyplotMauiSample
{
    public partial class CustomTrackerPage
    {
        public CustomTrackerPage()
        {
            InitializeComponent();
            this.Loaded += CustomTrackerPage_Loaded;
        }

        private void CustomTrackerPage_Loaded(object sender, EventArgs e)
        {
            PlotView.Model = ShowCases.CreateNormalDistributionModel();
        }
    }
}