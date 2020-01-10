using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;

namespace SpawnerSystem {

    public class WaveMasterSystem : ComponentSystem
    {


        public int wavecnt = 0;
        EntityManager mgr;
        protected override void OnCreate()
        {
            base.OnCreate();
            mgr = World.DefaultGameObjectInjectionWorld.EntityManager;
        }
        bool CurWave(WaveComponent wave) {
            return wave.Level == wavecnt;
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref WaveComponent wave) => {
                if (!CurWave(wave))
                    return;
                if (!wave.testOnce)
                {
                    DynamicBuffer<WaveBufferComponent> buffer = EntityManager.GetBuffer<WaveBufferComponent>(entity);
                    foreach (WaveBufferComponent waveEnemy in buffer)
                    {
                        int cnt = 0;
                        while (cnt < waveEnemy.EnemySpecForWave.SpawnCount)
                        {
                            Entities.ForEach((Entity entity2, ref SpawnPointComponent SPC, ref EnemySpawnTag Tag, ref LocalToWorld transform) =>
                            {
                                DynamicBuffer<EnemySpawnData> Buffer = EntityManager.GetBuffer<EnemySpawnData>(entity2);
                                if (!SPC.Temporoary)// To be removed
                            {
                                    foreach (EnemySpawnData Data in Buffer)
                                    {
                                        if (Data.SpawnID == waveEnemy.EnemySpecForWave.spawnID)
                                        {
                                            if (cnt == waveEnemy.EnemySpecForWave.SpawnCount)
                                                return;
                                            Object.Instantiate(EnemyDatabase.GetEnemy(Data.SpawnID).GO, transform.Position, transform.Rotation);
                                            // SPC.Temporoary = true;
                                            cnt++;
                                    }
                                    }

                                }
                            });

                        }
                        Debug.Log(cnt);

                    }
                    wave.testOnce = true;
                }
            });
        }

    }

}