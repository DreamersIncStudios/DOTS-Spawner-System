using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace SpawnerSystem
{
    public class EnemySpawnPoint : Spawner,IConvertGameObjectToEntity
    {
        public int MaxEnemyLevel;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        { }
    }
}