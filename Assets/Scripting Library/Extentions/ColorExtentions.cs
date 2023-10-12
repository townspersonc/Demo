using UnityEngine;

public static class ColorExtentions
{
    public static Color Transparent(this Color c) => c.WithAlpha(0f);
    public static Color Opaque(this Color c) => c.WithAlpha(1f);
    public static Color WithAlpha(this Color c, float a) => new Color(c.r, c.g, c.b, a);
}