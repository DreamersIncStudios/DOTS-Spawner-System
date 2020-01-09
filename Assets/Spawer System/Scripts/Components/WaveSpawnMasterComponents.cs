using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Collections;



namespace SpawnerSystem
{
    [System.Serializable]
    public struct Wave
    {
        public int WaveNumber;
        public List<EnemyWaveSpec> EnemiesForWave;
        public int RewardGold;
        public int RewardEXP;
        public int RewardSpawnID;
        
    }
    [System.Serializable]
    public struct EnemyWaveSpec {
        public int spawnID;
        public int SpawnCount;
        public int level;
    }
    //public struct WaveBufferComponent : IBufferElementData {
    //    public Wave _wave;

    //    public static implicit operator Wave(WaveBufferComponent WBC) { return WBC; }
    //    public static implicit operator WaveBufferComponent(Wave _w) { return new WaveBufferComponent { _wave = _w }; }
    //}
}