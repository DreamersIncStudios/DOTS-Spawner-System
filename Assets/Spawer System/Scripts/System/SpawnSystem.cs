using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.AI;
using SpawnerSystem.WorldLevel;
using Unity.Collections;
using Unity.Mathematics;


namespace SpawnerSystem
{
    [UpdateAfter(typeof(WaveSystem.WaveMasterSystem))]
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


            if (!SpawnControl.CanSpawn)
                return;
            else
            {
                switch (SpawnControl.ControlMode)
                {
                    case SpawnControlMode.Game:
                        SpawnGame();
                        break;
             
                }
            }
        }

        public void SpawnGame()
        {
            NativeArray<LocalToWorld> NPCPosition = GetEntityQuery(typeof(NpcTag), typeof(LocalToWorld)).ToComponentDataArray<LocalToWorld>(Allocator.TempJob);
            Entities.ForEach((ref WorldSpawn Areas ) => 
            {
                //Areas.CurrentNpcCount = 0;
                WorldSpawn TempArea = Areas;
                TempArea.CurrentNpcCount = 0;
                Entities.ForEach((ref NpcTag NPC, ref LocalToWorld Pos) => 
                {
                   float3  displacementFromCenterOfArea = Pos.Position - TempArea.CenterPostion;
                    if(Mathf.Abs(displacementFromCenterOfArea.x)< TempArea.MaxDisplacementFromCenter.x && Mathf.Abs(displacementFromCenterOfArea.y) < TempArea.MaxDisplacementFromCenter.y && Mathf.Abs(displacementFromCenterOfArea.z) < TempArea.MaxDisplacementFromCenter.z)
                        TempArea.CurrentNpcCount++;
                    
                });

                Areas = TempArea;

                if (Areas.CurrentNpcCount < Areas.MaxNpcCount)
                {
                    int SpawnCount = Areas.MaxNpcCount - Areas.CurrentNpcCount;

                    Entities.ForEach((Entity SPEntity, ref EnemySpawnTag Tag, ref LocalToWorld transform) =>
                    {
                        float3 displacementFromCenterOfArea = transform.Position - TempArea.CenterPostion;
                        if (Mathf.Abs(displacementFromCenterOfArea.x) < TempArea.MaxDisplacementFromCenter.x && Mathf.Abs(displacementFromCenterOfArea.y) < TempArea.MaxDisplacementFromCenter.y && Mathf.Abs(displacementFromCenterOfArea.z) < TempArea.MaxDisplacementFromCenter.z)
                        {
                            
                            DynamicBuffer<EnemySpawnData> Buffer = EntityManager.GetBuffer<EnemySpawnData>(SPEntity);
                            for (int cnt = 0; cnt < Buffer.Length; cnt++)
                            {
                                if (SpawnCount != 0)
                                {
                                    Enemy spawn = EnemyDatabase.GetEnemy(Buffer[cnt].spawnData.SpawnID);
                                    Object.Instantiate(spawn.GO, (Vector3)transform.Position + spawn.SpawnOffset, transform.Rotation);
                                    EnemySpawnData tempData = Buffer[cnt];
                                    tempData.spawnData.SpawnCount--;
                                    SpawnControl.CountInScene++;
                                    Buffer[cnt] = tempData;
                                    SpawnCount--;
                                }

                            }
                        }
                    });
                    
                    }
            });
        }

        bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
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

       

    }


}
