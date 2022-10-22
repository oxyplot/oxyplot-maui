using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;

namespace IssueDemos
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

        private async void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var demoInfo = (DemoInfo)e.Item;
            var page = demoInfo.CreatePage();
            page.Title = demoInfo.Title;
            await Navigation.PushAsync(page);
        }
    }
}
