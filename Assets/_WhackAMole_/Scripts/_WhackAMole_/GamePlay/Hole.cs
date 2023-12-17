using DG.Tweening;

namespace WhackAMole
{
    public class Hole : PoolItem, iAppearable
    {
        public static ScaleAppearableConfiguration Config => GameManager.Instance.Configuration.HoleConfiguration;

        private Tween _tween;

        public void Appear()
        {
            _tween.OverKill();

            _tween = transform.DOScale(Config.AppearScale, Config.AppearDuration).From(Config.HideScale).SetEase(Ease.OutBack);
        }

        public void Disappear()
        {
            _tween.OverKill();

            _tween = transform.DOScale(Config.HideScale, Config.HideDuration).SetEase(Config.HideEase);
        }
    }

    [System.Serializable]
    public struct HoleConfiguration
    {
        public float AppearScale, AppearDuration, HideScale, HideDuration;
        public Ease AppearEase, HideEase;
    }
}