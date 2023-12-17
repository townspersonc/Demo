using UnityEngine;

public class TargetFrameRate : MonoBehaviour
{
    [SerializeField, Range(30, 120)] private int _targetFrameRate = 60;

    private void Awake()
    {
        Application.targetFrameRate = _targetFrameRate;
    }
}
