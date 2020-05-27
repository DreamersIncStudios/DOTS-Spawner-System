using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
namespace SpawnerSystem.Squads
{

    public interface BaseComponent : IComponentData
    {
        int Level { get; set; }
        bool IsLeader { get; set; }
    }
}
