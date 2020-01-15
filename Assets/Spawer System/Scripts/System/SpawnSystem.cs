using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;

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

            if (!SpawnControl.CanSpawn)
                return;

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

        private void SpawnWave()
        {

            Entities.ForEach((Entity SPEntity, ref EnemySpawnTag Tag, ref LocalToWorld transform) =>
            {
                DynamicBuffer<EnemySpawnData> Buffer = EntityManager.GetBuffer<EnemySpawnData>(SPEntity);
                for(int cnt=0;cnt<Buffer.Length;cnt++)
                {
                    if (!SpawnControl.CanSpawn)
                        return;
                    if (Buffer[cnt].Spawn) {
                        Object.Instantiate(EnemyDatabase.GetEnemy(Buffer[cnt].SpawnID).GO, transform.Position, transform.Rotation);
                        EnemySpawnData tempData = Buffer[cnt];
                        tempData.SpawnCount--;
                        if (tempData.SpawnCount == 0)
                            tempData.Spawn = false;
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
    }

}
