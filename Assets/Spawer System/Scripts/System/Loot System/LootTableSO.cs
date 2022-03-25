using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SpawnerSystem.Loot
{
    [CreateAssetMenu(fileName = "LootTable", menuName = "ScriptableObjects/Loot Table", order = 1)]
    public class LootTableSO : ScriptableObject
    {
        public List<LootDrop> ItemsToDrop;

        public float probabilityWeight;
        public float probabilityPercent { get; set; }
        public float probabilityRangeFrom { get; set; }
        public float probabilityRangeTo { get; set; }

        float probabilityTotalWeight;
        /// <summary>
        /// Items to be random drop in Game world when GO is destroyed.
        /// </summary>
        /// <param name="NumOfDrops"></param>
        /// <param name="SpawnPos"></param>
        public void PickUpItem(int NumOfDrops, Vector3 SpawnPos) {
            // CreateLootTable();

            foreach (LootDrop Item in DroppedItems(NumOfDrops))
            {

                //if (RandomPoint(Pos.Position, Tag.spawnrange, out Vector3 point))
                //{
                //    ItemDatabase.GetItem(Item.spawnData.SpawnID).Spawn(point);
                //}
            }

        }
        /// <summary>
        /// Items to be added to Player Inventory when Chest is open or decode item
        /// </summary>
        /// <param name="NumOfDrops"></param>
        public void AddItemsToInventory(int NumOfDrops) {
            foreach (LootDrop Item in DroppedItems(NumOfDrops))
            {

                //if (RandomPoint(Pos.Position, Tag.spawnrange, out Vector3 point))
                //{
                //    ItemDatabase.GetItem(Item.spawnData.SpawnID).Spawn(point);
                //}
            }
        }
        void CreateLootTable() {

            float currentProbabilityWeightMaximum = 0f;
            for (int cnt = 0; cnt < ItemsToDrop.Count; cnt++)
            {
                LootDrop Drop = ItemsToDrop[cnt];
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
                probabilityTotalWeight = currentProbabilityWeightMaximum;

                ItemsToDrop[cnt] = Drop;
            }


            for (int cnt = 0; cnt < ItemsToDrop.Count; cnt++)
            {
                LootDrop Drop = ItemsToDrop[cnt];
                Drop.probabilityPercent = ((Drop.probabilityWeight) / probabilityTotalWeight) * 100;
                ItemsToDrop[cnt] = Drop;

            }


        }
        private void OnValidate()
        {
            CreateLootTable();
        }

        List<LootDrop> DroppedItems( int NumOfDrops) {
            List<LootDrop> Dropped = new List<LootDrop>();

            for (int cnt = 0; cnt < NumOfDrops; cnt++)
            {
                float pickedNumber = Random.Range(0, probabilityTotalWeight);

                // Find an item whose range contains pickedNumber
                foreach (LootDrop Drop in ItemsToDrop)
                {

                    // If the picked number matches the item's range, return item
                    if (pickedNumber > Drop.probabilityRangeFrom && pickedNumber < Drop.probabilityRangeTo)
                    {
                        Dropped.Add(Drop);
                        if (Dropped.Count >= NumOfDrops)
                            break;

                    }

                }
            }
            return Dropped;
        }
        bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }
            result = Vector3.zero;
            return false;
        }

        [System.Serializable]
        public struct LootDrop
        {
            public ScriptableObject Item;

            public float probabilityWeight;
            public float probabilityPercent { get; set; }
            public float probabilityRangeFrom { get; set; }
            public float probabilityRangeTo { get; set; }
        }

    }
}
