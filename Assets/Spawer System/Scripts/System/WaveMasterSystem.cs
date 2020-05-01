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
        /*  Rewrite with spawn tag

                     */
        public int wavecnt = 0;
        EntityManager mgr;
        protected override void OnCreate()
        {
            base.OnCreate();
            mgr = World.DefaultGameObjectInjectionWorld.EntityManager;
            m_Group = GetEntityQuery( typeof(EnemySpawnTag));

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
        EntityQuery m_Group;
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
                                chunkEnemyBuffer = GetArchetypeChunkBufferType<EnemySpawnData>(),
                                C1= GetArchetypeChunkComponentType<EnemySpawnTag>()
                            };

                            JobHandle handle= testing.Schedule(m_Group);
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

    [Unity.Burst.BurstCompile]
    public struct DispatchSpawnsToSpawnPointsEnemy : IJobChunk
    {
        public int SpawnID;
        public int SpawnCount;
        public NativeArray<int> count;
        public ArchetypeChunkBufferType<EnemySpawnData> chunkEnemyBuffer;
        public ArchetypeChunkComponentType<EnemySpawnTag> C1;
        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var EnemyBuffer2 = chunk.GetBufferAccessor(chunkEnemyBuffer);

            for (int cnt = 0; cnt < EnemyBuffer2.Length; cnt++)
            {
                DynamicBuffer<EnemySpawnData> EnemyBuffer = EnemyBuffer2[cnt];
                for (int i = 0; i < EnemyBuffer.Length; i++)
                {
                    if (count[0] >= SpawnCount)
                        return;
                    if (EnemyBuffer[i].spawnData.SpawnID == SpawnID)
                    {
                        EnemySpawnData temp = EnemyBuffer[i];
                        temp.spawnData.SpawnCount++;
                        temp.spawnData.Spawn = true;
                        EnemyBuffer[i] = temp;
                        count[0]++;

                    }
                }
            }
        }

      
    }
}