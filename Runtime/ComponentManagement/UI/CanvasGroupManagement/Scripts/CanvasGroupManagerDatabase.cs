using DoubleDash.CodingTools.GenericDatabases;
using UnityEngine;
using DoubleDash.CodingTools;

namespace DoubleDash.ComponentManagement.UIManagement
{
    public class CanvasGroupManagerDatabase : MonoBehaviour, IVariable<Database<CanvasGroupManager>>
    {
        [SerializeField]
        Database<CanvasGroupManager> _database = new CodingTools.GenericDatabases.Database<CanvasGroupManager>();

        public Database<CanvasGroupManager> Value { get => _database; set => _database = value; }
    }
}