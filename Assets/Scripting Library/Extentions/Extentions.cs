public static class Extentions
{
    public static bool RandomBool => UnityEngine.Random.Range(0, 2) == 1;
    public static float RandomSign => RandomBool ? -1 : 1;

    public static int ToLayerMask(this int i) => 1 << i;
}
