using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpawnerSystem.ScriptableObjects;

namespace SpawnerSystem.Test
{
    public class SquadSpawn : MonoBehaviour
    {
        public SquadSO Squad1;
        // Start is called before the first frame update
        void Start()
        {
            Squad1.Spawn(this.transform.position);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}