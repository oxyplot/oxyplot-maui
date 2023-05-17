using OxyPlot.Maui.Skia.Fonts;
using AApplication = Android.App.Application;

namespace OxyPlot.Maui.Skia.Platforms.Android;

public class MauiFontLoader : IMauiFontLoader
{
    static readonly string[] FontFolders = new[]
    {
        "Fonts/",
        "fonts/",
    };

    private IFontRegistrar fontRegistrar = null;
    /// <inheritdoc />
    public Stream Load(string fontName)
    {
        if (fontRegistrar == null)
            fontRegistrar = IPlatformApplication.Current.Services.GetRequiredService<IFontRegistrar>();

        return GetFromAssets(fontName);
    }

    Stream GetFromAssets(string fontName)
    {
        fontName = fontRegistrar.GetFont(fontName) ?? fontName;

        // First check Alias
        var asset = LoadFontFromAsset(fontName);
        if (asset != null)
            return asset;

        // The font might be a file, such as a temporary file extracted from EmbeddedResource
        if (File.Exists(fontName))
        {
            return File.OpenRead(fontName);
        }

        var fontFile = FontFile.FromString(fontName);
        if (!string.IsNullOrWhiteSpace(fontFile.Extension))
        {
            return FindFont(fontFile.FileNameWithExtension());
        }

        foreach (var ext in FontFile.Extensions)
        {
            var font = FindFont(fontFile.FileNameWithExtension(ext));
            if (font != null)
                return font;
        }

        return null;
    }

    Stream FindFont(string fileWithExtension)
    {
        var result = LoadFontFromAsset(fileWithExtension);
        if (result != null)
            return result;

        foreach (var folder in FontFolders)
        {
            result = LoadFontFromAsset(folder + fileWithExtension);
            if (result != null)
                return result;
        }

        return null;
    }

    Stream LoadFontFromAsset(string fontName)
    {
        try
        {
            var fontPath = FontNameToFontFile(fontName);
            if (!AApplication.Context.Assets.List("").Contains(fontPath))
                return null;
            return AApplication.Context.Assets.Open(fontPath);
        }
        catch
        {
            // ignore
        }

        return null;
    }

    string FontNameToFontFile(string fontFamily)
    {
        fontFamily ??= string.Empty;

        int hashtagIndex = fontFamily.IndexOf("#", StringComparison.Ordinal);
        if (hashtagIndex >= 0)
            return fontFamily.Substring(0, hashtagIndex);

        return fontFamily;
    }
}