using UnityEngine;

public static class GameObjectExtentions
{
    public static void Enable(this GameObject gameObject) => gameObject.SetActive(true);
    public static void Disable(this GameObject gameObject) => gameObject.SetActive(false);
    public static void ReEnable(this GameObject gameObject)
    {
        gameObject.Disable();
        gameObject.Enable();
    }
}
