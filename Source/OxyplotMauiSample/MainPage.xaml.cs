namespace OxyplotMauiSample
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void GoToPage(object sender, EventArgs e)
        {
            var pageName = (sender as Button).Text;
            GoToPageAsync(pageName);
        }

        private Task GoToPageAsync(string name)
        {
            var pageType = GetType().Assembly.GetType($"{GetType().Namespace}.{name}");
            var page = (ContentPage)Activator.CreateInstance(pageType);
            return Navigation.PushAsync(page);
        }
    }
}