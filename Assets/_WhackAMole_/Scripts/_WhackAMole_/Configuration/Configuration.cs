using UnityEngine;

namespace WhackAMole
{
    [CreateAssetMenu(menuName = "Scriptable Object/Configuration", fileName = "Configuration")]
    public class Configuration : ScriptableObject
    {
        [SerializeField] private GeneralConfiguration _generalConfiguration;
        public GeneralConfiguration GeneralConfiguration => _generalConfiguration;

        
        [SerializeField] private LevelConfiguration _levelConfiguration;
        public LevelConfiguration LevelConfiguration => _levelConfiguration;


        [SerializeField] private MoleConfiguration _moleConfiguration;
        public MoleConfiguration MoleConfiguration => _moleConfiguration;


        [SerializeField] private ScaleAppearableConfiguration _holeConfiguration;
        public ScaleAppearableConfiguration HoleConfiguration => _holeConfiguration;

        
        [SerializeField] private HammerConfiguration _hammerConfiguration;
        public HammerConfiguration HammerConfiguration => _hammerConfiguration;

        
        [SerializeField] private ScaleTweenButtonConfiguration _scaleTweenButtonConfiguration;
        public ScaleTweenButtonConfiguration ScaleTweenButtonConfiguration => _scaleTweenButtonConfiguration;


        [SerializeField] private ScaleAppearableConfiguration _startWindowConfiguration;
        public ScaleAppearableConfiguration StartWindowConfiguration => _startWindowConfiguration;


        [SerializeField] private float _gamePlayWindowFadeDuration;
        public float GamePlayWindowFadeDuration => _gamePlayWindowFadeDuration;
    }
}