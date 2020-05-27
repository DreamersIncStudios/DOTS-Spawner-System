﻿using UnityEditor;
using UnityEngine;
using SpawnerSystem.ScriptableObjects;


namespace SpawnerSystem.Editors
{

    static public class SOEditor
    {
        [MenuItem("Assets/Create/RPG/Enemy")]

        static public void CreateEnemy()
        {
            Enemy enemy;
            ScriptableObjectUtility.CreateAsset<Enemy>("Enemy", out enemy);
            EnemyDatabase.LoadDatabaseForce();
            enemy.SpawnID = EnemyDatabase._Enemies.Count + 1;
            enemy.Scale = Vector3.one;
        }

        [MenuItem("Assets/Create/RPG/Recovery Item")]

        static public void CreateRecoveryItem()
        {
            RecoveryItemSO Item;
            ScriptableObjectUtility.CreateAsset<RecoveryItemSO>("Item", out Item);
           ItemDatabase.LoadDatabaseForce();
            Item.SpawnID = ItemDatabase.droppables.Count + 1;
        }

        [MenuItem("Assets/Create/RPG/Squad")]
        static public void CreateSquadSO()
        {
            SquadSO squad;
            ScriptableObjectUtility.CreateAsset<SquadSO>("Squad", out squad);
            SquadDatabase.LoadDatabaseForce();
            squad.SpawnID = SquadDatabase.Squads.Count + 1;
        }

    }
}