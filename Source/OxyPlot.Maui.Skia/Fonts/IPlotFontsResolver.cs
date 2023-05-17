namespace OxyPlot.Maui.Skia.Fonts;

public interface IPlotFontsResolver
{
    Stream ResolveFont(string fontFamily, int fontWeight);
}