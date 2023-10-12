using UnityEngine;

public class TargetFrameRate : MonoBehaviour
{
    [SerializeField, Range(30, 60)] private int _targetFrameRate;

    private void Awake()
    {
        Application.targetFrameRate = _targetFrameRate;
    }
}
