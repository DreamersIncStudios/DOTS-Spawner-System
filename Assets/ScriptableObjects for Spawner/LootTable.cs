using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
namespace SpawnerSystem
{
    //[System.Serializable]
    //public struct LootDropItem : IBufferElementData {
    //    public int spawnID;
    //    public float probabilityWeight;
    //    [HideInInspector]
    //    public float probabilityPercent;
    //    [HideInInspector]
    //    public float probabilityRangeFrom;
    //    [HideInInspector]
    //    public float probabilityRangeTo;

    //}
    public struct CreateLootTableTag : IComponentData {
        [HideInInspector]
        public float probabilityTotalWeight;
    }
    public struct SelectADropTag : IComponentData
    {
        public uint NumOfDrops;
    }

    public struct ProbTotal : IComponentData {
        public float probabilityTotalWeight;
    }

    public class DropSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, EnemyGOC GO, ref SelectADropTag DropInfo, ref ProbTotal prob) => 
            {
                DynamicBuffer<ItemSpawnData> Buffer = EntityManager.GetBuffer<ItemSpawnData>(entity);

                for (int cnt = 0; cnt < DropInfo.NumOfDrops; cnt++)
                {
                    float pickedNumber = Random.Range(0, prob.probabilityTotalWeight);
                    GO.Dropped.Add(Buffer[0]);
                    // Find an item whose range contains pickedNumber
                    foreach (ItemSpawnData Drop in Buffer)
                    {
                        // If the picked number matches the item's range, return item
                        if (pickedNumber > Drop.probabilityRangeFrom && pickedNumber < Drop.probabilityRangeTo)
                        {
                            GO.Dropped.Add(Drop);
                            return;
                        }

                    }
                }
                PostUpdateCommands.RemoveComponent<SelectADropTag>(entity);
            });
        }
    }


    public class LootSystem : JobComponentSystem
    {
        public EndSimulationEntityCommandBufferSystem end;

        protected override void OnCreate()
        {
            base.OnCreate();
            end = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
         
        public struct CreateDropTable : IJobForEachWithEntity_EBCC<ItemSpawnData, CreateLootTableTag,ProbTotal>
        {
             public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute(Entity entity, int index, DynamicBuffer<ItemSpawnData> DropItems, ref CreateLootTableTag Enemy, ref ProbTotal prob)
            {
                float currentProbabilityWeightMaximum = 0f;
             

                for (int cnt = 0; cnt < DropItems.Length; cnt++)
                {
                    ItemSpawnData Drop = DropItems[cnt];
                    if (Drop.probabilityWeight <= 0)
                    {
                        Debug.LogWarning("Loot Drop Item Not set up");
                        Drop.probabilityWeight = 0f;
                    }
                    else
                    {
                        Drop.probabilityRangeFrom = currentProbabilityWeightMaximum;
                        currentProbabilityWeightMaximum += Drop.probabilityWeight;
                        Drop.probabilityRangeTo = currentProbabilityWeightMaximum;
                    }
                    prob.probabilityTotalWeight = currentProbabilityWeightMaximum;

                    DropItems[cnt] = Drop;
                }


                for (int cnt = 0; cnt < DropItems.Length; cnt++)
                {
                    ItemSpawnData    Drop = DropItems[cnt];
                    Drop.probabilityPercent = ((Drop.probabilityWeight) / prob.probabilityTotalWeight) * 100;
                    DropItems[cnt] = Drop;
                }
                CommandBuffer.RemoveComponent<CreateLootTableTag>(index, entity);
                

            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var create = new CreateDropTable() {CommandBuffer = end.CreateCommandBuffer().ToConcurrent()};
            JobHandle job = create.Schedule(this, inputDeps);
            end.AddJobHandleForProducer(job);
            return job;
        }

    }
}