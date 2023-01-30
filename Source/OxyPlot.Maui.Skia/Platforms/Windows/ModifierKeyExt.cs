using Windows.System;

namespace OxyPlot.Maui.Skia.Windows;

public static class ModifierKeyExt
{
    public static OxyModifierKeys ToOxyModifierKeys(this VirtualKeyModifiers vkm)
    {
        var modifiers = OxyModifierKeys.None;

        if (vkm.HasFlag(VirtualKeyModifiers.Shift))
        {
            modifiers |= OxyModifierKeys.Shift;
        }

        if (vkm.HasFlag(VirtualKeyModifiers.Control))
        {
            modifiers |= OxyModifierKeys.Control;
        }

        if (vkm.HasFlag(VirtualKeyModifiers.Menu))
        {
            modifiers |= OxyModifierKeys.Alt;
        }

        if (vkm.HasFlag(VirtualKeyModifiers.Windows))
        {
            modifiers |= OxyModifierKeys.Windows;
        }

        return modifiers;
    }
}