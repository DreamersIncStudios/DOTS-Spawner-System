using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpawnerSystem.ScriptableObjects;

namespace SpawnerSystem
{
    public class SpawnableSO : ScriptableObject, ISpawnable
    {
        [SerializeField]int _spawnID;

        public int SpawnID { get { return _spawnID; } set { _spawnID = value; } }
    }
}