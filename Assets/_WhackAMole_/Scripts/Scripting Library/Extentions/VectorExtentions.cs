using UnityEngine;

public static class VectorExtentions
{
    public static float Random(this Vector2 v) => UnityEngine.Random.Range(v.x, v.y);
    /// <summary>
    /// Inclusive
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static int Random(this Vector2Int v) => UnityEngine.Random.Range(v.x, v.y + 1);

    public static Vector2 Abs(this Vector2 v) => new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    /// <summary>
    /// Returns the lerped value from x to y by step t.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float Lerp(this Vector2 v, float t) => Mathf.Lerp(v.x, v.y, t);
    public static int Lerp(this Vector2Int v, float t) => (int)Mathf.Lerp(v.x, v.y, t);
    /// <summary>
    /// Clamps value between x and y components of this Vector.
    /// </summary>
    /// <param name="v">Bounds for clamping: x - min, y - max</param>
    /// <param name="t">Value to be clamped</param>
    public static float Clamp(this Vector2 v, float t) => Mathf.Clamp(t, v.x, v.y);
    /// <summary>
    /// Clamps value between x and y components of this Vector.
    /// </summary>
    /// <param name="v">Bounds for clamping: x - min, y - max</param>
    /// <param name="i">Value to be clamped</param>
    public static int Clamp(this Vector2Int v, int i) => Mathf.Clamp(i, v.x, v.y);

    public static Vector3 ChangeX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);

    public static Vector3 RotateZ(this Vector3 v, float degrees)
    {
        return Quaternion.AngleAxis(degrees, Vector3.forward) * v;
    }
    public static Vector3 RotateY(this Vector3 v, float degrees)
    {
        return Quaternion.AngleAxis(degrees, Vector3.up) * v;
    }
}
