using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace SpawnerSystem.ScriptableObjects
{
    public class LeaderComponent : MonoBehaviour,IConvertGameObjectToEntity
    {
        public GameObject BackupLeader;
        public List<SquadMemberBuffer> Squad;
        public Entity Self { get { return selfRef; } }
        Entity selfRef;
        public LeaderComponent() { }
        public LeaderComponent(GameObject Back, List<SquadMemberBuffer> squad) 
        {
            BackupLeader = Back;
            Squad = squad;
        }
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var leader = new LeaderTag() {};

            dstManager.AddComponentData(entity, leader);
            dstManager.AddBuffer<SquadMemberBuffer>(entity);
            selfRef = entity;
        }

 
    }
    public struct LeaderTag : IComponentData
    {
       public Entity BackupLeader;

    }

    // all npc need a reference to self entity for squad grouping


    public struct SquadMemberBuffer : IBufferElementData
    {
        public Entity SquadMember;

    }


   public class SquadUP : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref LeaderTag Tag, LeaderComponent Leader) => {
                if(Leader.BackupLeader !=null)
                Tag.BackupLeader = Leader.BackupLeader.GetComponent<EnemyGOC>().reference;
            
            });
        }
    }
}

