using DG.Tweening;
using UnityEngine;

namespace WhackAMole
{
    public class GamePlayWindow : Window
    {
        public static float FadeDuration => GameManager.Instance.Configuration.GamePlayWindowFadeDuration;

        [SerializeField] private TMP_FormatedText _levelText;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private PropertyLabel_Int _missedMolesLabel, _remainingMolesLabel, _playerLivesLabel;

        public override int OpenMiliSeconds => (int)(FadeDuration * 1000f);
        public override int CloseMiliSeconds => OpenMiliSeconds;

        private void OnEnable()
        {
            _levelText.SetText(LevelManager.ActiveLevelData.Level);

            _missedMolesLabel.Init(GameManager.Instance.LevelManager.MissedMoles);
            _remainingMolesLabel.Init(GameManager.Instance.LevelManager.RemainingMolesToHit);
            _playerLivesLabel.Init(GameManager.Instance.LevelManager.PlayerLives);
        }

        public void U_CloseClick()
        {
            GameManager.Instance.LevelAbort();
        }

        protected override void PerformOpenAnimation()
        {
            _group.DOFade(1f, FadeDuration).From(0f);
        }

        protected override void PerformCloseAnimation()
        {
            _group.DOFade(0f, FadeDuration);
        }
    }
}
