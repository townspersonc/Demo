using DG.Tweening;
using System;
using UnityEngine;

namespace WhackAMole
{
    public class Mole : PoolItem, iAppearable
    {
        public static event Action<Mole> MoleGotHit;
        public static event Action<Mole> MoleEscaped;

        public MoleConfiguration Config => GameManager.Instance.Configuration.MoleConfiguration;

        [SerializeField] private MoleCollider _moleCollider;
        [SerializeField] private Transform _moveTransform;
        [SerializeField] private Hole _hole;

        private float _showTime;
        private Sequence _seq;
        private Tween ShowTween => _moveTransform.DOMoveY(Config.VisibleY, Config.ShowDur).SetEase(Config.ShowEase);
        private Tween HideTween => _moveTransform.DOMoveY(0f, Config.HideDur).SetEase(Config.HideEase);
        private bool Escaped => _moleCollider.DetectCollision;

        private void Start()
        {
            _moleCollider.OnHammerEnter += GotHit;
        }

        public void Init(Vector3 localPosition)
        {
            _showTime = float.MaxValue;
            transform.localPosition = localPosition;
            _moveTransform.localPosition = Vector3.zero;
            CalculateNextShowTime();

            _hole.Appear();
            Appear();
        }

        private void Update()
        {
            if (_showTime < Time.time)
            {
                Show();
            }
        }

        private void Show()
        {
            _seq.OverKill();
            _seq = DOTween.Sequence();

            float showInterval = Config.ShowInterval.Random();

            _seq.Append(ShowTween);
            _seq.AppendInterval(showInterval);
            _seq.AppendCallback(Hide);

            _showTime = float.MaxValue;
            _moleCollider.DetectCollision = true;
        }

        private void Hide()
        {
            _seq.OverKill();
            _seq = DOTween.Sequence();

            _seq.Append(HideTween);
            _seq.AppendCallback(OnPostHide);

            CalculateNextShowTime();

            void OnPostHide()
            {
                if (Escaped) MoleEscaped?.Invoke(this);
            }
        }

        private void CalculateNextShowTime()
        {
            _showTime = Time.time + Config.HideInterval.Random();
        }

        private void GotHit()
        {
            Hide();
            MoleGotHit?.Invoke(this);
        }

        public void Appear()
        {
            _seq?.OverKill();
            _seq = DOTween.Sequence();

            _seq.Append(ShowTween.SetDelay(Config.PeakDelay.Random()));
            _seq.AppendInterval(Config.PeekDuration);
            _seq.Append(HideTween);
        }
        public void Disappear()
        {
            _showTime = float.MaxValue;

            _seq.OverKill();
            _seq = DOTween.Sequence();

            _seq.Append(HideTween);
            _seq.AppendCallback(OnPostDisappear);
            _seq.AppendInterval(Hole.Config.HideDuration);
            _seq.AppendCallback(ReturnToPool);

            void OnPostDisappear()
            {
                _hole.Disappear();
            }
        }
    }

    [Serializable]
    public struct MoleConfiguration
    {
        public float VisibleY;
        public float ShowDur, HideDur, PeekDuration;
        public Ease ShowEase, HideEase;
        public Vector2 ShowInterval, HideInterval, PeakDelay;
    }
}