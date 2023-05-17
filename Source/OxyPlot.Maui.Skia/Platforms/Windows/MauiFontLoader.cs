using Windows.Storage;
using OxyPlot.Maui.Skia.Fonts;

namespace OxyPlot.Maui.Skia.Windows;

public class MauiFontLoader : IMauiFontLoader
{
    private IFontRegistrar _fontRegistrar = null;

    /// <inheritdoc />
    public Stream Load(string fontName)
    {
        if (_fontRegistrar == null)
            _fontRegistrar = IPlatformApplication.Current.Services.GetRequiredService<IFontRegistrar>();

        fontName = _fontRegistrar.GetFont(fontName);
        if (File.Exists(fontName))
        {
            return File.OpenRead(fontName);
        }

        try
        {
            var file = StorageFile.GetFileFromApplicationUriAsync(new Uri(fontName)).AsTask().Result;
            return file.OpenReadAsync().AsTask().Result.AsStream();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }

        return null;
    }
}