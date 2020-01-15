using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Utilities.ECS;

namespace SpawnerSystem
{
    public class EnemyGOC : MonoBehaviour, IConvertGameObjectToEntity
    {

        Entity reference;
        public bool DestroyGO;
        // add a clean up Destory Component tag and system
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var c1 = new EnemyTag() { Destory = false };
            dstManager.AddComponentData(entity, c1);
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
            mgr.AddComponentData(reference, new Destroytag() { delay = 0.0f });
            SpawnController.Instance.CountinScene--;
            Destroy(this.gameObject);

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