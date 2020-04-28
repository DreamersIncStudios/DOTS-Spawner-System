using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace SpawnerSystem
{
 public struct SpawnPointComponent : IComponentData {
        public bool Temporoary;
        public uint SpawnPointID;


    }
    public struct PlayerSpawnTag : IComponentData{ }
    public struct EnemySpawnTag : IComponentData {
        public int MaxLevel;
    }
    public struct ItemSpawnTag : IComponentData
    {
        public int spawnrange { get { return 5; } }
    }
    public struct SpawnTag : IComponentData { }


    [System.Serializable]
    public struct EnemySpawnData:IBufferElementData {
        public int SpawnID;
        public bool Spawn;
        public int SpawnCount;

        public static implicit operator int(EnemySpawnData e) { return e; }

        public static implicit operator EnemySpawnData(int e) { return new EnemySpawnData { SpawnID = e }; }
    }

    [System.Serializable]
    public struct ItemSpawnData : IBufferElementData
    {
        public int SpawnID;
        public bool Spawn;
        public int SpawnCount;

        public float probabilityWeight;
        [HideInInspector]
        public float probabilityPercent;
        [HideInInspector]
        public float probabilityRangeFrom;
        [HideInInspector]
        public float probabilityRangeTo;

        public static implicit operator int(ItemSpawnData e) { return e; }

        public static implicit operator ItemSpawnData(int e) { return new ItemSpawnData { SpawnID = e }; }
    }
}