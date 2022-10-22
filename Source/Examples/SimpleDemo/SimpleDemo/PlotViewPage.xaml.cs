using System;
using ExampleLibrary;
using ExampleLibrary.Utilities;
using OxyPlot.XF.Skia;
using Xamarin.Forms;

namespace SimpleDemo
{
    public partial class PlotViewPage
    {
        private const string DefaultFont = "Noto Sans CJK SC";
        public ExampleInfo ExampleInfo { get; set; }

        public PlotViewPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
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
        }

        protected override void OnDisappearing()
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
