using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace SpawnerSystem
{
    public class EnemySpawnPoint : MonoBehaviour,IConvertGameObjectToEntity
    {
        public int MaxEnemyLevel;

        public GameObject[] EnemiesAvailableForSpawn;
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var Spawnpoint = new SpawnPointComponent();
            dstManager.AddComponentData(entity, Spawnpoint);
            var EnemySpawn = new EnemySpawnTag() { MaxLevel = MaxEnemyLevel };
            dstManager.AddComponentData(entity, EnemySpawn);

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}