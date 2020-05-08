using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
namespace SpawnerSystem.WorldLevel
{
    [GenerateAuthoringComponent]
    public struct WorldSpawn : IComponentData
    {
        public int Raduis;
        public uint3 MaxDisplacementFromCenter; //
        public float3 CenterPostion;
        public int MaxNpcCount;
        public int CurrentNpcCount;

        public int CurrentEnemyNPCCount;


    }


}