using DG.Tweening;
using UnityEngine;

namespace WhackAMole
{
    public class Hammer : MonoBehaviour, iAppearable
    {
        private const float c_rayCastDistance = 20f;

        public HammerConfiguration Config => GameManager.Instance.Configuration.HammerConfiguration;

        private Ray _ray;
        private Camera _camera;
        private LayerMask _targetLayers;
        private Camera _safeCamera
        {
            get
            {
                if (_camera == null) _camera = Camera.main;
                return _camera;
            }
        }
        private Sequence _hitSequence;
        private bool _hitSequenceCancelable = true;
        private Vector3 _startPos;
        private Quaternion _startRot;

        private void Awake()
        {
            _targetLayers = Extentions.ToLayerMaskValue((int)Layers.Mole, (int)Layers.Ground);
            _startPos = transform.position;
            _startRot = transform.rotation;
        }

        private void Update()
        {
            DetectInput();
        }

        private void DetectInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ray = _safeCamera.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(_ray, out RaycastHit hitInfo, c_rayCastDistance, _targetLayers.value);

                if (hitInfo.collider != null)
                {
                    var obj = hitInfo.collider.gameObject;

                    Vector3 hitPos = Vector3.zero;

                    if (obj.layer == (int)Layers.Mole)
                    {
                        hitPos = obj.transform.position;
                    }
                    else if (obj.layer == (int)Layers.Ground)
                    {
                        hitPos = hitInfo.point;
                    }

                    Hit(hitPos);
                }
            }
        }
        [SerializeField] private float k;
        private void Hit(Vector3 position)
        {
            if (!_hitSequenceCancelable) return;

            _hitSequence.OverKill();

            position += Config.HitPosOffset;

            _hitSequence = DOTween.Sequence();
            _hitSequenceCancelable = false;

            _hitSequence.Append(transform.DOJump(position, Config.HitMoveInJumpPower, 1, Config.HitMoveInTime).SetEase(Config.HitMoveInEase));
            _hitSequence.Join(transform.DORotate(Config.HitRot, Config.HitRotInTime).SetDelay(Config.PreHitRotDelay).SetEase(Config.HitRotInEase));
            _hitSequence.AppendCallback(() => _hitSequenceCancelable = true);
            _hitSequence.Append(transform.DORotateQuaternion(_startRot, Config.HitRotOutTime).SetEase(Config.HitRotOutEase));
            _hitSequence.Append(transform.DOMove(_startPos, Config.HitMoveOutTime).SetEase(Config.HitMoveOutEase));
            _hitSequence.OnKill(() => _hitSequenceCancelable = true);
        }

        public void Appear()
        {
            gameObject.Enable();
            _hitSequence.OverKill();
            transform.position = _startPos;
            transform.rotation = _startRot;

            transform.DOScale(Config.AppearConf.AppearScale, Config.AppearConf.AppearDuration)
                .From(Config.AppearConf.HideScale)
                .SetEase(Config.AppearConf.AppearEase);
        }

        public void Disappear()
        {
            transform.DOScale(Config.AppearConf.HideScale, Config.AppearConf.HideDuration)
                .SetEase(Config.AppearConf.HideEase)
                .OnComplete(gameObject.Disable);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(_ray);
        }
#endif
    }

    [System.Serializable]
    public struct HammerConfiguration
    {
        public float HitMoveInTime, HitMoveInJumpPower, HitRotInTime, HitRotOutTime, PreHitRotDelay, HitMoveOutTime;
        public Ease HitMoveInEase, HitRotInEase, HitRotOutEase, HitMoveOutEase;
        public Vector3 HitPosOffset;
        public Vector3 HitRot;

        public ScaleAppearableConfiguration AppearConf;
    }
}