using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpawnerSystem.ScriptableObjects
{
    public class SquadSO : ScriptableObject,ISpawnable, iSquad
    {

        [SerializeField] int _spawnID;
        [SerializeField] int leaderID;
        [SerializeField] int backupLeaderID;
        [SerializeField] List<SquadMemberID> squadMemberID;
        public int SpawnID { get { return _spawnID; } set { _spawnID = value; } }

        public Vector3 SpawnOffset { get { return Vector3.zero; } }

        public int LeaderID { get { return leaderID; }  }

        public int BackupLeaderID { get { return backupLeaderID; } }

        public List<SquadMemberID> SquadMemberID { get { return squadMemberID; }}

        public void Spawn(Vector3 Position) {
            GameObject leaderGO = Instantiate(EnemyDatabase.GetEnemy(leaderID).GO, Position, Quaternion.identity);
            GameObject BackupGO = Instantiate(EnemyDatabase.GetEnemy(BackupLeaderID).GO, Position, Quaternion.identity);

            LeaderComponent test = leaderGO.AddComponent<LeaderComponent>();
            test.BackupLeader = BackupGO;
            

        }
    }

    public interface iSquad
    {
        int LeaderID { get;}
        int BackupLeaderID { get;}
        List<SquadMemberID> SquadMemberID { get; }
    }
    [System.Serializable]

    // pass a list of squad gameobjects. Write a componentsystem or Ijob or method to get entities 
    public struct SquadMemberID{
        public int ID;
        public int NumberOfSpawns;
    }

}