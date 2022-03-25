using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Utilities.ECS;


namespace SpawnerSystem
{
    public class EnemyGOC : MonoBehaviour, IConvertGameObjectToEntity
    {

        [HideInInspector] public Entity reference;
        public bool DestroyGO;

        [SerializeField] List<ItemSpawnData> LootTable;
        public uint numOfDropitems =1 ;
        public List<ItemSpawnData> Dropped { get; set; }


        // add a clean up Destory Component tag and system
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var c1 = new EnemyTag() { Destory = false };
            dstManager.AddComponentData(entity, c1);
                       reference = entity;
        }



    }



}