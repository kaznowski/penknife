using UnityEngine;
using Cinemachine;

namespace DoubleDash.ComponentManagement.CinemachineManagement
{
    public class CinemachineVirtualCameraConfiner : CinemachineVirtualCameraComponent
    {
        //Reference to the confiner component of the targeted camera.
        CinemachineConfiner _confiner;

        public CinemachineConfiner Confiner
        {
            get 
            {
                //Get component if necessary and return
                if (_confiner == null) _confiner = virtualCameraManager.TargetCamera.GetComponent<CinemachineConfiner>();
                return _confiner;
            }
        }

        public void SetConfineMode(Cinemachine.CinemachineConfiner.Mode mode) {
            Confiner.m_ConfineMode = mode;
        }

        public void SetConfiner2D(Collider2D boundingShape)
        {
            Confiner.m_BoundingShape2D = boundingShape;
        }

        public void SetConfiner(Collider boundingVolume)
        {
            Confiner.m_BoundingVolume = boundingVolume;
        }

        public void RemoveConfiner() => Confiner.m_BoundingVolume = null;

        public void RemoveConfiner2D() => Confiner.m_BoundingShape2D = null;

        public void RemoveAllConfiners()
        {
            RemoveConfiner();
            RemoveConfiner2D();
        }

        public void SetDamping(int i)
        {
            Confiner.m_Damping = i;
        }
    }
}