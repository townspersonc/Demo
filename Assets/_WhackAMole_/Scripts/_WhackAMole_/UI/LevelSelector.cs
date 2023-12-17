using UnityEngine;
using UnityEngine.UI;

namespace WhackAMole
{
    public class LevelSelector : MonoBehaviour
    {
        [SerializeField] private TMP_FormatedText _levelLabel;
        [SerializeField] private Slider _slider;

        private Vector2Int _levelClamp;

        private void OnEnable()
        {
            int maxLevel = SaveGame.MaxLevel;

            _levelClamp = new Vector2Int(1, maxLevel);
            SelectedLevel = maxLevel;
            RefreshSlider();
        }

        private int _selectedLevel;
        private int SelectedLevel
        {
            get => _selectedLevel;
            set
            {
                _selectedLevel = Mathf.Clamp(value, _levelClamp.x, _levelClamp.y);
                _levelLabel.SetText(_selectedLevel);
            }
        }

        public void U_OnRightClick()
        {
            SelectedLevel++;
            RefreshSlider();
        }
        public void U_OnLeftClick()
        {
            SelectedLevel--;
            RefreshSlider();
        }

        public void U_OnSliderValueChange(float value)
        {
            SelectedLevel = (int)Mathf.Lerp(_levelClamp.x, _levelClamp.y, value);
        }

        public void U_OnStartClick()
        {
            GameManager.Instance.StartLevel(SelectedLevel);
        }

        private void RefreshSlider()
        {
            _slider.value = Mathf.InverseLerp(_levelClamp.x, _levelClamp.y, SelectedLevel);
        }
    }
}
