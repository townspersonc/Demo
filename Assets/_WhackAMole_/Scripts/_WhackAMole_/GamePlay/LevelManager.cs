using System.Collections.Generic;
using UnityEngine;

namespace WhackAMole
{
    public class LevelManager : MonoBehaviour
    {
        public static bool GameOn { get; private set; } = false;

        public static LevelData ActiveLevelData { get; private set; }

        [SerializeField] private Transform _molesParent;
        [SerializeField] private Hammer _hammer;
        private List<Vector3> _spawnPoints;
        private List<iAppearable> _moles;

        public Property<int> RemainingMolesToHit;
        public Property<int> MissedMoles;
        public Property<int> PlayerLives;
        private GeneralConfiguration _generalConf;

        private void OnEnable()
        {
            Mole.MoleGotHit += OnMoleGotHit;
            Mole.MoleEscaped += OnMoleEscaped;
        }
        private void OnDisable()
        {
            Mole.MoleGotHit -= OnMoleGotHit;
            Mole.MoleEscaped -= OnMoleEscaped;
        }

        public static List<Vector3> GenerateSpawnPoints(int amount, float areaRadius = 1.55f, float itemRadius = 0.37f, int maxItterationCount = 200)
        {
            var spawnPoints = new List<Vector3>();

            var randPoints = new List<Vector2>();
            float allowedSqrDistance = Mathf.Pow(itemRadius * 2f, 2);

            while (randPoints.Count < amount && maxItterationCount > 0)
            {
                maxItterationCount--;

                var newPos = Random.insideUnitCircle * areaRadius;
                var valid = true;

                foreach (var point in randPoints)
                {
                    if ((point - newPos).sqrMagnitude < allowedSqrDistance)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    randPoints.Add(newPos);
                }
            }

            foreach (var point in randPoints)
            {
                spawnPoints.Add(new Vector3(point.x, 0f, point.y));
            }

            return spawnPoints;
        }

        public void StartLevel(LevelData levelData, GeneralConfiguration generalConfiguration)
        {
            ActiveLevelData = levelData;
            _generalConf = generalConfiguration;

            RemainingMolesToHit = new Property<int>(levelData.TargetMoleHits);
            MissedMoles = new Property<int>();
            PlayerLives = new Property<int>(generalConfiguration.PlayerLives);

            _spawnPoints = GenerateSpawnPoints(levelData.HoleCount);
            SpawnMoles();

            SaveGame.Level = levelData.Level;

            _hammer.Appear();
            GameOn = true;
            Menu.Instance.Open<GamePlayWindow>();
        }

        public void LevelComplete()
        {
            int newLevel = ++SaveGame.Level;
            SaveGame.MaxLevel = Mathf.Max(newLevel, SaveGame.MaxLevel);

            LevelFinish(true);
        }
        public void LevelFail()
        {
            LevelFinish(false);
        }
        private void LevelFinish(bool won)
        {
            GameOn = false;
            _hammer.Disappear();

            _moles.ForEach(mole => mole.Disappear());
            _moles.Clear();

            Menu.Instance.Close<GamePlayWindow>();
            GameManager.Instance.LevelFinished(won);
        }

        public void GrantExtraLife()
        {
            if (GameOn)
            {
                PlayerLives.Value++;
            }
        }

        private void SpawnMoles()
        {
            _moles = new List<iAppearable>();

            foreach (var point in _spawnPoints)
            {
                var mole = PoolManager.Get<Mole>(_molesParent, true);
                mole.Init(point);
                _moles.Add(mole);
            }
        }

        private void OnMoleGotHit(Mole mole)
        {
            if (GameOn)
            {
                if (--RemainingMolesToHit.Value <= 0)
                {
                    LevelComplete();
                }
            }
        }
        private void OnMoleEscaped(Mole mole)
        {
            if (GameOn)
            {
                if (++MissedMoles.Value % _generalConf.LifeToMissedMoleRatio == 0)
                {
                    if (--PlayerLives.Value == 0)
                    {
                        LevelFail();
                    }
                }
            }
        }
    }
}