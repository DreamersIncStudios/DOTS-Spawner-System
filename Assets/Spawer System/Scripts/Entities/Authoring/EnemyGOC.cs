using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Utilities.ECS;
using SpawnerSystem.ScriptableObjects;

namespace SpawnerSystem
{
    public class EnemyGOC : MonoBehaviour, IConvertGameObjectToEntity
    {

        Entity reference;
        public bool DestroyGO;

        [SerializeField] public List<ItemSpawnData> LootTable;
        public uint numOfDropitems =1;
        public List<ItemSpawnData> Dropped { get; set; }


        // add a clean up Destory Component tag and system
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var c1 = new EnemyTag() { Destory = false };
            dstManager.AddComponentData(entity, c1);
            DynamicBuffer<ItemSpawnData> Buffer = dstManager.AddBuffer<ItemSpawnData>(entity);
            foreach (ItemSpawnData drop in LootTable)
                Buffer.Add(drop);
            dstManager.AddComponentData(entity, new CreateLootTableTag());
            dstManager.AddComponentData(entity, new ProbTotal() { probabilityTotalWeight = 0.0f });
            reference = entity;
        }

        public void Update()
        {
            if (DestroyGO)
                DestroyEnemy();
        }
        void DestroyEnemy()
        {
            EntityManager mgr = World.DefaultGameObjectInjectionWorld.EntityManager;
            mgr.AddComponentData(reference, new SelectADropTag() { NumOfDrops=numOfDropitems});
            spawnItemDropSpawnPoint(mgr);   
            
            mgr.AddComponentData(reference, new Destroytag() { delay = 0.0f });
            SpawnController.Instance.CountinScene--;
            Destroy(this.gameObject);

        }

        void spawnItemDropSpawnPoint(EntityManager MGR) {


            Entity entity = MGR.CreateEntity();
            MGR.AddComponentData(entity, new SpawnPointComponent()
            {
                Temporoary = true,
                SpawnPointID = 10000
            });
            DynamicBuffer<ItemSpawnData> Buffer = MGR.AddBuffer<ItemSpawnData>(entity);

            // Change to a custom input.
            foreach (ItemSpawnData loot in Dropped)
            {
                Buffer.Add(loot);
            }
            MGR.AddComponentData(entity, new ItemSpawnTag());
            MGR.AddComponentData(entity, new LocalToWorld() );
            MGR.AddComponentData(entity, new Translation() { Value=this.transform.position});
            MGR.SetName(entity,"Loot Spawn Point");

        }

    }

    
    //public class test : ComponentSystem
    //{
    //    protected override void OnUpdate()
    //    {
    //        throw new System.NotImplementedException();
    //    }


    //}

}