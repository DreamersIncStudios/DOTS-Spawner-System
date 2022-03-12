using Unity.Entities;
using UnityEngine;

namespace SpawnerSystem
{
    [GenerateAuthoringComponent]
    [System.Serializable]
    public struct ItemSpawnData : IBufferElementData
    {
        [System.Serializable]
        public struct Data
        {
            public int SpawnID;
            public bool Spawn;
            public int SpawnCount;

            public float probabilityWeight;
            public float probabilityPercent { get; set; }
            public float probabilityRangeFrom { get; set; }
            public float probabilityRangeTo { get; set; }
        }
        public Data spawnData;

        public static implicit operator Data(ItemSpawnData e) { return e; }

        public static implicit operator ItemSpawnData(Data e) { return new ItemSpawnData { spawnData = e }; }
    }
}