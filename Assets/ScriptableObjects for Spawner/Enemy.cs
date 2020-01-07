using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpawnerSystem.ProofofConcept;
namespace SpawnerSystem {
    public class Enemy : SpawnableSO, IObject, ICharacterData
    {
        [SerializeField] string _name;
        [SerializeField] uint _level;
        [SerializeField] int _baseHealth;
        [SerializeField] int _baseMana;
        [SerializeField] GameObject _GO;
        [SerializeField] Vector3 _scale;

        public GameObject GO { get { return _GO; }  }
        public Vector3 Scale { get { return _scale; } }
        public string Name { get { return _name; } }
        public uint Level { get { return _level; } }
        public int BaseHealth { get { return _baseHealth; } }
        public int BaseMana { get { return _baseMana; } }
    }
}