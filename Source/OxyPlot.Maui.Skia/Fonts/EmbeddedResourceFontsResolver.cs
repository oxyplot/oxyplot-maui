using System.Reflection;

namespace OxyPlot.Maui.Skia.Fonts;

public class EmbeddedResourceFontsResolver : IPlotFontsResolver
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

    public Assembly Assembly { get; set; }

    public string[] SearchFontExtensions { get; set; } = { ".ttf", ".otf" };

    public EmbeddedResourceFontsResolver(Assembly assembly)
    {
        this.Assembly = assembly;
    }

    /// <inheritdoc />
    public Stream ResolveFont(string fontFamily, int fontWeight)
    {
        if (Assembly == null)
        {
            return null;
        }

        var weight = (fontWeights.TryGetValue(fontWeight, out var w) ? w : "Regular");
        var resourceNames = Assembly.GetManifestResourceNames();
        foreach (var ext in SearchFontExtensions)
        {
            var fontFileName = $"{fontFamily}-{weight}{ext}";
            var resourceName =
                resourceNames.FirstOrDefault(x => x.EndsWith(fontFileName, StringComparison.OrdinalIgnoreCase));

            if (resourceName == null)
            {
                fontFileName = $"{fontFamily}{ext}";
                resourceName =
                    resourceNames.FirstOrDefault(x => x.EndsWith(fontFileName, StringComparison.OrdinalIgnoreCase));
            }

            if (resourceName == null)
                continue;
            return Assembly.GetManifestResourceStream(resourceName);
        }

        return null;
    }
}