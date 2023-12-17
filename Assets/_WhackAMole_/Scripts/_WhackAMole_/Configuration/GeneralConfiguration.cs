using UnityEngine;

namespace WhackAMole
{
    [System.Serializable]
    public struct GeneralConfiguration
    {
        [SerializeField] private int _playerLives;
        public int PlayerLives => _playerLives;
        
        [SerializeField, Tooltip("Player will lose 1 life when this many moles escape")] private int _lifeToMissedMoleRatio;
        public int LifeToMissedMoleRatio => _lifeToMissedMoleRatio;
    }
}