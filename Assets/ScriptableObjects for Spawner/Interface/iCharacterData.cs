﻿namespace SpawnerSystem.ScriptableObjects
{
    public interface ICharacterData
    {
        string Name { get;  }
        uint Level { get;  }
        int BaseHealth { get; }
        int BaseMana { get; }



    }
}