using System.Reflection;

namespace OxyplotMauiSample
{
    public partial class IssueDemoPage
    {
        public IssueDemoPage()
        {
            this.InitializeComponent();
            var demoPages = new List<DemoInfo>();
            foreach (var type in typeof(IssueDemoPage).GetTypeInfo().Assembly.ExportedTypes)
            {
                var ti = type.GetTypeInfo();
                if (ti.GetCustomAttribute<DemoPageAttribute>() != null)
                {
                    demoPages.Add(new DemoInfo(type));
                }
            }

            this.list1.ItemsSource = demoPages;
        }

        private async void CollectionView_OnItemTapped(object sender, TappedEventArgs e)
        {
            var grid = (Grid)sender;
            var demoInfo = (DemoInfo)grid.BindingContext;
            var page = demoInfo.CreatePage();
            page.Title = demoInfo.Title;
            await Navigation.PushAsync(page);
            list1.SelectedItem = null;
        }
    }
}
