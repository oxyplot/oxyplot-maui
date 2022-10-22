using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SimpleDemo
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ExampleBrowser());
            //MainPage = new NavigationPage(new IssueDemos.IssueDemoPage());
            //MainPage = new CustomTrackerPage();
            //MainPage = new PanModePage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
