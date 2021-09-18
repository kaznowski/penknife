using UnityEngine;
using Cinemachine;
using DoubleDash.CodingTools;

namespace DoubleDash.ComponentManagement.CinemachineManagement
{
    public class CinemachineVirtualCameraConfinerDatabaseMono : MonoBehaviour, IVariable<CinemachineVirtualCameraConfinerDatabase>, ICinemachineVirtualCameraConfinerDatabase
    {
        [SerializeField] CinemachineVirtualCameraConfinerDatabase _database;

        public CinemachineVirtualCameraConfinerDatabase Value { get => _database; set => _database = value; }

        public void SetConfineMode(CinemachineConfiner.Mode mode) => _database.SetConfineMode(mode);

        public void SetConfiner(Collider boundingVolume) => _database.SetConfiner(boundingVolume);

        public void SetConfiner2D(Collider2D boundingShape) => _database.SetConfiner2D(boundingShape);

        public void RemoveAllConfiners() => _database.RemoveAllConfiners();

        public void RemoveConfiner() => _database.RemoveConfiner();

        public void RemoveConfiner2D() => _database.RemoveConfiner2D();

        public void SetDamping(int i) => _database.SetDamping(i);
    }
}
