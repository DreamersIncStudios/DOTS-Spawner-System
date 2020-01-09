using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;


namespace SpawnerSystem
{
    public class SpawnMaster : MonoBehaviour   //,IConvertGameObjectToEntity
    {
        public List<Wave> EnemyWaves;
        public int CurLevel;

        //public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        //{
        //    DynamicBuffer<WaveBufferComponent> Waves = dstManager.AddBuffer<WaveBufferComponent>(entity);
        //    foreach (Wave wave in EnemyWaves)
        //    {
        //        Waves.Add(new WaveBufferComponent { _wave = wave });
        //    }
        //}

    }

    public class testjobsystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref SpawnPointComponent SPC, ref EnemySpawnTag Tag, ref LocalToWorld transform) =>
            {
                DynamicBuffer<EnemySpawnData> Buffer = EntityManager.GetBuffer<EnemySpawnData>(entity);
                if (SPC.Temporoary)
                    return;
               // EnemyDatabase.LoadDatabaseForce
              
                 Object.Instantiate(EnemyDatabase.GetEnemy(Buffer[0].SpawnID).GO,transform.Position, transform.Rotation);
                SPC.Temporoary = true;
            });
        }
    }
}