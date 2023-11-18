using UnityEngine;

public static class MathExtentions
{
    public static float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (Mathf.Clamp(x, in_min, in_max) - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="integer"></param>
    /// <param name="start">Inclusive</param>
    /// <param name="end">Exsclusive</param>
    /// <returns></returns>
    public static bool Between(this int integer, int start, int end)
    {
        if (start > end) Swap(ref start, ref end);

        return integer >= start && integer < end;
    }
    public static bool Between(this float f, float start, float end)
    {
        if (start > end) Swap(ref start, ref end);

        return f >= start && f <= end;
    }
    public static bool BetweenExclusive(this float f, float start, float end)
    {
        if (start > end) Swap(ref start, ref end);

        return f > start && f < end;
    }

    public static void Swap(ref int x, ref int y)
    {
        x += y;
        y = x - y;
        x -= y;
    }
    public static void Swap(ref float x, ref float y)
    {
        x += y;
        y = x - y;
        x -= y;
    }

    /// <summary>
    /// Returns Modulo as defined in Math.
    /// </summary>
    public static int RealModulo(int devidant, int devisor)
    {
        int a = devidant % devisor;
        return a < 0 ? a + devisor : a;
    }
    public static float RealModulo(float devidant, float devisor)
    {
        float a = devidant % devisor;
        return a < 0 ? a + devisor : a;
    }

    /// <summary>
    /// Clamps input parameter between negative absolute value and absolute value of this float.
    /// </summary>
    /// <param name="f"></param>
    /// <param name="clampValue"></param>
    /// <returns></returns>
    public static float Clamp(this float f, float clampValue) => Mathf.Clamp(clampValue, -Mathf.Abs(f), Mathf.Abs(f));

    /// <summary>
    /// Gets how close value is from min to max, if x == min than 0, if x== max than 1.
    /// </summary>
    /// <param name="x">value to be calculated</param>
    /// <param name="min">minimum value</param>
    /// <param name="max">maximum value</param>
    /// <returns></returns>
    public static float FromZeroToOne(float x, float min, float max)
    {
        x = Mathf.Clamp(x, min, max);

        return (x - min) / (max - min);
    }
}