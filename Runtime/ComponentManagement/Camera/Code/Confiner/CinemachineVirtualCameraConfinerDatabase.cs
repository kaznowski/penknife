using UnityEngine;
using DoubleDash.CodingTools.GenericDatabases;
using System.Collections.Generic;

namespace DoubleDash.ComponentManagement.CinemachineManagement
{
    public interface ICinemachineVirtualCameraConfinerDatabase
    {
        public void SetConfineMode(Cinemachine.CinemachineConfiner.Mode mode);

        public void SetConfiner2D(Collider2D boundingShape);

        public void SetConfiner(Collider boundingVolume);

        public void SetDamping(int i);

        public void RemoveConfiner();

        public void RemoveConfiner2D();

        public void RemoveAllConfiners();
    }

    [System.Serializable]
    public class CinemachineVirtualCameraConfinerDatabase : Database<CinemachineVirtualCameraConfiner>, ICinemachineVirtualCameraConfinerDatabase
    {
        public void SetConfineMode(Cinemachine.CinemachineConfiner.Mode mode)
        {
            foreach (KeyValuePair<string, CinemachineVirtualCameraConfiner> pair in Dictionary) 
            {
                if (pair.Value != null) pair.Value.Confiner.m_ConfineMode = mode;
            }
        }

        public void SetConfiner2D(Collider2D boundingShape)
        {
            foreach (KeyValuePair<string, CinemachineVirtualCameraConfiner> pair in Dictionary)
            {
                if (pair.Value != null) pair.Value.Confiner.m_BoundingShape2D = boundingShape;
            }
        }

        public void SetConfiner(Collider boundingVolume)
        {
            foreach (KeyValuePair<string, CinemachineVirtualCameraConfiner> pair in Dictionary)
            {
                if (pair.Value != null) pair.Value.Confiner.m_BoundingVolume = boundingVolume;
            }
        }

        public void RemoveConfiner() 
        {
            foreach (KeyValuePair<string, CinemachineVirtualCameraConfiner> pair in Dictionary)
            {
                if (pair.Value != null) pair.Value.RemoveConfiner();
            }
        }

        public void RemoveConfiner2D() 
        {
            foreach (KeyValuePair<string, CinemachineVirtualCameraConfiner> pair in Dictionary)
            {
                if (pair.Value != null) pair.Value.RemoveConfiner2D();
            }
        }

        public void RemoveAllConfiners()
        {
            foreach (KeyValuePair<string, CinemachineVirtualCameraConfiner> pair in Dictionary)
            {
                if (pair.Value != null) pair.Value.RemoveAllConfiners();
            }
        }

        public void SetDamping(int i)
        {
            foreach (KeyValuePair<string, CinemachineVirtualCameraConfiner> pair in Dictionary)
            {
                if (pair.Value != null) pair.Value.Confiner.m_Damping = i;
            }
        }
    }
}

