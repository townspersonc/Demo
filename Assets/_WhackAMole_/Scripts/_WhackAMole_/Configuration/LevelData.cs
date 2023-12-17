using System;

namespace WhackAMole
{
    [Serializable]
    public struct LevelData
    {
        public int Level;
        public int Time;
        public int HoleCount;
        public int TargetMoleHits;

        public LevelData(int level, int time, int holeCount, int targetMoleHits)
        {
            Level = level;
            Time = time;
            HoleCount = holeCount;
            TargetMoleHits = targetMoleHits;
        }
    }
}