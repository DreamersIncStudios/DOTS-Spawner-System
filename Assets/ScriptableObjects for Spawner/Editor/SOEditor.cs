using UnityEditor;

namespace SpawnerSystem.Editors
{

    static public class SOEditor
    {
        [MenuItem("Assets/Create/RPG/Enemy")]

        static public void CreateEnemy() {
            Enemy enemy;
            ScriptableObjectUtility.CreateAsset<Enemy>("Enemy" ,  out enemy);
            EnemyDatabase.LoadDatabaseForce();
            enemy.SpawnID = EnemyDatabase._Enemies.Count + 1;
        }
    }
}