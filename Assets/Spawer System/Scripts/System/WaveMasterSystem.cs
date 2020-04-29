using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Utilities.ECS;

namespace SpawnerSystem {

    public class WaveMasterSystem : ComponentSystem
    {
        /*  Find all the spawn points that can spawn this enemy

                     */
        public int wavecnt = 0;
        EntityManager mgr;
        protected override void OnCreate()
        {
            base.OnCreate();
            mgr = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        bool CurWave(WaveComponent wave)
        {
            return wave.Level == wavecnt;
        }

        bool WaveDefeat(WaveComponent wave)
        {
            if (wave.EnemiesDispatched > 0)
            {
                return wave.EnemiesDefeated == wave.EnemiesDispatched;
            }
            else
                return false;
        }
        SpawnController Control;
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref WaveComponent wave) =>
            {
                if (!CurWave(wave))
                    return;
                if (Control == null)
                    Control = SpawnController.Instance;
                if(Control.MaxCountInScene != wave.MaxEnemyAtOnce)
                    Control.MaxCountInScene = wave.MaxEnemyAtOnce;

                if (!wave.AllEnemiesDispatched)
                {
                    DynamicBuffer<WaveBufferComponent> WaveBuffer = EntityManager.GetBuffer<WaveBufferComponent>(entity);
                    foreach (WaveBufferComponent waveEnemy in WaveBuffer)
                    {
                        int count = waveEnemy.EnemySpecForWave.SpawnCount;
                        while (count != 0)
                        {
                            NativeArray<int> dispatched = new NativeArray<int>(1, Allocator.TempJob);
                            var testing = new DispatchSpawnsToSpawnPointsEnemy()
                            {
                                SpawnCount = count,
                                SpawnID = waveEnemy.EnemySpecForWave.spawnID,
                                count = dispatched,
 
                            };

                            JobHandle handle = testing.Schedule(this);
                            handle.Complete();

                            count -= testing.count[0];
                            wave.EnemiesDispatched += dispatched[0];
                            dispatched.Dispose();
                        }
                    }
                    wave.AllEnemiesDispatched = true;
                }
                WaveComponent currentwave = wave;
                Entities.ForEach((ref Destroytag tag, ref EnemyTag Enemy) => {
                    currentwave.EnemiesDefeated++;

                });
                wave = currentwave;
                if (WaveDefeat(wave))
                {
                    PostUpdateCommands.DestroyEntity(entity);
                    wavecnt++; }
            });
        }


 


    }


    public struct DispatchSpawnsToSpawnPointsEnemy : IJobForEach_BC<EnemySpawnData, EnemySpawnTag>
    {
        public int SpawnID;
        public int SpawnCount;
        public NativeArray<int> count;
        public void Execute(DynamicBuffer<EnemySpawnData> EnemyBuffer, ref EnemySpawnTag c1)
        {
            for (int cnt = 0; cnt < EnemyBuffer.Length; cnt++)
            {
                if (count[0] >= SpawnCount)
                    return;
                if (EnemyBuffer[cnt].spawnData.SpawnID == SpawnID)
                {
                    EnemySpawnData temp = EnemyBuffer[cnt];
                    temp.spawnData.SpawnCount++;
                    temp.spawnData.Spawn = true;
                    EnemyBuffer[cnt] = temp;
                    count[0]++;

                }
            }
        }
    }
}