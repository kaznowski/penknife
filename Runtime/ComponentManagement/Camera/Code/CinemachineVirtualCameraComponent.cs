using UnityEngine;
using DoubleDash.CodingTools;

namespace DoubleDash.ComponentManagement.CinemachineManagement
{
    public abstract class CinemachineVirtualCameraComponent : MonoBehaviour, INameable
    {
        [Header("Virtual Camera Component Settings")]
        [Tooltip("Virtual Camera Manager with the reference to the Virtual Camera that this component modifies.")]
        public CinemachineVirtualCameraManager virtualCameraManager;

        public string Name { get => virtualCameraManager.TargetCamera.gameObject.name; set => virtualCameraManager.TargetCamera.gameObject.name = value; }
    }
}