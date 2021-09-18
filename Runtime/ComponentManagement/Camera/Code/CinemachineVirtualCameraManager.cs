using UnityEngine;
using Cinemachine;

namespace DoubleDash.ComponentManagement.CinemachineManagement
{
    public class CinemachineVirtualCameraManager : MonoBehaviour
    {
        [Tooltip("Camera that this manager controls.")]
        [SerializeField] private CinemachineVirtualCamera _targetCamera;

        public CinemachineVirtualCamera TargetCamera {
            get {
                return _targetCamera;
            }
            set {
                _targetCamera = value;
            }
        }
    }
}
