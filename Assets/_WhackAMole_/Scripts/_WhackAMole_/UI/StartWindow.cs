using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WhackAMole
{
    public class StartWindow : Window
    {
        public static ScaleAppearableConfiguration Config => GameManager.Instance.Configuration.StartWindowConfiguration;

        [SerializeField] private Button _start, _continue;
        [SerializeField] private GameObject _levelSelector;

        [SerializeField] private List<OpenVariation> _openVariations;

        public override int OpenMiliSeconds => (int)(Config.AppearDuration) * 1000;
        public override int CloseMiliSeconds => (int)(Config.HideDuration) * 1000;

        protected override void OnOpenStart(Payload_Base payload = null)
        {
            base.OnOpenStart(payload);

            Variant variant = Variant.Start;

            if (payload is Payload load) variant = load.Variant;

            foreach (var v in _openVariations)
            {
                bool correctVariation = v.Variant == variant;

                v.GameObjects.ForEach(g => g.SetActive(correctVariation));
            }

            _levelSelector.Enable();

            if (variant == Variant.Start)
            {
                bool newPlayer = SaveGame.MaxLevel == 1;

                _start.gameObject.SetActive(newPlayer);
                _continue.gameObject.SetActive(!newPlayer);
                _levelSelector.gameObject.SetActive(!newPlayer);
            }
        }

        public void U_StartButtonClick()
        {
            GameManager.Instance.StartNextLevel();
        }

        protected override void PerformOpenAnimation()
        {
            transform.DOScale(Config.AppearScale, Config.AppearDuration).SetEase(Config.AppearEase);
        }

        protected override void PerformCloseAnimation()
        {
            transform.DOScale(Config.HideScale, Config.HideDuration).SetEase(Config.HideEase);
        }

        public enum Variant
        {
            Start,
            Win,
            Lose
        }
        public class Payload : Payload_Base
        {
            public readonly Variant Variant;

            public Payload(Variant variant) : base()
            {
                Variant = variant;
            }
        }

        [System.Serializable]
        private class OpenVariation
        {
            public Variant Variant;
            public List<GameObject> GameObjects;
        }
    }

}
