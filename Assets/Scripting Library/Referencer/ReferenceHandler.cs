using UnityEngine;

[DefaultExecutionOrder(-5)]
public class ReferenceHandler : MonoBehaviour
{
    [SerializeField] Object _target;

    private void OnEnable()
    {
        Referencer.Add(_target);
    }
    private void OnDisable()
    {
        Referencer.Remove(_target);
    }
}
