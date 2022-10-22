using OxyPlot.Maui.Skia.Effects;

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
#elif __IOS__
            // effects.Add<MyTouchEffect, OxyPlot.Maui.Skia.ios.Effects.PlatformTouchEffect>();
#endif
        });
    }
}