﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerSystem
{
    
 static public class EnemyDatabase
    {
        static public List<Enemy> _Enemies;
        static public bool IsLoaded{get; private set;} = false;

        static private void ValidateDatebase() {
            if (_Enemies == null) _Enemies = new List<Enemy>();
        }

        static public void LoadDatabase() {
            if (IsLoaded)
                return;
            LoadDatabaseForce();
        }
        static public void LoadDatabaseForce() {
            ValidateDatebase();
            IsLoaded = true;
            Enemy[] resources = Resources.LoadAll<Enemy>(@"Enemy");
            foreach (Enemy enemy in resources)
            {
                if (!_Enemies.Contains(enemy))
                    _Enemies.Add(enemy);
            }
        }

        static public void ClearDatabase() {
            IsLoaded = false;
            _Enemies.Clear();
        }

        static public Enemy GetEnemy(int SpawnID) {
            ValidateDatebase();
            LoadDatabase();
            foreach (Enemy enemy in _Enemies)
            {
                if (enemy.SpawnID == SpawnID)
                    return ScriptableObject.Instantiate(enemy) as Enemy;    
            }
            return null;
        }
    }


}