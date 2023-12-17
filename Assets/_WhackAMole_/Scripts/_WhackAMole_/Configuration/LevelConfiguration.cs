using UnityEngine;

namespace WhackAMole
{
    [System.Serializable]
    public struct LevelConfiguration
    {
        [SerializeField] private LevelData Easy, Hard;

        public LevelData GetLevelData(int forLevel)
        {
            float fromZeroToOne = Mathf.InverseLerp(Easy.Level, Hard.Level, forLevel);

            return new LevelData
            (
                level: forLevel,
                holeCount: (int)Mathf.Lerp(Easy.HoleCount, Hard.HoleCount, fromZeroToOne),
                time: (int)Mathf.Lerp(Easy.Time, Hard.Time, fromZeroToOne),
                targetMoleHits: (int)Mathf.Lerp(Easy.TargetMoleHits, Hard.TargetMoleHits, fromZeroToOne)
            );
        }
    }
}