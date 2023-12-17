using UnityEngine;

namespace WhackAMole
{
    public static class SaveGame
    {
        private const string c_levelKey = "level";
        public static int Level
        {
            get => PlayerPrefs.GetInt(c_levelKey, 1);
            set => PlayerPrefs.SetInt(c_levelKey, value);
        }

        private const string c_maxLevelKey = "max_level";
        public static int MaxLevel
        {
            get => PlayerPrefs.GetInt(c_maxLevelKey, 1);
            set => PlayerPrefs.SetInt(c_maxLevelKey, value);
        }
    }
}