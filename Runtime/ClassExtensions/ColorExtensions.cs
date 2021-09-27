using UnityEngine;

public static class ColorExtensions
{
    public static Color Opaque(this Color color)
    {
        return new Color(color.r, color.g, color.b);
    }

    public static Color WithAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    /// <summary>
    /// Compress a color object to an int.
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static int Encoded(this Color self)
    {
        Color32 color32 = self;
        int c = 0;
        c |= color32.a << 24;
        c |= color32.r << 16;
        c |= color32.g << 8;
        c |= color32.b;
        return c;
    }
}