using UnityEngine;

namespace SpawnerSystem.ScriptableObjects
{
    public class GenericNPC : SpawnableSO,ICharacterBase
    {
        [SerializeField] string _name;
        public Color BaseColor;
        public string Name { get { return _name; } }

        public void Spawn( Vector3 Pos) { 
            GameObject temp = Instantiate(GO,Pos,Quaternion.identity);


        }
    }
}