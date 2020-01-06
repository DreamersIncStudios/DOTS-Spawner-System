using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace SpawnerSystem
{
 public struct SpawnPointComponent : IComponentData {
        public bool Temporoary;
        

    }
    public struct PlayerSpawnTag : IComponentData{ }
    public struct EnemySpawnTag : IComponentData {
        public int MaxLevel;
    }
    public struct ItemSpawnTag : IComponentData { }
    public struct LootSpawnTag : IComponentData { }

    [System.Serializable]
    public struct EnemyGO : ISharedComponentData {
        public GameObject Enemy;

    }

}