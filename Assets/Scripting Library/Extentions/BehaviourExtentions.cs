using UnityEngine;

public static class BehaviourExtentions
{
    public static void Enable(this Behaviour behaviour) => behaviour.enabled = true;
    public static void Disable(this Behaviour behaviour) => behaviour.enabled = false;
    public static void ReEnable(this Behaviour behaviour)
    {
        behaviour.Disable();
        behaviour.Enable();
    }
}