using DoubleDash.CodingTools;
using UnityEngine;

namespace DoubleDash.ComponentManagement.CinemachineManagement
{
    public class CinemachineVirtualCameraPriority : CinemachineVirtualCameraComponent
    {
        [Header("Priority Settings")]
        [Tooltip("Base priority of this camera.\n\nNormally, the camera with the highest priority is the one set as the active camera by the Cinemachine brain.")]
        public VariableReference<int> basePriority;

        public void Awake()
        {
            virtualCameraManager.TargetCamera.Priority = basePriority.Value;
        }

        public void MoveToTopOfQueue() {
            virtualCameraManager.TargetCamera.MoveToTopOfPrioritySubqueue();
        }

        //Changes the base priority to the given value.
        public void SetBasePriority(int i)
        {
            virtualCameraManager.TargetCamera.Priority = basePriority.Value + i;
        }

        //Resets the camera's priority to the basePriority.
        public void ResetPriority()
        {
            virtualCameraManager.TargetCamera.Priority = basePriority.Value;
        }

        //Changes the priority to the given value.
        public void SetPriority(int i)
        {
            virtualCameraManager.TargetCamera.Priority = i;
        }

        //Modifies the priority by the given number.
        public void ModifyPriority(int i)
        {
            virtualCameraManager.TargetCamera.Priority += i;
        }

        //Modifies the priority to be equal to the basePriority, modified by the given number.
        public void ModifyPriorityFromBasePriority(int i)
        {
            virtualCameraManager.TargetCamera.Priority = basePriority.Value + i;
        }
    }
}