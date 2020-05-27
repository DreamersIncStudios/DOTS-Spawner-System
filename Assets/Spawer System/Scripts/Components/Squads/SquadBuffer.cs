using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace SpawnerSystem.Squads {
    [GenerateAuthoringComponent]
    public struct SquadBuffer : IBufferElementData
    {
        public SquadMember squadMember;
        public static implicit operator int(SquadBuffer e) { return e; }
        public static implicit operator SquadBuffer(SquadMember e) { return new SquadBuffer { squadMember = e }; }
    }

    public struct SquadMember
    {
        Entity SquadMemeberEntity;
        int Level;
    } 
}
