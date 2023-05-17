using OxyPlot.Maui.Skia.Fonts;

namespace OxyPlot.Maui.Skia.ios;

public class MauiFontLoader : IMauiFontLoader
{
    private IFontRegistrar fontRegistrar = null;

    /// <inheritdoc />
    public Stream Load(string fontName)
    {
        if (fontRegistrar == null)
            fontRegistrar = IPlatformApplication.Current.Services.GetRequiredService<IFontRegistrar>();

        fontName = fontRegistrar.GetFont(fontName);
        if (File.Exists(fontName))
        {
            return File.OpenRead(fontName);
        }

        try
        {
            var resolvedFilename = ResolveFileSystemFont(fontName);
            if (!string.IsNullOrEmpty(resolvedFilename) && File.Exists(resolvedFilename))
            {
                return File.OpenRead(resolvedFilename);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }

        return null;
    }

    string ResolveFileSystemFont(string filename)
    {
        var mainBundlePath = Foundation.NSBundle.MainBundle.BundlePath;

#if MACCATALYST
        // macOS Apps have Contents folder in the bundle root, iOS does not
        mainBundlePath = Path.Combine(mainBundlePath, "Contents");
#endif

        var fontBundlePath = Path.Combine(mainBundlePath, filename);
        if (File.Exists(fontBundlePath))
            return fontBundlePath;

        fontBundlePath = Path.Combine(mainBundlePath, "Resources", filename);
        if (File.Exists(fontBundlePath))
            return fontBundlePath;

        fontBundlePath = Path.Combine(mainBundlePath, "Fonts", filename);
        if (File.Exists(fontBundlePath))
            return fontBundlePath;

        fontBundlePath = Path.Combine(mainBundlePath, "Resources", "Fonts", filename);
        if (File.Exists(fontBundlePath))
            return fontBundlePath;

        // TODO: check other folders as well

        return null;
    }

}