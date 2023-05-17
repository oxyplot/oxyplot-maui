namespace OxyplotMauiSample
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        //private async void InitFonts()
        //{
        //    var cacheDir = FileSystem.Current.CacheDirectory;
        //    var fontsDir = Path.Combine(cacheDir, "oxyplot_fonts");
        //    //MauiPlotSetting.CustomFontsDirectory = fontsDir;

        //    var fontFileName = "NotoSansCJKsc-Regular.otf";
        //    var fontFileFullPath = Path.Combine(fontsDir, fontFileName);
        //    if (File.Exists(fontFileFullPath))
        //        return;

        //    Directory.CreateDirectory(fontsDir);
        //    var fs = await FileSystem.OpenAppPackageFileAsync(fontFileName);
        //    await using var fileStream = File.Create(fontFileFullPath);
        //    await fs.CopyToAsync(fileStream);
        //}
    }
}