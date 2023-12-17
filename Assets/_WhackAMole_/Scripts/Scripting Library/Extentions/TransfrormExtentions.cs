using UnityEngine;

public static class TransfrormExtentions
{
    public static void DestroyAllChildren(this Transform transform)
    {
        int childCount = transform.childCount;

        while (childCount-- > 0)
        {
            var destroyTarget = transform.GetChild(childCount).gameObject;

            if (Application.isPlaying)
            {
                GameObject.Destroy(destroyTarget);
            }
            else
            {
                GameObject.DestroyImmediate(destroyTarget);
            }
        }
    }

    public static void GainPosAndRot(this Transform t, Transform _ref) => GainPosAndRot(t, _ref.position, _ref.rotation);
    public static void GainPosAndRot(this Transform t, Vector3 pos, Quaternion rot)
    {
        t.position = pos;
        t.rotation = rot;
    }
    public static void GainXZ(this Transform t, float x, float z) => t.position = new Vector3(x, t.position.y, z);
    public static void GainXY(this Transform t, float x, float y) => t.position = new Vector3(x, y, t.position.z);
    public static void GainY(this Transform t, float y) => t.position = new Vector3(t.position.x, y, t.position.z);
    public static void GainZ(this Transform t, float z) => t.position = new Vector3(t.position.x, t.position.y, z);
    public static void GainX(this Transform t, float x) => t.position = new Vector3(x, t.position.y, t.position.z);
}
