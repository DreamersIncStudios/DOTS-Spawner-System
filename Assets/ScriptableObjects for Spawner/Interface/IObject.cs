using UnityEngine;

namespace SpawnerSystem.ProofofConcept
{
    public interface ISpawnable {
        int SpawnID { get; set; }
    }

    public interface IObject 
    {
        GameObject GO { get; }
        Vector3 Scale { get;}
    }
}