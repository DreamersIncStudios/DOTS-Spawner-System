using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpawnerSystem.Loot
{
    public class LootManager : MonoBehaviour
    {
        public List<LootTableSO> LootTables;
        int tableIndex;
        void PickLootTableAndNumDrops() { 
        
        }

        public void DropItems(int numOfDrop) {

            LootTables[tableIndex].PickUpItem(numOfDrop, transform.position);
        }

        public void OpenCrest(int numOfDrop) { 
            LootTables[tableIndex].AddItemsToInventory(numOfDrop);

        }

    }
}