using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpawnerSystem.ScriptableObjects;

namespace SpawnerSystem
{
    public class SpawnableSO : ScriptableObject, ISpawnable
    {
        [SerializeField] int _spawnID;
        [SerializeField] Vector3 _spawnOffset;
        public int SpawnID { get { return _spawnID; } set { _spawnID = value; } }
        public Vector3 SpawnOffset { get { return _spawnOffset; } }
    }
}