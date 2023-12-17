public static class Extentions
{
    public static bool RandomBool => UnityEngine.Random.Range(0, 2) == 1;
    public static float RandomSign => RandomBool ? -1 : 1;

    public static int ToLayerMaskValue(this int i) => 1 << i;
    public static int ToLayerMaskValue(params int[] args)
    {
        int mask = 0;

        foreach(var a in args) 
        {
            mask |= (1 << a);
        }

        return mask;
    }
}
