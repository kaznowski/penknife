using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleDash.CodingTools;

using DoubleDash.CodingTools.GenericDatabases;

namespace DoubleDash.ComponentManagement.CinemachineManagement
{
    public class CinemachineVirtualCameraPriorityDatabaseMono : MonoBehaviour, IVariable<CinemachineVirtualCameraPriorityDatabase>, ICinemachineVirtualCameraPriorityDatabase
    {        
        [SerializeField] CinemachineVirtualCameraPriorityDatabase _database;

        public CinemachineVirtualCameraPriorityDatabase Value { get => _database; set => _database = value; }


        public void ResetPriority()
        {
            _database.ResetPriority();
        }
    }
}
