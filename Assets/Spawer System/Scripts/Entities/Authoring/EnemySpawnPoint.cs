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
        {
            var Spawnpoint = new SpawnPointComponent() { SpawnPointID= SpawnPointID};
            dstManager.AddComponentData(entity, Spawnpoint);
            var EnemySpawn = new EnemySpawnTag() { MaxLevel = MaxEnemyLevel };
            dstManager.AddComponentData(entity, EnemySpawn);
            DynamicBuffer<EnemySpawnData> buffer =  dstManager.AddBuffer<EnemySpawnData>(entity);

            foreach (int SpawnID in SpawnIDList)
            {
                buffer.Add(new EnemySpawnData() { SpawnID = SpawnID});
            }

        }

    }
}