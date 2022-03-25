using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpawnerSystem.Loot
{
    public class LootManager : MonoBehaviour
    {

        List<LootTableSO> lootTables;
        int tableIndex;
        float probabilityTotalWeight;

        public void Setup(List<LootTableSO> LootTables)
        {
            lootTables = new List<LootTableSO>();
            foreach (LootTableSO table in LootTables)
            {
                lootTables.Add(Instantiate(table));
            }
            CreateLootTable();
        }

        public void DropItems(int numOfDrop)
        {

            PickedTable().PickUpItem(numOfDrop, transform.position);
        }

        public void OpenCrest(int numOfDrop)
        {
            PickedTable().AddItemsToInventory(numOfDrop);

        }


        void CreateLootTable()
        {

            float currentProbabilityWeightMaximum = 0f;
            for (int cnt = 0; cnt < lootTables.Count; cnt++)
            {
                LootTableSO Drop = lootTables[cnt];
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

                lootTables[cnt] = Drop;
            }


            for (int cnt = 0; cnt < lootTables.Count; cnt++)
            {
                LootTableSO Drop = lootTables[cnt];
                Drop.probabilityPercent = ((Drop.probabilityWeight) / probabilityTotalWeight) * 100;
                lootTables[cnt] = Drop;

            }


        }

        LootTableSO PickedTable()
        {
            float pickedNumber = Random.Range(0, probabilityTotalWeight);
            // Find an item whose range contains pickedNumber
            foreach (LootTableSO Drop in lootTables)
            {

                // If the picked number matches the item's range, return item
                if (pickedNumber > Drop.probabilityRangeFrom && pickedNumber < Drop.probabilityRangeTo)
                {
                    return Drop;
                }

            }
            return null;
        }
    }
}