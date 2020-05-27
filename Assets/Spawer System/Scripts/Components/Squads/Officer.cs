using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
namespace SpawnerSystem.Squads
{
    [GenerateAuthoringComponent]
    public struct Officer : BaseComponent
    {
        int level;

        public int Level
        {
            get { return level; }
            set
            {
                if (value <= 9 && value > 0)
                { level = value; }
                else
                {
                    Debug.LogWarning("Invalid level passed to Entity. NPC level set to 1");
                    level = 1;
                }
            }
        }
        public bool IsLeader { get; set; }
    }
}