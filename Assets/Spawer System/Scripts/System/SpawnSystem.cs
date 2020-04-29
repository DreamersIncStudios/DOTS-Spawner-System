using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using UnityEngine.AI;


namespace SpawnerSystem
{
    [UpdateAfter(typeof(WaveMasterSystem))]
    public class SpawnSystem : ComponentSystem
    {
        public SpawnController SpawnControl;
        protected override void OnCreate()
        {
            base.OnCreate();
            SpawnControl = SpawnController.Instance;
        }
        
        protected override void OnUpdate()
        {
            if(SpawnControl==null)
            SpawnControl = SpawnController.Instance;

            SpawnDropsItem();

            if (!SpawnControl.CanSpawn)
                return;
            else
            {
                switch (SpawnControl.ControlMode)
                {
                    case SpawnControlMode.Game:
                        SpawnGame();
                        break;
                    case SpawnControlMode.WaveGen:
                        SpawnWave();
                        break;
                }
            }
        }

        private void SpawnWave()
        {

            Entities.ForEach((Entity SPEntity, ref EnemySpawnTag Tag, ref LocalToWorld transform) =>
            {
                DynamicBuffer<EnemySpawnData> Buffer = EntityManager.GetBuffer<EnemySpawnData>(SPEntity);
                for(int cnt=0;cnt<Buffer.Length;cnt++)
                {
                    if (!SpawnControl.CanSpawn)
                        return;
                    if (Buffer[cnt].spawnData.Spawn) {
                        Object.Instantiate(EnemyDatabase.GetEnemy(Buffer[cnt].spawnData.SpawnID).GO, transform.Position, transform.Rotation);
                        EnemySpawnData tempData = Buffer[cnt];
                        tempData.spawnData.SpawnCount--;
                        if (tempData.spawnData.SpawnCount == 0)
                            tempData.spawnData.Spawn = false;
                        Buffer[cnt] = tempData;
                        SpawnControl.CountinScene++;
      
                    }
                }
            });

        }
        public void SpawnGame()
        {
            //TBD
        }

        bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = center + Random.insideUnitSphere * range;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }
            result = Vector3.zero;
            return false;
        }

        public void SpawnDropsItem() {
            Entities.ForEach((Entity entity, ref SpawnPointComponent SPC, ref ItemSpawnTag Item, ref LocalToWorld Pos) => 
            {
                DynamicBuffer<ItemSpawnData> Buffer = EntityManager.GetBuffer<ItemSpawnData>(entity);
            // Loot Table job 
            Vector3 point;
                if (RandomPoint(Pos.Position, Item.spawnrange, out point))
                {
                    ItemDatabase.GetItem(Buffer[0].spawnData.SpawnID).Spawn(point);

                    EntityManager.DestroyEntity(entity);
                }
            });


        }
    }


}
