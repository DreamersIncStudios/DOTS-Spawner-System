using UnityEngine;
using System.Collections;

namespace SpawnerSystem.ScriptableObjects
{
    public class RecoveryItemSO : Droppable, iRecoverable,IObject
    {
        [SerializeField] RecoverType Type;
        [SerializeField] int recoveryAmount;
        [SerializeField] GameObject _GO;
        [SerializeField] Vector3 _scale;

        public GameObject GO { get { return _GO; } }
        public Vector3 Scale { get { return _scale; } }
        public RecoverType recoverType { get { return Type; } }
    
        public int RecoverAmount { get { return recoveryAmount; } }

        public override void Spawn(Vector3 Pos) {
            Instantiate(GO,Pos,Quaternion.identity);
            PickupRecoverItem PRI = GO.AddComponent<PickupRecoverItem>();
            PRI.RecoverWhat = recoverType;
            PRI.Amount = RecoverAmount;
        }

    }

}