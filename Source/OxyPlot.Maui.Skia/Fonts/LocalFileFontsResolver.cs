using static System.Net.Mime.MediaTypeNames;

namespace OxyPlot.Maui.Skia.Fonts;

public class LocalFileFontsResolver : IPlotFontsResolver
{
    private readonly Dictionary<int, string> fontWeights = new()
    {
        [100] = "Thin",
        [200] = "ExtraLight",
        [300] = "Light",
        [400] = "Regular",
        [500] = "Medium",
        [600] = "SemiBold",
        [700] = "Bold",
        [800] = "ExtraBold",
        [900] = "Black"
    };

    public string FontsDirectory { get; set; }

    public string[] SearchFontExtensions { get; set; } = { ".ttf", ".otf" };

    public LocalFileFontsResolver(string fontsDirectory)
    {
        this.FontsDirectory = fontsDirectory;
    }

    /// <inheritdoc />
    public Stream ResolveFont(string fontFamily, int fontWeight)
    {
        if (!Directory.Exists(FontsDirectory))
        {
            return null;
        }

        var weight = (fontWeights.TryGetValue(fontWeight, out var w) ? w : "Regular");
        foreach (var ext in SearchFontExtensions)
        {
            var fontFilePath = Path.Combine(FontsDirectory, $"{fontFamily}-{weight}{ext}");
            if (!File.Exists(fontFilePath))
            {
                fontFilePath = Path.Combine(FontsDirectory, fontFamily + ext);
            }

            if (!File.Exists(fontFilePath))
            {
                continue;
            }

            return File.OpenRead(fontFilePath);
        }

        // can not find bold,try fallback to Regular
        if (weight != "Regular")
        {
            foreach (var ext in SearchFontExtensions)
            {
                var fontFilePath = Path.Combine(FontsDirectory, $"{fontFamily}-Regular{ext}");
                if (!File.Exists(fontFilePath))
                {
                    continue;
                }

                return File.OpenRead(fontFilePath);
            }
        }

        return null;
    }
}