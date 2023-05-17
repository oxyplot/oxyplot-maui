using OxyPlot.Maui.Skia.Effects;
using OxyPlot.Maui.Skia.Fonts;

namespace OxyPlot.Maui.Skia;

public static class AppHostBuilderExtensions
{
    public static MauiAppBuilder UseOxyPlotSkia(this MauiAppBuilder builder)
    {
        return builder.ConfigureEffects(effects =>
        {
#if __ANDROID__
            effects.Add<MyTouchEffect, Droid.Effects.PlatformTouchEffect>();
#elif WINDOWS
            effects.Add<MyTouchEffect, Windows.Effects.PlatformTouchEffect>();
#elif MACCATALYST
            // not implemented
#elif __IOS__
            effects.Add<MyTouchEffect, ios.Effects.PlatformTouchEffect>();
#endif
        });
    }

    public static MauiAppBuilder UseOxyPlotSkiaCustomFonts(this MauiAppBuilder builder, IPlotFontsResolver resolver)
    {
        SkFontsHelper.FontsResolver = resolver;
        return builder;
    }
}