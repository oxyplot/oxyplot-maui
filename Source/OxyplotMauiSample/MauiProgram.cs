using OxyPlot.Maui.Skia;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace OxyplotMauiSample
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UseOxyPlotSkia()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("NotoSansCJKsc-Regular.otf", "NotoSansCJKsc");
                });

            // CustomFontsResolver:
            //builder.UseOxyPlotSkiaCustomFonts(new OxyPlot.Maui.Skia.Fonts.LocalFileFontsResolver(fontsDir));

            return builder.Build();
        }
    }
}