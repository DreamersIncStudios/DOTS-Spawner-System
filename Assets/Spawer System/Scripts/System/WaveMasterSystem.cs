using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Utilities.ECS;

namespace SpawnerSystem.WaveSystem {

    public class WaveMasterSystem : ComponentSystem
    {
        /*  Rewrite with spawn tag

                     */
        public int wavecnt = 1;
        EntityManager mgr;
        protected override void OnCreate()
        {
            base.OnCreate();
            mgr = World.DefaultGameObjectInjectionWorld.EntityManager;
            m_Group = GetEntityQuery( typeof(EnemySpawnTag));

        }

        bool CurWave(WaveBuffer wave)
        {
            return wave.spawnData.Level == wavecnt;
        }


        SpawnController Control;
        EntityQuery m_Group;
        bool StartNewWave = true;
       public  int EnemiesInWave;
        public int EnemiesDefeat;


        protected override void OnUpdate()
        {
            if (Control == null)
                Control = SpawnController.Instance;
            Entities.ForEach(( DynamicBuffer<WaveBuffer> waveBuffer, ref BaseEnemySpecsForWave baseEnemy) =>
                {
                    if (StartNewWave)
                    {
                     
                        foreach (WaveBuffer wave in waveBuffer)
                        {
                            if (CurWave(wave))
                            {
                                int count = wave.spawnData.SpawnCount;
                             
                                while (count != 0)
                                {
                                    NativeArray<int> dispatched = new NativeArray<int>(1, Allocator.TempJob);
                                    var testing = new DispatchSpawnsToSpawnPointsEnemy()
                                    {
                                        SpawnCount = count,
                                        SpawnID = baseEnemy.EnemyId,
                                        count = dispatched,
                                        chunkEnemyBuffer = GetArchetypeChunkBufferType<EnemySpawnData>(),
                                        C1 = GetArchetypeChunkComponentType<EnemySpawnTag>()
                                    };

                                    JobHandle handle = testing.Schedule(m_Group);
                                    handle.Complete();

                                    count -= testing.count[0];
                                   
                                    dispatched.Dispose();
                                }
                                EnemiesInWave += wave.spawnData.SpawnCount;
                            }


                        }
                    }
            });
            StartNewWave = false;
            // write logic for spawning wave next 
           
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