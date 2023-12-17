using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WhackAMole
{
    [System.Serializable]
    public struct ScaleTweenButtonConfiguration
    {
        public float PressedScale, PressedDuration, ReleaseDuration;
        public Ease PressedEase, ReleaseEase;
    }

    public class ScaleTweenButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public static ScaleTweenButtonConfiguration Config => GameManager.Instance.Configuration.ScaleTweenButtonConfiguration;

        private Tween _tween;

        public void OnPointerDown(PointerEventData eventData)
        {
            _tween.OverKill();

            _tween = transform.DOScale(Config.PressedScale, Config.PressedDuration).SetEase(Config.PressedEase);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _tween.OverKill();

            _tween = transform.DOScale(1f, Config.ReleaseDuration).SetEase(Config.ReleaseEase);
        }
    }
}
