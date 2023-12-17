using DG.Tweening;

namespace WhackAMole
{
    [System.Serializable]
    public struct ScaleAppearableConfiguration
    {
        public float AppearScale, AppearDuration, HideScale, HideDuration;
        public Ease AppearEase, HideEase;
    }
}