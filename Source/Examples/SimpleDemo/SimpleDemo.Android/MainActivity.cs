using System.IO;
using Android.App;
using Android.Content.PM;
using Android.OS;
using OxyPlot.XF.Skia;

namespace SimpleDemo.Droid
{
    [Activity(Label = "SimpleDemo", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            var cacheDir = this.CacheDir.AbsolutePath;
            var fontsDir = Path.Combine(cacheDir, "oxyplot_fonts");
            if (!Directory.Exists(fontsDir))
            {
                Directory.CreateDirectory(fontsDir);
            }

            XFPlotSetting.CustomFontDirectory = fontsDir;

            // Simplified Chinese
            string fontName = "NotoSansCJKsc-Regular.otf";
            string fontPath = Path.Combine(fontsDir, fontName);

            if (!File.Exists(fontPath))
            {
                using var asset = Assets.Open(fontName);
                using var dest = File.Open(fontPath, FileMode.Create);
                asset.CopyTo(dest);
            }

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }
    }
}

