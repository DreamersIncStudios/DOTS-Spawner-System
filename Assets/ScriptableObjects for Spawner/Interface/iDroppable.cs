﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerSystem.ScriptableObjects
{
    public interface iDroppable
    {
        // How many units the item takes - more units, higher chance of being picked
        float probabilityWeight { get; set; }

        // Displayed only as an information for the designer/programmer. Should not be set manually via inspector!    
        float probabilityPercent { get; set; }

        // These values are assigned via LootDropTable script. They represent from which number to which number if selected, the item will be picked.
        float probabilityRangeFrom { get; set; }
        float probabilityRangeTo { get; set; }
    }
    public abstract class Droppable : ScriptableObject, iDroppable,ISpawnable
    {
        [SerializeField] int _spawnID;

        public int SpawnID { get { return _spawnID; } set { _spawnID = value; } }

        // To Be Added Later
        // removing 
        public float probabilityWeight { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float probabilityPercent { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float probabilityRangeFrom { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float probabilityRangeTo { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public virtual void Spawn(Vector3 Pos) { }
    }
}